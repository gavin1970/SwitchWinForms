using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SwitchWinForms
{
    /// <summary>
    /// Building custom boxes on your form.
    /// </summary>
    public class SWFBoxing : BoxMethods
    {
        #region Public Form Events and Delegates
        //public delegate void SWFExceptionEventHandler(object sender, SWFMessageEventArgs e);
        public delegate void SWFMessageHandler(object sender, SWFMessageEventArgs e);
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;
        public event EventHandler MouseLeave;
        public event EventHandler Resize;
        public event PaintEventHandler Paint;
        public event KeyEventHandler KeyUp;
        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event SWFMessageHandler Exception;
        public event SWFMessageHandler Success;
        #endregion

        #region Constructor
        /// <summary>
        /// Building custom boxes on your form.
        /// </summary>
        /// <param name="frm">Form Handle => new SWFBoxing(this, ...)</param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SWFBoxing(Form frm, SWFBoxingOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("Options is required.");
            if (string.IsNullOrWhiteSpace(options.Name))
                throw new ArgumentNullException($"Name property for this box is required.\nType: {options.Type}\nText: {options.TextSetup.Text}");

            if (GetZIndex(ref options, out int z))
                throw new ArgumentNullException($"'{options.Name}' already exists.");

            options.TextSetup.TextCursor = options.TextSetup.Text.Length;

            this.SetBoxOptions = options;
            this.BoxingOptions.Parent = frm;
            this.BoxingOptions.ZIndex = z;

            BoxingLoc = new Rectangle(this.BoxingOptions.Location, this.BoxingOptions.Size);

            SetupOptions();
        }
        #endregion

        #region Private Properties
        private Rectangle BoxingLoc { get; set; } = new Rectangle();
        private bool IgnoreKeyPress { get; set; } = false;
        private bool ButtonClicked { get; set; } = false;
        private bool InputStart { get; set; } = false;
        private bool IsInBounds { get; set; } = false;
        private bool CursorChanged { get; set; } = false;
        private string DefaultTip { get; set; } = string.Empty;
        private SWFBoxingOptions SetBoxOptions { get; set; } = new SWFBoxingOptions();
        #endregion

        #region Public Properties
        /// <summary>
        /// All setup options for the object your creating.  Can be passed in with Constructor or this property.
        /// </summary>
        public SWFBoxingOptions BoxingOptions { get { return SetBoxOptions; } }
        /// <summary>
        /// Get bounderies for this object.
        /// </summary>
        public Rectangle Rectangle { get { return BoxingLoc; } }
        /// <summary>
        /// Get location for this object.
        /// </summary>
        public Point Location { get { return BoxingLoc.Location; } }
        /// <summary>
        /// Get size for this object.
        /// </summary>
        public Size Size { get { return BoxingLoc.Size; } }
        /// <summary>
        /// Get left boundary for this object.
        /// </summary>
        public int Left { get { return BoxingLoc.X; } }
        /// <summary>
        /// Get top boundary for this object.
        /// </summary>
        public int Top { get { return BoxingLoc.Y; } }
        /// <summary>
        /// Get right boundary for this object.
        /// </summary>
        public int Right { get { return BoxingLoc.X + BoxingLoc.Width; } }
        /// <summary>
        /// Get bottom boundary for this object.
        /// </summary>
        public int Bottom { get { return BoxingLoc.Y + BoxingLoc.Height; } }
        #endregion

        #region Private Keyboard Events
        private void MR_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }
        private void MR_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.BoxingOptions != null && this.InputStart)
            {
                bool changes = false;
                string text = this.BoxingOptions.TextSetup.Text;

                //KeyPress doesn't capture some keys.  We must handle them here.
                if (e.KeyCode == Keys.Right)
                {
                    this.BoxingOptions.TextSetup.TextCursor++;
                    changes = true;
                }
                else if (e.KeyCode == Keys.Left)
                {
                    this.BoxingOptions.TextSetup.TextCursor--;
                    changes = true;
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    Console.WriteLine($"KeyDown: {e.KeyCode}");
                    //only used for Delete at the moment.
                    if (!string.IsNullOrEmpty(text))
                    {
                        int textCursor = this.BoxingOptions.TextSetup.TextCursor;
                        if (text.Length - (textCursor) > 0)
                        {
                            string newText = text.Substring(0, textCursor);
                            newText += text.Substring(textCursor + 1, text.Length - (textCursor + 1));
                            text = newText;
                            changes = true;

                            if (textCursor - 1 > text.Length)
                                this.BoxingOptions.TextSetup.TextCursor--;
                        }
                    }
                    else
                        Console.WriteLine($"KeyDown: Nothing to {Keys.Delete}");
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    bool valid = true;
                    string validMessage = "No expression found";

                    lock (SetBoxOptions)
                        this.IgnoreKeyPress = true;

                    Console.WriteLine($"KeyDown: {e.KeyCode}");
                    if (this.BoxingOptions.TextSetup.ValidInputExpression != null)
                    {
                        string pattern = this.BoxingOptions.TextSetup.ValidInputExpression;
                        valid = Regex.Match(text, pattern, RegexOptions.IgnoreCase).Success;
                        if (valid)
                            validMessage = "Expression is valid";
                    }

                    if (valid)
                    {
                        changes = true;
                        this.InputStart = false;
                        this.BoxingOptions.TextSetup.TextCursor = text.Length;
                        Success?.Invoke(this,
                                new SWFMessageEventArgs(
                                    validMessage
                                ));
                    }
                    else
                    {
                        Exception?.Invoke(this,
                                new SWFMessageEventArgs(
                                    new Exception("Failed regex expression.")
                                ));
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    lock (SetBoxOptions)
                    {
                        this.IgnoreKeyPress = true;
                        this.InputStart = false;
                    }

                    Console.WriteLine($"KeyDown: {e.KeyCode}");
                    changes = true;
                    text = this.DefaultTip;
                    this.BoxingOptions.TextSetup.TextCursor = text.Length;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    Console.WriteLine($"KeyDown: {e.KeyCode}");
                    int textCursor = this.BoxingOptions.TextSetup.TextCursor;
                    if (!string.IsNullOrEmpty(text) && textCursor > 0)
                    {
                        string newText = text.Substring(0, textCursor - 1);
                        int size = text.Length > textCursor ? text.Length - textCursor : textCursor - text.Length;
                        newText += text.Substring(textCursor, size);
                        text = newText;

                        changes = true;
                        this.BoxingOptions.TextSetup.TextCursor--;
                    }
                    else
                        Console.WriteLine($"Nothing to {e.KeyCode}");

                    lock (SetBoxOptions)
                        this.IgnoreKeyPress = true;
                }

                if (this.BoxingOptions.TextSetup.TextCursor < 0)
                    this.BoxingOptions.TextSetup.TextCursor = 0;
                else if (this.BoxingOptions.TextSetup.TextCursor > text.Length)
                    this.BoxingOptions.TextSetup.TextCursor = text.Length;

                if (changes)
                {
                    this.BoxingOptions.TextSetup.Text = text;
                    this.BoxingOptions.Parent.Invalidate();
                }
            }

            KeyDown?.Invoke(this, e);
        }
        private void MR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.BoxingOptions != null && this.InputStart)
            {
                lock (SetBoxOptions)
                {
                    if (!this.IgnoreKeyPress)
                    {
                        Console.WriteLine($"KeyPress: {e.KeyChar}");

                        string text = this.BoxingOptions.TextSetup.Text;
                        int textCursor = this.BoxingOptions.TextSetup.TextCursor;

                        string newText = string.IsNullOrEmpty(text) || textCursor == 0 ? "" : 
                                                    text.Substring(0, textCursor);
                        string subText = string.IsNullOrEmpty(text) || textCursor == 0 ? "" : 
                                                    text.Substring(textCursor, text.Length - textCursor);

                        this.BoxingOptions.TextSetup.TextCursor++;

                        text = newText;
                        text += e.KeyChar.ToString();
                        text += subText;

                        this.BoxingOptions.TextSetup.Text = text;
                        this.BoxingOptions.Parent.Invalidate();

                        if (this.BoxingOptions.TextSetup.TextCursor < 0)
                            this.BoxingOptions.TextSetup.TextCursor = 0;
                        else if (this.BoxingOptions.TextSetup.TextCursor > text.Length)
                            this.BoxingOptions.TextSetup.TextCursor = text.Length;
                    }
                    else
                        Console.WriteLine($"KeyPress: Ignoring {e.KeyChar}");

                    this.IgnoreKeyPress = false;
                }
            }

            KeyPress?.Invoke(this, e);
        }
        #endregion

        #region Private Mouse Events
        private void MR_MouseLeave(object sender, EventArgs e)
        {
            if (CursorChanged && this.BoxingOptions != null)
            {
                CursorChanged = false;
                this.BoxingOptions.Parent.Cursor = Cursors.Default;
            }
            this.MouseLeave?.Invoke(this, e);
        }
        private void MR_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.BoxingOptions != null && this.ButtonClicked && this.IsInBounds)
            {
                this.ButtonClicked = false;
                this.BoxingOptions.Location = new Point(this.BoxingOptions.Location.X - this.BoxingOptions.Shadowing.Depth,
                                                        this.BoxingOptions.Location.Y - this.BoxingOptions.Shadowing.Depth);
                this.BoxingOptions.Parent.Invalidate();
            }

            if (this.IsInBounds)
                MouseUp?.Invoke(this, e);
        }
        private void MR_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.BoxingOptions == null)
                return;

            if (IsClickable() && this.IsInBounds)
            {
                if (ImOnTop(this.BoxingOptions))
                {
                    if (this.BoxingOptions.Type == SWF_BOX_TYPE.TextBox)
                    {
                        this.InputStart = true;
                        string text = this.BoxingOptions.TextSetup.Text;
                        this.DefaultTip = text;

                        if (text.StartsWith("[") && text.EndsWith("]"))
                            this.BoxingOptions.TextSetup.Text = "";
                        this.BoxingOptions.Parent.Invalidate();
                    }
                    else if (this.BoxingOptions.Type == SWF_BOX_TYPE.Button && this.BoxingOptions.Shadowing.Depth > 0)
                    {
                        this.ButtonClicked = true;
                        this.BoxingOptions.Location = new Point(this.BoxingOptions.Location.X + this.BoxingOptions.Shadowing.Depth,
                                                                this.BoxingOptions.Location.Y + this.BoxingOptions.Shadowing.Depth);
                        this.BoxingOptions.Parent.Invalidate();
                    }

                    this.MouseDown?.Invoke(this, e);
                }
            }
        }
        private void MR_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.BoxingOptions == null)
                return;

            if(Global.IsInBounds(this.BoxingOptions.Parent, e.Location, this.BoxingLoc))
            {
                if (ImOnTop(this.BoxingOptions))
                {
                    this.IsInBounds = true;
                    bool isClickable = IsClickable();
                    Cursor cursor = GetCursorByType(this.BoxingOptions.Parent, this.BoxingOptions.Cursors.OnMove, out bool cursorChanged);

                    if (isClickable && cursorChanged)
                    {
                        this.BoxingOptions.Parent.Cursor = cursor;
                        this.CursorChanged = cursorChanged;
                    }

                    this.MouseMove?.Invoke(this, e);
                }
            }
            else
            {
                //check to see if this object was in bounds, if not, then ignore..
                if (this.IsInBounds)
                {
                    this.IsInBounds = false;
                    this.BoxingOptions.Parent.Cursor = Cursors.Default;
                    this.CursorChanged = true;
                    this.MouseLeave?.Invoke(this, e);
                }

                if (this.ButtonClicked)
                {
                    this.ButtonClicked = false;
                    this.BoxingOptions.Location = new Point(this.BoxingOptions.Location.X - this.BoxingOptions.Shadowing.Depth,
                                                            this.BoxingOptions.Location.Y - this.BoxingOptions.Shadowing.Depth);
                    this.BoxingOptions.Parent.Invalidate();
                }
            }
        }
        #endregion

        #region Private Form Events
        private void MR_Resize(object sender, EventArgs e)
        {
            this.BoxingLoc = new Rectangle(this.BoxingOptions.Location, this.BoxingOptions.Size);
            this.BoxingOptions.Parent.Invalidate();
            this.Resize?.Invoke(this, e);
        }
        private void MR_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;
            Graphics g = e.Graphics;
            if (g != null && 
                this.BoxingOptions.Location.X < this.BoxingOptions.Parent.Width && 
                this.BoxingOptions.Location.Y < this.BoxingOptions.Parent.Height)
            {
                if (this.BoxingOptions.Type == SWF_BOX_TYPE.FormBody)
                {
                    this.BoxingOptions.Location = new Point(0, this.BoxingOptions.TitleBarHeight - this.BoxingOptions.BorderSize);
                    this.BoxingOptions.Size = new Size(this.BoxingOptions.Parent.Width, 
                                                       this.BoxingOptions.Parent.Height - this.BoxingOptions.TitleBarHeight);

                    //cannot do shadowing on the form, as it's not able to draw off the form.  
                    //possibly in the future, sizing of the form instead so shadow shows up,
                    //however, since the real form is pink, it might not work.
                    this.BoxingOptions.Shadowing = null;
                }

                Rectangle titleBG = new Rectangle(this.BoxingOptions.Location, this.BoxingOptions.Size);

                BoxInfo bi = Global.CreateBox(g, new BoxInfo()
                {
                    BackColor = this.BoxingOptions.BackColor,
                    ForeColor = this.BoxingOptions.TextSetup.Color,
                    BorderColor = this.BoxingOptions.BorderColor,
                    BorderSize = this.BoxingOptions.BorderSize,
                    Rectangle = titleBG,
                    HAlign = this.BoxingOptions.TextSetup.HorizonalAlign,
                    VAlign = this.BoxingOptions.TextSetup.VerticalAlign,
                    Font = this.BoxingOptions.TextSetup.Font,
                    Text = this.BoxingOptions.TextSetup.Text,
                    Padding = this.BoxingOptions.Padding,
                    Shadowing = this.BoxingOptions.Shadowing,
                    BackgroundImage = this.BoxingOptions.TextSetup.BackgroundImage,
                    RoundedCorners = this.BoxingOptions.RoundedCorners,
                    RoundedRadius = this.BoxingOptions.RoundedRadius,
                    BackgroundImageLayout = this.BoxingOptions.TextSetup.BackgroundImageLayout,
                    ReplaceImageColor = this.BoxingOptions.TextSetup.ReplaceImageColor,
                    ButtonClicked = this.ButtonClicked,
                    TextCursor = this.BoxingOptions.TextSetup.TextCursor,
                    Type = this.BoxingOptions.Type,
                    InputStart = this.InputStart,
                });
                
                if (bi.BackgroundImageModified)
                {
                    this.BoxingOptions.TextSetup.BackgroundImage = bi.BackgroundImage;
                    this.BoxingOptions.TextSetup.ReplaceImageColor = bi.ReplaceImageColor;
                }

                BoxingLoc = new Rectangle(this.BoxingOptions.Location, this.BoxingOptions.Size);
            }
            this.Paint?.Invoke(this, e);
        }
        #endregion

        #region Private Methods
        private bool IsClickable()
        {
            if (this.MouseDown != null ||
                this.MouseUp != null || 
                this.BoxingOptions.Type == SWF_BOX_TYPE.Button ||
                this.BoxingOptions.Type == SWF_BOX_TYPE.TextBox)
                return true;
            else
                return false;
        }
        private void SetupOptions()
        {
            this.BoxingOptions.Parent.MouseMove += MR_MouseMove;
            this.BoxingOptions.Parent.MouseLeave += MR_MouseLeave;
            this.BoxingOptions.Parent.MouseUp += MR_MouseUp;
            this.BoxingOptions.Parent.MouseDown += MR_MouseDown;
            this.BoxingOptions.Parent.Paint += MR_Paint;
            this.BoxingOptions.Parent.Resize += MR_Resize;
            this.BoxingOptions.Parent.KeyPress += MR_KeyPress;
            this.BoxingOptions.Parent.KeyUp += MR_KeyUp;
            this.BoxingOptions.Parent.KeyDown += MR_KeyDown;
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Load image from file.  Supported files types are (gif,bmp,ico,tif,jpg,png).
        /// </summary>
        public static object ImageFromFile<T>(string path)
        {
            if (typeof(T).Name.ToLower() == "image")
                return (object)Global.GetImage(path);
            else if (typeof(T).Name.ToLower() == "icon")
                return (object)Global.GetIcon(path);
            else
                return null;
        }
        #endregion
    }
}

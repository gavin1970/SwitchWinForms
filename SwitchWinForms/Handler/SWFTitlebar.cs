using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace SwitchWinForms
{
    /// <summary>
    /// Creating a custome Titlebar, this replaces your exists.  Do not use more that one instnace per project.
    /// </summary>
    public partial class SWFTitlebar : BoxMethods
    {
        #region Constructor
        public SWFTitlebar(Form frm, SWFTitlebarOptions options = null)
        {
            if (frm == null)
            {
                SetLastError = new ArgumentNullException("Missing Form Object");
                throw SetLastError;
            }

            if (frm.FormBorderStyle != FormBorderStyle.None)
                frm.FormBorderStyle = FormBorderStyle.None;
            
            //Ensure doublebuffered set on form.
            //This stops major flickering.
            SetDoubleBuffered(frm);

            frm.BackColor = Color.FromArgb(192, 0, 191);
            frm.TransparencyKey = frm.BackColor;

            if (frm.MinimumSize.Width == 0 &&
                frm.MinimumSize.Height == 0)
                frm.MinimumSize = new Size(100, 100);

            TitleOptions = options ?? new SWFTitlebarOptions();
            TitleOptions.GetSetParent = frm;

            if (TitleOptions.TextSetup != null && !TitleOptions.TextSetup.Empty)
                TitleOptions.Parent.Text = TitleOptions.TextSetup?.Text;

            SetupOptions();
        }
        #endregion

        #region Private Form Events
        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            this.Drag = false;
        }
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Close) == SWF_TITLE_BUTTON.Close  
                && Global.IsInBounds(TitleOptions.Parent, e.Location, this.BtnCloseRec, Cursors.Hand))
            {
                TitleOptions.Parent.Close();
            }
            else if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Minimize) == SWF_TITLE_BUTTON.Minimize 
                && Global.IsInBounds(TitleOptions.Parent, e.Location, this.BtnMinRec, Cursors.Hand))
            {
                TitleOptions.Parent.WindowState = FormWindowState.Minimized;
            }
            else if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Maximize_Normal) == SWF_TITLE_BUTTON.Maximize_Normal 
                && Global.IsInBounds(TitleOptions.Parent, e.Location, this.BtnMaxRec, Cursors.Hand))
            {
                
                if (TitleOptions.Parent.WindowState == FormWindowState.Maximized)
                    TitleOptions.Parent.WindowState = FormWindowState.Normal;
                else
                    TitleOptions.Parent.WindowState = FormWindowState.Maximized;
            }
            else if (TitleOptions.Height > e.Location.Y)
            {
                this.StartPoint = e.Location;
                this.Drag = true;
            }
        }
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Drag)
            {
                // if we should be dragging it, we need to figure out some movement
                Point p1 = new Point(e.X, e.Y);
                Point p2 = TitleOptions.Parent.PointToScreen(p1);
                Point p3 = new Point(p2.X - this.StartPoint.X,
                                     p2.Y - this.StartPoint.Y);

                TitleOptions.Parent.Location = p3;
            } 
            else
            {
                if (Global.IsInBounds(TitleOptions.Parent, e.Location, TitleOptions.Rectangle) 
                    && TitleOptions.Parent.Cursor != Cursors.Default)
                {
                    int x = 0;
                    int t = 0;
                    int b = 0;

                    if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Minimize) == SWF_TITLE_BUTTON.Minimize)
                    {
                        x = this.BtnMinRec.Left;
                        t = this.BtnMinRec.Top;
                        b = this.BtnMinRec.Top + this.BtnMinRec.Height;
                    }
                    else if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Maximize_Normal) == SWF_TITLE_BUTTON.Maximize_Normal)
                    {
                        x = this.BtnMaxRec.Left;
                        t = this.BtnMaxRec.Top;
                        b = this.BtnMaxRec.Top + this.BtnMaxRec.Height;
                    }
                    else if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Close) == SWF_TITLE_BUTTON.Close)
                    {
                        x = this.BtnCloseRec.Left;
                        t = this.BtnCloseRec.Top;
                        b = this.BtnCloseRec.Top + this.BtnCloseRec.Height;
                    }

                    if (e.X < x || e.Y < t || e.Y > b)
                        TitleOptions.Parent.Cursor = Cursors.Default;
                }
            }
        }
        private void Form_Resize(object sender, EventArgs e)
        {
            if ((TitleOptions.Parent.MinimumSize.Width == 0 &&
                TitleOptions.Parent.MinimumSize.Height == 0) ||
                (TitleOptions.Parent.MinimumSize.Width == 100 &&
                TitleOptions.Parent.MinimumSize.Height == 100))
            {
                int w = 0;
                int fWidth = TitleOptions.Parent.Width + (this.BtnMinRec.Width / 2);

                if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Minimize) == SWF_TITLE_BUTTON.Minimize && this.BtnMinRec.Height > 0)
                    w = fWidth - this.BtnMinRec.Left;
                else if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Maximize_Normal) == SWF_TITLE_BUTTON.Maximize_Normal && this.BtnMaxRec.Height > 0)
                    w = fWidth - this.BtnMaxRec.Left;
                else if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Close) == SWF_TITLE_BUTTON.Close && this.BtnCloseRec.Height > 0)
                    w = fWidth - this.BtnCloseRec.Left;

                if (w > 0)
                    TitleOptions.Parent.MinimumSize = new Size(w, 100);
            }

            TitleOptions.Parent.Invalidate();
        }
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (g != null)
            {
                TitleOptions.Location = new Point(0, 0);
                TitleOptions.Size = new Size(TitleOptions.Parent.Width, TitleOptions.Height);

                /*if ((TitleOptions.RoundedCorners & SWF_ARC_CORNERS.TopLeft) > 0 && TitleOptions.TextSetup.HorizonalAlign == StringAlignment.Near)
                {
                    float f = ((float)TitleOptions.RoundedRadius) * 0.5F;
                    int extra = (int)Math.Floor(f);
                    TitleOptions.Padding = new Padding(TitleOptions.Padding.Left + extra, TitleOptions.Padding.Top, TitleOptions.Padding.Right, TitleOptions.Padding.Bottom);
                }*/

                BoxInfo boxInfo = new BoxInfo()
                {
                    BackColor = TitleOptions.BackColor,
                    ForeColor = TitleOptions.TextSetup.Color,
                    BorderColor = TitleOptions.BorderColor,
                    BorderSize = TitleOptions.BorderSize,
                    Rectangle = TitleOptions.Rectangle,
                    HAlign = TitleOptions.TextSetup.HorizonalAlign,
                    VAlign = TitleOptions.TextSetup.VerticalAlign,
                    Font = TitleOptions.TextSetup.Font,
                    Text = TitleOptions.TextSetup.Text,
                    Padding = TitleOptions.Padding,
                    BackgroundImage = TitleOptions.TextSetup.BackgroundImage,
                    RoundedCorners = TitleOptions.RoundedCorners,
                    RoundedRadius = TitleOptions.RoundedRadius,
                    BackgroundImageLayout = TitleOptions.TextSetup.BackgroundImageLayout,
                    ReplaceImageColor = TitleOptions.TextSetup.ReplaceImageColor,
                    Type = SWF_BOX_TYPE.Titlebar,
                    Icon = TitleOptions.Icon
                };

                BoxInfo bi = Global.CreateBox(g, boxInfo);      //title

                if (bi.BackgroundImageModified)
                {
                    TitleOptions.TextSetup.BackgroundImage = bi.BackgroundImage;
                    TitleOptions.TextSetup.ReplaceImageColor = bi.ReplaceImageColor;
                }

                CreateButtons(g, boxInfo);  //title buttons
            }
        }
        #endregion

        #region Private Methods
        private bool SetDoubleBuffered(Form frm)
        {
            bool retVal = false;
            if (SystemInformation.TerminalServerSession)
            {
                SetLastError = new Exception("## WARN ##, Flicking may occur, because your in a terminal session.");
                return retVal;
            }

            if (frm != null)
            {
                try
                {
                    PropertyInfo aProp =
                        typeof(Control).GetProperty("DoubleBuffered",
                            BindingFlags.NonPublic | BindingFlags.Instance);

                    aProp.SetValue(frm, true, null);
                    retVal = true;
                }
                catch (Exception ex)
                {
                    SetLastError = ex;
                }
            }
            else
                SetLastError = new Exception("## WARN ##, Missing Form object.");

            return retVal;
        }
        private void CreateButtons(Graphics g, BoxInfo boxInfo)
        {
            int buttonPadding = 2;
            Rectangle titleRect = boxInfo.Rectangle;
            Rectangle btnRect = boxInfo.Rectangle;
            BoxInfo newBtnSetup;

            btnRect.Height = titleRect.Height - (TitleOptions.Padding.Top + TitleOptions.Padding.Bottom) - (boxInfo.BorderSize * 2);
            btnRect.Width = btnRect.Height;

            float f = ((float)TitleOptions.RoundedRadius) * 0.5F;
            int extra = (int)Math.Floor(f);

            if (TitleOptions.Icon != null)
            {
                btnRect.X = TitleOptions.Padding.Left + TitleOptions.BorderSize;
                btnRect.Y = TitleOptions.Padding.Top + TitleOptions.BorderSize;

                if ((TitleOptions.RoundedCorners & SWF_ARC_CORNERS.TopLeft) > 0) 
                    btnRect.X += extra;

                newBtnSetup = new BoxInfo()
                {
                    Rectangle = btnRect,
                    BackgroundImageLayout = SWF_BG_IMG_LAYOUT.Normal,
                    BorderSize = TitleOptions.Close.BorderSize,
                    BorderColor = TitleOptions.Close.BorderColor,
                    RoundedCorners = TitleOptions.Close.RoundedCorners,
                    RoundedRadius = TitleOptions.Close.RoundedRadius,
                    BackgroundImage = TitleOptions.Icon,
                };
                //newBtnSetup.Rectangle = new Rectangle(new Point(30, 0), new Size(200, 200));

                Global.CreateBox(g, newBtnSetup);
            }

            btnRect.X = titleRect.Width - btnRect.Width - TitleOptions.Padding.Right - TitleOptions.BorderSize;
            btnRect.Y = boxInfo.Rectangle.Y + TitleOptions.Padding.Top + TitleOptions.BorderSize;

            if ((TitleOptions.RoundedCorners & SWF_ARC_CORNERS.TopRight) > 0)
                btnRect.X -= extra;

            if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Close) > 0)
            {
                newBtnSetup = new BoxInfo()
                {
                    BackColor = TitleOptions.Close.BackColor,
                    ForeColor = TitleOptions.Close.TextSetup.Color,
                    BorderSize = TitleOptions.Close.BorderSize,
                    BorderColor = TitleOptions.Close.BorderColor,
                    Font = TitleOptions.Close.TextSetup.Font,
                    Padding = TitleOptions.Close.Padding,
                    HAlign = TitleOptions.Close.TextSetup.HorizonalAlign,
                    VAlign = TitleOptions.Close.TextSetup.VerticalAlign,
                    Rectangle = btnRect,
                    Text = TitleOptions.Close.TextSetup.Text,
                    RoundedCorners = TitleOptions.Close.RoundedCorners,
                    RoundedRadius = TitleOptions.Close.RoundedRadius
                };

                this.BtnCloseRec = newBtnSetup.Rectangle;
                Global.CreateBox(g, newBtnSetup);
                btnRect.X -= (btnRect.Width + buttonPadding + boxInfo.BorderSize);
                newBtnSetup.Rectangle = btnRect;
            }
            else
                this.BtnCloseRec = new Rectangle();

            if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Maximize_Normal) > 0)
            {
                if (TitleOptions.Parent.WindowState == FormWindowState.Maximized)
                {
                    newBtnSetup = new BoxInfo()
                    {
                        BackColor = TitleOptions.Maximize.BackColor,
                        ForeColor = TitleOptions.Maximize.TextSetup.Color,
                        BorderSize = TitleOptions.Maximize.BorderSize,
                        BorderColor = TitleOptions.Maximize.BorderColor,
                        Font = TitleOptions.Maximize.TextSetup.Font,
                        Padding = TitleOptions.Maximize.Padding,
                        HAlign = TitleOptions.Maximize.TextSetup.HorizonalAlign,
                        VAlign = TitleOptions.Maximize.TextSetup.VerticalAlign,
                        Rectangle = btnRect,
                        Text = TitleOptions.Maximize.TextSetup.Text,
                        RoundedCorners = TitleOptions.Maximize.RoundedCorners,
                        RoundedRadius = TitleOptions.Maximize.RoundedRadius
                    };
                }
                else
                {
                    newBtnSetup = new BoxInfo()
                    {
                        BackColor = TitleOptions.Normal.BackColor,
                        ForeColor = TitleOptions.Normal.TextSetup.Color,
                        BorderSize = TitleOptions.Normal.BorderSize,
                        BorderColor = TitleOptions.Normal.BorderColor,
                        Font = TitleOptions.Normal.TextSetup.Font,
                        Padding = TitleOptions.Normal.Padding,
                        HAlign = TitleOptions.Normal.TextSetup.HorizonalAlign,
                        VAlign = TitleOptions.Normal.TextSetup.VerticalAlign,
                        Rectangle = btnRect,
                        Text = TitleOptions.Normal.TextSetup.Text,
                        RoundedCorners = TitleOptions.Normal.RoundedCorners,
                        RoundedRadius = TitleOptions.Normal.RoundedRadius
                    };
                }

                this.BtnMaxRec = newBtnSetup.Rectangle;
                Global.CreateBox(g, newBtnSetup);
                btnRect.X -= (btnRect.Width + buttonPadding + boxInfo.BorderSize);
                newBtnSetup.Rectangle = btnRect;
            }
            else
                this.BtnMaxRec = new Rectangle();

            if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Minimize) > 0)
            {
                newBtnSetup = new BoxInfo()
                {
                    BackColor = TitleOptions.Minimized.BackColor,
                    ForeColor = TitleOptions.Minimized.TextSetup.Color,
                    BorderSize = TitleOptions.Minimized.BorderSize,
                    BorderColor = TitleOptions.Minimized.BorderColor,
                    Font = TitleOptions.Minimized.TextSetup.Font,
                    Padding = TitleOptions.Minimized.Padding,
                    HAlign = TitleOptions.Minimized.TextSetup.HorizonalAlign,
                    VAlign = TitleOptions.Minimized.TextSetup.VerticalAlign,
                    Rectangle = btnRect,
                    Text = TitleOptions.Minimized.TextSetup.Text,
                    RoundedCorners = TitleOptions.Minimized.RoundedCorners,
                    RoundedRadius = TitleOptions.Minimized.RoundedRadius
                };

                this.BtnMinRec = newBtnSetup.Rectangle;
                Global.CreateBox(g, newBtnSetup);
                btnRect.X -= (btnRect.Width + buttonPadding + boxInfo.BorderSize);
                newBtnSetup.Rectangle = btnRect;
            }
            else
                this.BtnMinRec = new Rectangle();
        }
        private void SetupOptions()
        {
            TitleOptions.Parent.MouseMove += Form_MouseMove;
            TitleOptions.Parent.MouseUp += Form_MouseUp;
            TitleOptions.Parent.MouseDown += Form_MouseDown;
            TitleOptions.Parent.Paint += Form_Paint;
            TitleOptions.Parent.Resize += Form_Resize;
        }
        #endregion
    }
}

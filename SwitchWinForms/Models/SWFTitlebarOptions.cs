using System;
using System.Drawing;
using System.Windows.Forms;

namespace SwitchWinForms
{
    public class SWFTitlebarOptions : TextUtil
    {
        private int _borderSize = 2;
        /// <summary>
        /// Name of Titlebar Option
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Parent Form
        /// </summary>
        internal Form GetSetParent { get; set; }
        /// <summary>
        /// Titlebar is always location 0,0
        /// </summary>
        internal Point Location { get; set; } = new Point(0, 0);
        /// <summary>
        /// Size is updated by paint.
        /// </summary>
        internal Size Size { get; set; } = new Size(0, 0);
        public Image Icon { get; set; }
        /// <summary>
        /// The primary form.
        /// </summary>
        public Form Parent { get { return GetSetParent; } }
        /// <summary>
        /// Get Location and Size of titlebar.
        /// </summary>
        public Rectangle Rectangle { get { return new Rectangle(this.Location, this.Size); } }
        /// <summary>
        /// Border size. 
        ///     Valid values (0-5).
        ///     Default: 2
        /// </summary>
        public int BorderSize
        { 
            get { return _borderSize; }
            set 
            {
                if (value < 0 || value > 5)
                    throw new Exception("BorderSize must be between 0-5");

                _borderSize = value; 
            }
        }
        /// <summary>
        /// Rounding corners specified.
        /// </summary>
        /// <remarks>
        /// Default: None
        /// </remarks>
        public SWF_ARC_CORNERS RoundedCorners { get; set; } = SWF_ARC_CORNERS.None;
        /// <summary>
        /// Radius of the arc for the rounded corners.
        /// Will be ignored if RoundedCorners is set to None.
        ///     Default: 0
        /// </summary>
        public int RoundedRadius { get; set; } = 0;
        /// <summary>
        /// All text and buttons will be adjusted based on padding.
        ///     Usage: Padding(Left, Top, Right, Bottom)
        ///     Default: 0
        /// </summary>
        public Padding Padding { get; set; } = new Padding(0);
        /// <summary>
        /// Height of titlebar.
        ///     Default: 30
        /// </summary>
        public int Height { get; set; } = 30;
        /// <summary>
        /// Enable the Minimize, Maximum/Normal, and Close buttons for titlebar.
        ///     Usage: SWF_TITLE_BUTTON.Minimize | SWF_TITLE_BUTTON.Close
        ///     Default: SWF_TITLE_BUTTON.All
        /// </summary>
        public SWF_TITLE_BUTTON ButtonsEnabled { get; set; } = SWF_TITLE_BUTTON.All;
        /// <summary>
        /// Setup or use defaults for Minimized button.
        /// </summary>
        /// <remarks>
        /// Default:<br/>
        /// BackColor = SystemColors.Window,<br/>
        /// BorderColor = SystemColors.WindowFrame,<br/>
        /// TextSetup = new TextOptions()<br/>
        /// {<br/>
        ///  Color = SystemColors.WindowText,<br/>
        ///  Text = SWFUnicodeChars.MIN,<br/>
        ///  Font = Global.DefaultButtonFont,<br/>
        ///  HorizonalAlign = StringAlignment.Center,<br/>
        ///  VerticalAlign = StringAlignment.Center,<br/>
        /// }<br/>
        /// </remarks>
        public ButtonsState Minimized { get; set; } = new ButtonsState()
        {
            BackColor = SystemColors.Window,
            BorderColor = SystemColors.WindowFrame,
            TextSetup = new TextOptions()
            {
                Color = SystemColors.WindowText,
                Text = SWFUnicodeChars.MIN,
                Font = Global.DefaultButtonFont,
                HorizonalAlign = StringAlignment.Center,
                VerticalAlign = StringAlignment.Center,
            }
        };
        /// <summary>
        /// Setup or use defaults for Maximize button.
        /// </summary>
        /// <remarks>
        /// Default:<br/>
        /// BackColor = SystemColors.Window,<br/>
        /// BorderColor = SystemColors.WindowFrame,<br/>
        /// TextSetup = new TextOptions()<br/>
        /// {<br/>
        ///  Color = SystemColors.WindowText,<br/>
        ///  Text = SWFUnicodeChars.MAX,<br/>
        ///  Font = Global.DefaultButtonFont,<br/>
        ///  HorizonalAlign = StringAlignment.Center,<br/>
        ///  VerticalAlign = StringAlignment.Center,<br/>
        /// }<br/>
        /// </remarks>
        public ButtonsState Maximize { get; set; } = new ButtonsState()
        {
            BackColor = SystemColors.Window,
            BorderColor = SystemColors.WindowFrame,
            TextSetup = new TextOptions()
            {
                Color = SystemColors.WindowText,
                Text = SWFUnicodeChars.MAX,
                Font = Global.DefaultButtonFont,
                HorizonalAlign = StringAlignment.Center,
                VerticalAlign = StringAlignment.Center,
            }
        };
        /// <summary>
        /// Setup or use defaults for Normal button.
        /// </summary>
        /// <remarks>
        /// Default:
        /// BackColor = SystemColors.Window,<br/>
        /// BorderColor = SystemColors.WindowFrame,<br/>
        /// TextSetup = new TextOptions()<br/>
        /// {<br/>
        ///  Color = SystemColors.WindowText,<br/>
        ///  Text = SWFUnicodeChars.NORM,<br/>
        ///  Font = Global.DefaultButtonFont,<br/>
        ///  HorizonalAlign = StringAlignment.Center,<br/>
        ///  VerticalAlign = StringAlignment.Center,<br/>
        /// }<br/>
        /// </remarks>
        public ButtonsState Normal { get; set; } = new ButtonsState()
        {
            BackColor = SystemColors.Window,
            BorderColor = SystemColors.WindowFrame,
            TextSetup = new TextOptions()
            {
                Color = SystemColors.WindowText,
                Text = SWFUnicodeChars.NORM,
                Font = Global.DefaultButtonFont,
                HorizonalAlign = StringAlignment.Center,
                VerticalAlign = StringAlignment.Center,
            }
        };
        /// <summary>
        /// Setup or use defaults for Close button.
        /// </summary>
        /// <remarks>
        /// Default:
        /// BackColor = SystemColors.Window,<br/>
        /// BorderColor = SystemColors.WindowFrame,<br/>
        /// TextSetup = new TextOptions()<br/>
        /// {<br/>
        ///  Color = SystemColors.WindowText,<br/>
        ///  Text = Global.GetUnicode(SWF_UNICODE_BUTTONS.CLOSE),<br/>
        ///  Font = Global.DefaultButtonFont,<br/>
        ///  HorizonalAlign = StringAlignment.Center,<br/>
        ///  VerticalAlign = StringAlignment.Center,<br/>
        /// }<br/>
        /// </remarks>
        public ButtonsState Close { get; set; } = new ButtonsState()
        {
            BackColor = SystemColors.Window,
            BorderColor = SystemColors.WindowFrame,
            TextSetup = new TextOptions()
            {
                Color = SystemColors.WindowText,
                Text = SWFUnicodeChars.CLOSE,
                Font = Global.DefaultButtonFont,
                HorizonalAlign = StringAlignment.Center,
                VerticalAlign = StringAlignment.Center,
            }
        };
        /// <summary>
        /// Titlebar caption information
        /// </summary>
        public TextOptions TextSetup { get; set; } = new TextOptions();
        /// <summary>
        /// Titlebar background color.
        /// Default: SystemColors.GradientActiveCaption
        /// </summary>
        public Color BackColor { get; set; } = SystemColors.GradientActiveCaption;
        /// <summary>
        /// Titlebar border color.
        /// Default: SystemColors.ActiveBorder
        /// </summary>
        public Color BorderColor { get; set; } = SystemColors.ActiveBorder;
        /// <summary>
        /// All button setup shown on the title bar.  This includes, Minimum, Normal/Maximum, Close buttons.
        /// </summary>
        public class ButtonsState : TextUtil
        {
            /// <summary>
            /// Border size around buttons.<br/>
            /// Default: 1
            /// </summary>
            public int BorderSize { get; set; } = 1;
            /// <summary>
            /// Separations between buttons.
            /// Default: 2
            /// </summary>
            public int MarginRight { get; set; } = 2;
            /// <summary>
            /// Buttons border color<br/>
            /// Default: SystemColors.ActiveBorder
            /// </summary>
            public Color BorderColor { get; set; } = SystemColors.ActiveBorder;
            /// <summary>
            /// Button Background Color<br/>
            /// Default: SystemColors.ActiveCaption
            /// </summary>
            public Color BackColor { get; set; } = SystemColors.ActiveCaption;
            /// <summary>
            /// Button's text configuration
            /// </summary>
            public TextOptions TextSetup { get; set; } = new TextOptions();
            /// <summary>
            /// Button's padding
            /// Default: 0
            /// </summary>
            public Padding Padding { get; set; } = new Padding(0);
            /// <summary>
            /// Which corners to be rounded for this button.<br/>
            /// Default: None
            /// </summary>
            public SWF_ARC_CORNERS RoundedCorners { get; set; } = SWF_ARC_CORNERS.None;
            /// <summary>
            /// Radius of the arc for each corner.<br/>
            /// Default: 0<br/>
            /// Suggest: 2-10
            /// </summary>
            public int RoundedRadius { get; set; } = 0;
            /// <summary>
            /// Is default ButtonsState class without any settings.
            /// </summary>
            public bool Empty { get { return this == new ButtonsState(); } }
        }
        /// <summary>
        /// Is default SWFTitlebarOptions class without any settings.
        /// </summary>
        public bool Empty { get { return this == new SWFTitlebarOptions(); } }
    }
}

using System.Drawing;
using System.Windows.Forms;

namespace SwitchWinForms
{
    public class SWFBoxingOptions : TextUtil
    {
        private SWF_BOX_TYPE _type = SWF_BOX_TYPE.Frame;
        private int _titleBarHeight = -1;
        /// <summary>
        /// To access the box it's easier with a name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Required to be setup, as different types of objects work differently based on type.
        /// </summary>
        public SWF_BOX_TYPE Type 
        { 
            get { return _type; } 
            set { 
                _type = value;
                if (_type != SWF_BOX_TYPE.FormBody)
                    _titleBarHeight = -1;
                else if (_titleBarHeight == -1)
                    _titleBarHeight = 30;   //default
            }
        }
        /// <summary>
        /// On mouse over or click, cursor changes.
        /// </summary>
        public SWFCursors Cursors { get; set; } = new SWFCursors();
        /// <summary>
        /// Parent allows zindex.
        /// </summary>
        public Form Parent { get; set; } = null;
        /// <summary>
        /// Select corners to round off.  Default None.
        /// </summary>
        public SWF_ARC_CORNERS RoundedCorners { get; set; } = SWF_ARC_CORNERS.None;
        /// <summary>
        /// Arc of the corners.  0 = Square.
        /// </summary>
        public int RoundedRadius { get; set; } = 0;
        /// <summary>
        /// Pixel size of the border for all 4 sides.
        /// </summary>
        public int BorderSize { get; set; } = 2;
        /// <summary>
        /// Padding for each size.  Example: new Padding(5,2,2,2)
        /// </summary>
        public Padding Padding { get; set; } = new Padding(0);
        /// <summary>
        /// Get or Set the Location of the box.
        /// </summary>
        public Point Location { get; set; } = new Point(0, 0);
        /// <summary>
        /// Get or Set the Boundaries of the box.
        /// </summary>
        public Size Size { get; set; } = new Size(0, 0);
        /// <summary>
        /// Get or Set the Location and Boundaries of the box.
        /// </summary>
        public Rectangle Rectangle 
        {
            get { return new Rectangle(Location, Size); }
            set
            {
                Location = value == null ? Point.Empty : value.Location;
                Size = value == null ? Size.Empty : value.Size;
            }
        }
        /// <summary>
        /// If there is a custom titlebar and if this this box type is<br/>
        /// a SWF_BOX_TYPE.FormBody, auto-sizing requires to know how tall the titlebar is.<br/>
        /// This setting will be ignored if this object type is not a SWF_BOX_TYPE.FormBody.
        /// </summary>
        public int TitleBarHeight 
        { 
            get { return _titleBarHeight; } 
            set 
            {
                _titleBarHeight = value;
            } 
        }
        /// <summary>
        /// Text location, value, font, etc..
        /// </summary>
        public TextOptions TextSetup { get; set; } = new TextOptions();
        /// <summary>
        /// Back ground color
        /// </summary>
        public Color BackColor { get; set; } = SystemColors.Control;
        /// <summary>
        /// Border color, if exists.
        /// </summary>
        public Color BorderColor { get; set; } = SystemColors.ActiveCaption;
        /// <summary>
        /// Setup shadowing color, opacity and depth
        /// </summary>
        public ShadowingSetup Shadowing { get; set; } = new ShadowingSetup();
        /// <summary>
        /// If setup as a button, if has a shadow, when clicked, should button look like it's clicked on move down and mouse up 
        /// </summary>
        public bool IsAnimatedButton { get; set; } = false;
        /// <summary>
        /// Internal tracking for mouse up and down, if animation on.
        /// </summary>
        internal bool ButtonClicked { get; set; } = false;
        /// <summary>
        /// Since everything is painted on a form, mouse over icons and clicks need to be reviewed based on zindex
        /// </summary>
        internal int ZIndex { get; set; } = 0;
    }
}

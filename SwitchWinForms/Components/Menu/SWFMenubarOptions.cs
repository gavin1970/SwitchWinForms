using System.Drawing;
using System;
using System.Windows.Forms;

namespace SwitchWinForms.Models
{
    public class SWFMenubarOptions
    {
        public SWFMenubarOptions() { }
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
        /// titlebar is always location 0,0
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
        ///     Valid alues (0-5).
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
        /// Is default SWFTitlebarOptions class without any settings.
        /// </summary>
        public bool Empty { get { return this == new SWFMenubarOptions(); } }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
//test
namespace SwitchWinForms
{
    internal class BoxInfo
    {
        public int BorderSize { get; set; }
        public Padding Padding { get; set; }
        public Rectangle Rectangle { get; set; }
        public SWF_BOX_TYPE Type { get; set; }
        public int TextCursor { get; set; }
        public string Text { get; set; }
        public Image Icon { get; set; }
        public Font Font { get; set; }
        public StringAlignment VAlign { get; set; }
        public StringAlignment HAlign { get; set; }
        public Color BackColor { get; set; }
        public Color BorderColor { get; set; }
        public Color ForeColor { get; set; }
        public Image BackgroundImage { get; set; } = null;
        public List<ReplaceColor> ReplaceImageColor { get; set; } = new List<ReplaceColor>() { };
        public ShadowingSetup Shadowing { get; set; } = new ShadowingSetup();
        public SWF_BG_IMG_LAYOUT BackgroundImageLayout { get; set; } = SWF_BG_IMG_LAYOUT.Stretch;
        public SWF_ARC_CORNERS RoundedCorners { get; set; } = SWF_ARC_CORNERS.None;
        public int RoundedRadius { get; set; } = 0;
        public bool Empty { get { return (this == new BoxInfo()); } }

        internal bool ButtonClicked { get; set; } = false;
        internal bool InputStart { get; set; } = false;
        internal Image BackgroundImageOrg { get; set; } = null;
        internal bool BackgroundImageModified { get; set; } = false;
        /// <summary>
        /// Deprecated Method.
        /// </summary>
        /// <returns></returns>
        [Obsolete("IsTextEmpty() is deprecated, please use 'Empty' property instead.")]
        public bool IsTextEmpty()
        {
            return Empty;
        }
    }
}

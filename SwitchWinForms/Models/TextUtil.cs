using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SwitchWinForms
{
    public abstract class TextUtil
    {
        public class TextOptions
        {
            internal int TextCursor { get; set; } = 0;
            public string Text { get; set; } = string.Empty;
            /// <summary>
            /// Only used for SWF_BOX_TYPE.TextBox
            /// </summary>
            public string ValidInputExpression { get; set; } = null;
            public Image BackgroundImage { get; set; } = null;
            public SWF_BG_IMG_LAYOUT BackgroundImageLayout { get; set; } = SWF_BG_IMG_LAYOUT.Stretch;
            public List<ReplaceColor> ReplaceImageColor { get; set; } = new List<ReplaceColor>() { };
            public Font Font { get; set; } = Global.DefaultFont;
            public Color Color { get; set; } = Global.DefaultFontColor;
            public StringAlignment VerticalAlign { get; set; } = StringAlignment.Near;
            public StringAlignment HorizonalAlign { get; set; } = StringAlignment.Near;
            public bool Empty { get { return this == new TextOptions(); } }
            /// <summary>
            /// Deprecated Method
            /// </summary>
            /// <returns></returns>
            [Obsolete("IsEmpty() is deprecated, please use 'Empty' property instead.")]
            public bool IsEmpty() {
                return Empty;
            }
        }
    }
}
using System;
using System.Drawing;

namespace SwitchWinForms
{
    public class ReplaceColor
    {
        private int _defaultImageColorTolerance = 10;
        public ReplaceColor() { }
        public ReplaceColor(Color delete, Color add, int clrTol = 100)
        {
            Delete = delete;
            Add = add;
            ImageColorTolerance = clrTol;
        }
        public Color Delete { get; set; } = Color.Empty;
        public Color Add { get; set; } = Color.Empty;
        public int ImageColorTolerance
        {
            get => this._defaultImageColorTolerance;
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("Invalid image color tolerance.\nValid range: 0 - 100");

                this._defaultImageColorTolerance = value;
            }
        }
        public bool Empty { get { return this == new ReplaceColor(); } }
    }
}

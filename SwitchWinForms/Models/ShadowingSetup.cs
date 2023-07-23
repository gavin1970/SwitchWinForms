using System.Drawing;

namespace SwitchWinForms
{
    public class ShadowingSetup
    {
        public Color Color { get; set; } = Color.Black;
        public SWF_TRANSPARENT_PERCENTAGE Visibilty { get; set; } = SWF_TRANSPARENT_PERCENTAGE.Empty;
        public int Depth { get; set; } = 5;
        internal int getVisibilty { get { return (int)Visibilty; } }
        public bool Empty { get { return this == new ShadowingSetup(); } }
    }
}

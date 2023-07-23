namespace SwitchWinForms
{
    public enum SWF_BG_IMG_LAYOUT
    {
        Normal,
        Center,
        Stretch,
        Tile
    }
    public enum SWF_BOX_TYPE
    {
        Empty,
        Titlebar,
        FormBody,
        Button,
        Frame,
        Label,
        TextBox
    }
    public enum SWF_ARC_CORNERS
    {
        None = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 4,
        BottomRight = 8,
        Left = TopLeft | BottomLeft,
        Right = TopRight | BottomRight,
        Top = TopLeft | TopRight,
        Bottom = BottomLeft | BottomRight,
        All = TopLeft | TopRight | BottomLeft | BottomRight,
    }
    public enum SWF_TRANSPARENT_PERCENTAGE
    {
        Empty = 0,
        Five = 13,
        Ten = 26,
        Fifteen = 38,
        Twenty = 51,
        TwentyFive = 64,
        Thirty = 77,
        ThirtyFive = 89,
        Forty = 102,
        FortyFive = 115,
        Fifty = 127,
        FiftyFive = 140,
        Sixty = 153,
        SixtyFive = 166,
        Seventy = 179,
        SeventyFive = 191,
        Eighty = 204,
        EightyFive = 217,
        Ninty = 230,
        NintyFive = 242,
        Hundered = 255
    }
    public enum SWF_TITLE_BUTTON
    {
        Minimize = 1,
        Maximize_Normal = 2,
        Close = 4,
        All = Minimize | Maximize_Normal | Close
    }
    internal enum SWF_RESIZEABLE
    {
        None = 0,
        Yes = 1,
        No = 2
    }
    internal enum SWF_HITTEST
    {
        HT_ERROR = -2,
        HT_TRANSPARENT = -1,
        HT_NOWHERE = 0,
        HT_CLIENT = 1,
        HT_CAPTION = 2,
        HT_SYSMENU = 3,
        HT_GROWBOX = 4,
        HT_MENU = 5,
        HT_HSCROLL = 6,
        HT_VSCROLL = 7,
        HT_MINBUTTON = 8,
        HT_MAXBUTTON = 9,
        HT_LEFT = 10,
        HT_RIGHT = 11,
        HT_TOP = 12,
        HT_TOPLEFT = 13,
        HT_TOPRIGHT = 14,
        HT_BOTTOM = 15,
        HT_BOTTOMLEFT = 16,
        HT_BOTTOMRIGHT = 17,
        HT_BORDER = 18,
        HT_OBJECT = 19,
        HT_CLOSE = 20,
        HT_HELP = 21
    }
}
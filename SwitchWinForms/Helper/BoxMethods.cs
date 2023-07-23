using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace SwitchWinForms
{
    public abstract class BoxMethods
    {
        private static SWFTitlebarOptions _titleOptions = null;

        const int cursorFormEdge = 5;
        const int WM_NCHITTEST = 0x0084;
        
        private SWF_RESIZEABLE _resizeable = SWF_RESIZEABLE.None;


        #region Internal Properties
        internal static Dictionary<string, SWFBoxingOptions> ZIndex { get; } = new Dictionary<string, SWFBoxingOptions>();
        internal static int LatestZIndex { get; set; } = 0;
        internal Rectangle BtnCloseRec { get; set; } = new Rectangle();
        internal Rectangle BtnMinRec { get; set; } = new Rectangle();
        internal Rectangle BtnMaxRec { get; set; } = new Rectangle();
        internal Point StartPoint { get; set; } = new Point(0, 0);                     // also for the moving
        internal bool Drag { get; set; } = false;                                      // determine if we should be moving the form
        internal Exception SetLastError { get; set; }
        #endregion

        #region Public Methods
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool WndProc(ref Message m)
        {
            bool retVal = false;

            if (m.Msg == WM_NCHITTEST && IsFormResizeable())
            {
                int x = (int)(m.LParam.ToInt64() & 0xFFFF);
                int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
                Point pt = SWFTitlebar.TitleOptions.Parent.PointToClient(new Point(x, y));
                Size clientSize = SWFTitlebar.TitleOptions.Parent.ClientSize;

                if (Global.IsInBounds(SWFTitlebar.TitleOptions.Parent, pt, this.BtnCloseRec, Cursors.Hand) ||
                    Global.IsInBounds(SWFTitlebar.TitleOptions.Parent, pt, this.BtnMinRec, Cursors.Hand) ||
                    Global.IsInBounds(SWFTitlebar.TitleOptions.Parent, pt, this.BtnMaxRec, Cursors.Hand))
                {
                    retVal = false;
                }
                else if (pt.X <= cursorFormEdge && pt.Y >= clientSize.Height - cursorFormEdge && clientSize.Height >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWFTitlebar.TitleOptions.Parent.IsMirrored ? SWF_HITTEST.HT_BOTTOMRIGHT : SWF_HITTEST.HT_BOTTOMLEFT);
                    retVal = true;
                }
                else if (pt.X >= clientSize.Width - cursorFormEdge && pt.Y >= clientSize.Height - cursorFormEdge && clientSize.Width >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWFTitlebar.TitleOptions.Parent.IsMirrored ? SWF_HITTEST.HT_BOTTOMLEFT : SWF_HITTEST.HT_BOTTOMRIGHT);
                    retVal = true;
                }
                else if (pt.X <= cursorFormEdge && pt.Y <= cursorFormEdge && clientSize.Height >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWFTitlebar.TitleOptions.Parent.IsMirrored ? SWF_HITTEST.HT_TOPRIGHT : SWF_HITTEST.HT_TOPLEFT);
                    retVal = true;
                }
                else if (pt.X >= clientSize.Width - cursorFormEdge && pt.Y <= cursorFormEdge && clientSize.Height >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWFTitlebar.TitleOptions.Parent.IsMirrored ? SWF_HITTEST.HT_TOPLEFT : SWF_HITTEST.HT_TOPRIGHT);
                    retVal = true;
                }
                else if (pt.Y <= cursorFormEdge && clientSize.Height >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWF_HITTEST.HT_TOP);
                    retVal = true;
                }
                else if (pt.Y >= clientSize.Height - cursorFormEdge && clientSize.Height >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWF_HITTEST.HT_BOTTOM);
                    retVal = true;
                }
                else if (pt.X <= cursorFormEdge && clientSize.Height >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWF_HITTEST.HT_LEFT);
                    retVal = true;
                }
                else if (pt.X >= clientSize.Width - cursorFormEdge && clientSize.Height >= cursorFormEdge)
                {
                    m.Result = (IntPtr)(SWF_HITTEST.HT_RIGHT);
                    retVal = true;
                }
                else
                {
                    //SWFTitlebar.TitleOptions.Parent.Cursor = Cursors.Default;
                    retVal = false;
                }
            }

            return retVal;
        }
        public static SWFTitlebarOptions TitleOptions
        {
            get { return _titleOptions; }
            set
            {
                _titleOptions = value;
                if (TitleOptions.Parent != null && !string.IsNullOrWhiteSpace(TitleOptions.TextSetup?.Text))
                    TitleOptions.Parent.Text = TitleOptions.TextSetup.Text;
            }
        }
        public Exception LastError { get { return SetLastError; } }
        #endregion

        #region Private/Internal Methods
        internal bool GetZIndex(ref SWFBoxingOptions options, out int idx)
        {
            bool retVal = false;

            lock (ZIndex)
            {
                if (ZIndex.TryGetValue(options.Name, out SWFBoxingOptions foundOpt))
                {
                    idx = foundOpt.ZIndex;
                    retVal = true;
                }
                else
                {
                    idx = ++LatestZIndex;
                    options.ZIndex = idx;
                    ZIndex.Add(options.Name, options);
                }
            }

            return retVal;
        }
        internal bool ImOnTop(SWFBoxingOptions options)
        {
            bool retVal = true;

            foreach (SWFBoxingOptions bopt in ZIndex.Values)
            {
                if (options.Name == bopt.Name)
                    continue;

                if (bopt.ZIndex > options.ZIndex)
                {
                    if (options.Rectangle.Contains(bopt.Rectangle))
                    {
                        retVal = false;
                        break;
                    }
                }
            }

            return retVal;
        }
        private bool IsFormResizeable()
        {
            if (this._resizeable == SWF_RESIZEABLE.None)
            {
                if ((TitleOptions.ButtonsEnabled & SWF_TITLE_BUTTON.Maximize_Normal) > 0)
                    this._resizeable = SWF_RESIZEABLE.Yes;
                else
                    this._resizeable = SWF_RESIZEABLE.No;
            }

            return this._resizeable == SWF_RESIZEABLE.Yes;
        }
        internal Cursor GetCursorByType(Form frm, Cursor defaultCursor, out bool cursorChanged)
        {
            Cursor retVal = frm.Cursor;
            cursorChanged = false;

            if (retVal != defaultCursor)
            {
                cursorChanged = true;
                retVal = defaultCursor;
            }
            return retVal;
        }
        #endregion
    }
}

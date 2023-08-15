using SwitchWinForms.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SwitchWinForms.Handler
{
    public partial class SWFMenubar : BoxMethods
    {
        #region Constructor
        public SWFMenubar(Form frm, SWFMenubarOptions options = null)
        {
            if (frm == null)
            {
                SetLastError = new ArgumentNullException("Menubar is missing Form object");
                throw SetLastError;
            }

            MenuOptions = options ?? new SWFMenubarOptions();
            MenuOptions.GetSetParent = frm;

            //SetupOptions();
        }
        #endregion
    }
}

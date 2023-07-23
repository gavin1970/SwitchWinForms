using System.Windows.Forms;

/*                CursorConverter cConverter = new CursorConverter();
                Cursor hand = (Cursor)cConverter.ConvertFromString("Hand");
                string hhh = this.BoxingOptions.Parent.Cursor.cu.ToString();
                string iii =Cursor.Current.ToString();
*/
namespace SwitchWinForms
{
    public class SWFCursors
    {
        internal Cursor OnMove { get; set; } = Cursors.Default;
        internal Cursor OnClick { get; set; } = Cursors.Default;
        public SWFCursors(Cursor onMove = null, Cursor onClick = null) 
        {
            if (onMove != null)
                this.OnMove = onMove;

            if (onClick != null)
                this.OnClick = onClick;
        }
    }
}

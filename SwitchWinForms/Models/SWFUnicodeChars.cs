using System.Globalization;

namespace SwitchWinForms
{
    /// <summary>
    /// Unicode for special characters.
    /// </summary>
    public class SWFUnicodeChars
    {
        private const int min = 10134;
        private const int max = 11026;
        private const int norm = 11027;
        private const int close = 10062;        //10060-X;
        private const int txtCursor = 10626;    //10070-❖   10626-⦂
        private int ucode = 0;
        /// <summary>
        /// You can pass any Unicode number and use 
        /// .ToString() to get the local string value.
        /// </summary>
        /// <param name="ucode"></param>
        public SWFUnicodeChars(int ucode) =>
            this.ucode = ucode;
        /// <summary>
        /// Internal method to convert Unicode to current culture string value.
        /// </summary>
        /// <param name="Unicode"></param>
        /// <returns></returns>
        private static string MakeString(int Unicode)
        {
            return ((char)Unicode)
                .ToString(new CultureInfo(CultureInfo.CurrentCulture.Name));
        }
        /// <summary>
        /// Returns the default value for Minimum button text.
        /// </summary>
        public static string MIN 
        {
            get { return MakeString(min); }
        }
        /// <summary>
        /// Returns the default value for Text Cursor text.
        /// </summary>
        public static string TXT_CURSOR
        {
            get { return MakeString(txtCursor); }
        }
        /// <summary>
        /// Returns the default value for Maximum button text.
        /// </summary>
        public static string MAX
        {
            get { return MakeString(max); }
        }
        /// <summary>
        /// Returns the default value for Normal button text.
        /// </summary>
        public static string NORM
        {
            get { return MakeString(norm); }
        }
        /// <summary>
        /// Returns the default value for Close button text.
        /// </summary>
        public static string CLOSE
        {
            get { return MakeString(close); }
        }
        /// <summary>
        /// Converts the Unicode value passed in to the current culture string value.
        /// </summary>
        public override string ToString()
        {
            return MakeString(this.ucode);
        }
        /// <summary>
        /// Has class been called.
        /// </summary>
        public bool Empty { get { return this.ucode == 0; } }
    }
}

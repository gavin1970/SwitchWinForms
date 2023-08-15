using System;

namespace SwitchWinForms
{
    public class SWFMessageEventArgs : EventArgs
    {
        private Exception _exception = null;
        private string _message = string.Empty;

        public SWFMessageEventArgs(Exception exception)
        {
            this._exception = exception;
            this._message = exception.Message;
        }
        public SWFMessageEventArgs(string message) =>
            this._message = message;
        public Exception Exception { get { return this._exception; } }
        public string Message { get { return this._message; } }
        public bool Success { get { return this._exception == null; } }
    }
}

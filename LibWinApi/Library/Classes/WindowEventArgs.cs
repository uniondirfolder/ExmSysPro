using System;

namespace LibWinApi.Library.Classes
{
    public class WindowEventArgs
    {
        public IntPtr Handle { get; }
        public WindowEventArgs(IntPtr handle)
        {
            Handle = handle;
        }
    }
}
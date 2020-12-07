using System;
using LibWinApi.Library.Import;

namespace LibWinApi.Library.Classes
{
    public class WindowInfoObject
    {
        public IntPtr HWnd;
        public int EventType;

        public string AppPath { get; set; }
        public string AppName { get; set; }
        public string AppTitle { get; set; }

       
    }
}
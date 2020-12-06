using System;

namespace LibWinApi.Library.Classes
{
    public class WindowInfoObject
    {
        public IntPtr HWnd;
        public int EventType;
        private string _appPath;

        public string AppPath
        {
            get => _appPath;
            set => _appPath = value;
        }
        public string AppName { get; set; }
        public string AppTitle { get; set; }
    }
}
using System;
using LibWinApi.Library.Enums;

namespace LibWinApi.Library.Classes
{
    public class ClipboardHookEventArgs:EventArgs
    {
        public object Data { get; set; }
        public EnumClipboardContentTypes DataFormat { get; set; }
    }
}
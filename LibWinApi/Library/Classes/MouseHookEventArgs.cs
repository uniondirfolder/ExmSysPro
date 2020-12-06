
using System;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Structs;

namespace LibWinApi.Library.Classes
{
    public class MouseHookEventArgs:EventArgs
    {
        internal EnumWinMsgs MouseMessage { get; set; }
        internal StructPoint Point { get; set; }
        public uint MouseInfo { get; set; }
    }
}
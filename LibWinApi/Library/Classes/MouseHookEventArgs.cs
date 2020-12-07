
using System;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Structs;

namespace LibWinApi.Library.Classes
{
    public class MouseHookEventArgs:EventArgs
    {
        public EnumWinMsgs MouseMessage { get; set; }
        public StructPoint Point { get; set; }
        public uint MouseInfo { get; set; }
    }
}
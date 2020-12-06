using System;
using System.Runtime.InteropServices;

namespace LibWinApi.Library.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct StructMSLLHOOKSRTUCT
    {
        internal StructPoint pt;
        internal readonly uint mouseData;
        internal readonly uint flags;
        internal readonly uint time;
        internal readonly IntPtr dwExtraInfo;
    }
}
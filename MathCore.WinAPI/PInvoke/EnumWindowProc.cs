using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MathCore.WinAPI.PInvoke
{
    [Serializable]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);
}

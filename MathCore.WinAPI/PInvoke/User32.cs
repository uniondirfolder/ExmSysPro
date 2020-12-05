using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MathCore.WinAPI.PInvoke
{
    public static class User32
    {
        public const string FileName = "user32.dll";
        #region SendMessage
        [DllImport(FileName, CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
        #endregion
        #region PostMessage
        [DllImport(FileName, CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
        #endregion
        [DllImport(FileName, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport(FileName, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, uint nMaxCount);

        [DllImport(FileName, SetLastError = true)]
        public static extern int EnumWindows(EnumWindowProc hWnd, IntPtr lParam);

        [DllImport(FileName, SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport(FileName, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport(FileName, SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    }
}

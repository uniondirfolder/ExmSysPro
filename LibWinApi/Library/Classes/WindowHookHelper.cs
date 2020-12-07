using System;
using System.Diagnostics;
using System.Text;
using LibWinApi.Library.Import;

namespace LibWinApi.Library.Classes
{
    internal class WindowHookHelper
    {
        internal static IntPtr GetActiveWindowHandle()
        {
            try
            {
                return (IntPtr)DllUser32.GetForegroundWindow();
            }
            catch (Exception)
            {
                // ~
            }
            return IntPtr.Zero;
        }
        internal static string GetAppPath(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                return "empty handle";
            }

            try
            {
                uint pid;
                DllUser32.GetWindowThreadProcessId(hWnd, out pid);
                var proc = Process.GetProcessById((int)pid);
                return proc.MainModule.FileName;
                
            }
            catch(Exception ex)
            {
                return "empty handle";
            }
        }
        internal static string GetWindowText(IntPtr hWnd)
        {
            try
            {
                int length = DllUser32.GetWindowTextLength(hWnd);
                var sb = new StringBuilder(length + 1);
                DllUser32.GetWindowText(hWnd, sb, sb.Capacity);
                return sb.ToString();
            }
            catch (Exception)
            {
                return "err-get-TxtWin";
            }
        }
        internal static string GetAppDescription(string appPath)
        {
            if (appPath == null)
            {
                return null;
            }
            try
            {
                return FileVersionInfo.GetVersionInfo(appPath).FileDescription;
            }
            catch
            {
                return null;
            }
        }
    }
}
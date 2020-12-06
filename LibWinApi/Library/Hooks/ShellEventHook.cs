using System;
using System.Windows.Forms;
using LibWinApi.Library.Delegates;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Import;

namespace LibWinApi.Library.Hooks
{
    internal sealed class ShellEventHook:NativeWindow
    {
        private readonly uint _wmShellHook;
        internal event GeneralShellHookEventHandler WindowCreated;
        internal event GeneralShellHookEventHandler WindowDestroyed;
        internal event GeneralShellHookEventHandler WindowActivated;
        internal ShellEventHook(IntPtr hWnd)
        {
            var cp = new CreateParams();
            CreateHandle(cp);
            DllUser32.SetTaskmanWindow(hWnd);
            if (DllUser32.RegisterShellHookWindow(Handle))
            {
                _wmShellHook = DllUser32.RegisterWindowMessage("SHELLHOOK");
            }
        }
        internal void DeRegister()
        {
            DllUser32.RegisterShellHook(Handle, 0);
        }
        internal void EnumWindows()
        {
            DllUser32.EnumWindows(EnumWindowsProc, IntPtr.Zero);
        }
        private bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            if (IsAppWindow(hWnd))
            {
                OnWindowCreated(hWnd);
            }

            return true;
        }
        private static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return DllUser32.GetWindowLong(hWnd, nIndex);
            }

            return DllUser32.GetWindowLongPtr(hWnd, nIndex);
        }
        private static bool IsAppWindow(IntPtr hWnd)
        {
            if ((GetWindowLong(hWnd, (int)EnumGWLIndex.GWL_STYLE) & (int)EnumWindowsStyle.WS_SYSMENU) == 0)
            {
                return false;
            }

            if (DllUser32.IsWindowVisible(hWnd))
            {
                if ((GetWindowLong(hWnd, (int)EnumGWLIndex.GWL_EXSTYLE) & (int)EnumWindowsStyleEx.WS_EX_TOOLWINDOW) != 0)
                {
                    return false;
                }

                var hwndOwner = DllUser32.GetWindow(hWnd, (int)EnumGetWindowConstants.GW_OWNER);
                return (GetWindowLong(hwndOwner, (int)EnumGWLIndex.GWL_STYLE) &
                        ((int)EnumWindowsStyle.WS_VISIBLE | (int)EnumWindowsStyle.WS_CLIPCHILDREN)) !=
                       ((int)EnumWindowsStyle.WS_VISIBLE | (int)EnumWindowsStyle.WS_CLIPCHILDREN) ||
                       (GetWindowLong(hwndOwner, (int)EnumGWLIndex.GWL_EXSTYLE) & (int)EnumWindowsStyleEx.WS_EX_TOOLWINDOW) != 0;
            }

            return false;
        }

        private void OnWindowCreated(IntPtr hWnd)
        {
            if (WindowCreated != null)
            {
                WindowCreated(this, hWnd);
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == _wmShellHook)
            {
                switch ((EnumShellEvents)m.WParam)
                {
                    case EnumShellEvents.HSHELL_WINDOWCREATED:
                        if (IsAppWindow(m.LParam))
                        {
                            OnWindowCreated(m.LParam);
                        }

                        break;
                    case EnumShellEvents.HSHELL_WINDOWDESTROYED:
                        WindowDestroyed?.Invoke(this, m.LParam);
                        break;

                    case EnumShellEvents.HSHELL_WINDOWACTIVATED:
                        WindowActivated?.Invoke(this, m.LParam);
                        break;
                }
            }
            base.WndProc(ref m);
        }
    }
}
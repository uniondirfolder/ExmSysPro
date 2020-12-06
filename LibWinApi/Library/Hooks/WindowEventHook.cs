using System;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Delegates;

namespace LibWinApi.Library.Hooks
{
    internal class WindowEventHook
    {
        private static ShellEventHook _seh;
        internal event GeneralShellHookEventHandler WindowCreated;
        internal event GeneralShellHookEventHandler WindowDestroyed;
        internal event GeneralShellHookEventHandler WindowActivated;
        internal WindowEventHook(SyncHookFactory syncHookFactory)
        {
            if (_seh == null)
            {
                _seh= new ShellEventHook(syncHookFactory.GetHandle());
                _seh.WindowCreated += WindowCreatedEvent;
                _seh.WindowDestroyed += WindowDestroyedEvent;
                _seh.WindowActivated += WindowActivatedEvent;
            }
        }
        private void WindowCreatedEvent(ShellEventHook shellObject, IntPtr hWnd)
        {
            WindowCreated?.Invoke(shellObject, hWnd);
        }
        private void WindowDestroyedEvent(ShellEventHook shellObject, IntPtr hWnd)
        {
            WindowDestroyed?.Invoke(shellObject, hWnd);
        }
        private void WindowActivatedEvent(ShellEventHook shellObject, IntPtr hWnd)
        {
            WindowActivated?.Invoke(shellObject, hWnd);
        }
        internal void Destroy()
        {
            _seh = null;
        }
    }
}
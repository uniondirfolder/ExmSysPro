using System;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Hooks;

namespace LibWinApi.Library.Delegates
{
    internal delegate IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam);

    internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    internal delegate IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam);

    internal delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    internal delegate void RawKeyEventHandler(object sender, RawKeyEventArgs args);

    internal delegate void KeyboardCallbackAsync(EnumKeyEvent keyEvent, int vkCode, string character);

    internal delegate void GeneralShellHookEventHandler(ShellEventHook sender, IntPtr hWnd);

    internal delegate void WinEventProc(IntPtr hookHandle, EnumWindowEvent @event, IntPtr hWnd, int @object, int child, int threadId, int timestampMs);
}

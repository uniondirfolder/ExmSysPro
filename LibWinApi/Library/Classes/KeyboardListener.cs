using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using LibWinApi.Library.Delegates;
using LibWinApi.Library.Import;

namespace LibWinApi.Library.Classes
{
    internal class KeyboardListener:IDisposable
    {
        private readonly Dispatcher _dispatherThreading;

        private readonly LowLevelKeyboardProc _hookProcDelegateToAvoidGC;
 
        private readonly IntPtr _hookId;

        private readonly KeyboardCallbackAsync _hookedKeyboardCallbackAsync;

        internal event RawKeyEventHandler KeyDown;

        internal event RawKeyEventHandler KeyUp;

        internal KeyboardListener()
        {
            _dispatherThreading = Dispatcher.CurrentDispatcher;
            _hookProcDelegateToAvoidGC = LowLevelKeyboardProc;
            _hookId = InterceptKeys.SetHook(_hookProcDelegateToAvoidGC);
            _hookedKeyboardCallbackAsync = KeyboardListener_KeyboardCallbackAsync;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam.ToUInt32() == (int)Enums.EnumKeyEvent.WM_KEYDOWN ||
                    wParam.ToUInt32() == (int)Enums.EnumKeyEvent.WM_KEYUP ||
                    wParam.ToUInt32() == (int)Enums.EnumKeyEvent.WM_SYSKEYDOWN ||
                    wParam.ToUInt32() == (int)Enums.EnumKeyEvent.WM_SYSKEYUP)
                {
                    string chars = InterceptKeys.VkCodeToString((uint)Marshal.ReadInt32(lParam),
                        wParam.ToUInt32() == (int)Enums.EnumKeyEvent.WM_KEYDOWN ||
                        wParam.ToUInt32() == (int)Enums.EnumKeyEvent.WM_SYSKEYDOWN);

                    _hookedKeyboardCallbackAsync.BeginInvoke((Enums.EnumKeyEvent)wParam.ToUInt32(),
                        Marshal.ReadInt32(lParam), chars, null, null);
                }
            }

            return DllUser32.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        private void KeyboardListener_KeyboardCallbackAsync(Enums.EnumKeyEvent keyEvent, int vkCode,
            string character)
        {
            switch (keyEvent)
            {
             
                case Enums.EnumKeyEvent.WM_KEYDOWN:
                    if (KeyDown != null)
                    {
                        _dispatherThreading.BeginInvoke(new RawKeyEventHandler(KeyDown), this,
                            new RawKeyEventArgs(vkCode, false, character, 0));
                    }

                    break;
                case Enums.EnumKeyEvent.WM_SYSKEYDOWN:
                    if (KeyDown != null)
                    {
                        _dispatherThreading.BeginInvoke(new RawKeyEventHandler(KeyDown), this,
                            new RawKeyEventArgs(vkCode, true, character, 0));
                    }

                    break;


                case Enums.EnumKeyEvent.WM_KEYUP:
                    if (KeyUp != null)
                    {
                        _dispatherThreading.BeginInvoke(new RawKeyEventHandler(KeyUp), this,
                            new RawKeyEventArgs(vkCode, false, character, 1));
                    }

                    break;
                case Enums.EnumKeyEvent.WM_SYSKEYUP:
                    if (KeyUp != null)
                    {
                        _dispatherThreading.BeginInvoke(new RawKeyEventHandler(KeyUp), this,
                            new RawKeyEventArgs(vkCode, true, character, 1));
                    }

                    break;
            }
        }

        public void Dispose()
        {
            DllUser32.UnhookWindowsHookEx(_hookId);
        }
        ~KeyboardListener()
        {
            Dispose();
        }
    }
}
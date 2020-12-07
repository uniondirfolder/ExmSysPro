using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Delegates;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Import;
using LibWinApi.Library.Structs;

namespace LibWinApi.Library.Hooks
{
    internal class MouseEventHook
    {
        private const int WH_MOUSE_LL = 14;
        private static IntPtr _hookId = IntPtr.Zero;
        private readonly LowLevelMouseProc _mouseProc;
        internal event EventHandler<RawMouseEventArgs> MouseAction = delegate { };

        public MouseEventHook()
        {
            _mouseProc = HookCallBack;
        }
        private static IntPtr SetHook(LowLevelMouseProc mouseProc)
        {
            var hook = DllUser32.SetWindowsHookEx(WH_MOUSE_LL, mouseProc, DllUser32.GetModuleHandle("user32"), 0);

            if (hook == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return hook;
        }
        private  IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            StructMSLLHOOKSRTUCT hookStruct;
            if (nCode < 0)
            {
                return DllUser32.CallNextHookEx(_hookId, nCode, wParam, lParam);
            }

            hookStruct = (StructMSLLHOOKSRTUCT)Marshal.PtrToStructure(lParam, typeof(StructMSLLHOOKSRTUCT));

            MouseAction(null, new RawMouseEventArgs { MouseMessage = (EnumWinMsgs)wParam, Point = hookStruct.pt, MouseInfo = hookStruct.mouseData });

            return DllUser32.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
        internal void Start()
        {
            _hookId = SetHook(_mouseProc);
        }

        internal void Stop()
        {
           DllUser32.UnhookWindowsHookEx(_hookId);
        }
    }
}
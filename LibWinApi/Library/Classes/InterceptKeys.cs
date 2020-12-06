using System;
using System.Diagnostics;
using System.Text;
using LibWinApi.Library.Delegates;
using LibWinApi.Library.Import;
//можливий баг 49 лінія
namespace LibWinApi.Library.Classes
{
    internal class InterceptKeys
    {
        internal static int WH_KEYBOARD_LL = 13;

        private static uint _lastVkCode;
        private static uint _lastScanCode;
        private static byte[] _lastKeyState = new byte[255];
        private static bool _lastIsDead;

        internal static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return DllUser32.SetWindowsHookEx(WH_KEYBOARD_LL, proc, DllUser32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        #region Convert VKCode to string

        // Note: Sometimes single VKCode represents multiple chars, thus string. 
        // E.g. typing "^1" (notice that when pressing 1 the both characters appear, 
        // because of this behavior, "^" is called dead key)

        /// <summary>
        ///     Convert VKCode to Unicode.
        ///     <remarks>isKeyDown is required for because of keyboard state inconsistencies!</remarks>
        /// </summary>
        /// <param name="vkCode">VKCode</param>
        /// <param name="isKeyDown">Is the key down event?</param>
        /// <returns>String representing single unicode character.</returns>
        internal static string VkCodeToString(uint vkCode, bool isKeyDown)
        {
            // ToUnicodeEx needs StringBuilder, it populates that during execution.
            var sbString = new StringBuilder(5);

            var bKeyState = new byte[255];
            bool bKeyStateStatus;
            bool isDead = false;

            // Gets the current windows window handle, threadID, processID
            IntPtr currentHWnd = (IntPtr)DllUser32.GetForegroundWindow();//may be bag
            uint currentWindowThreadId = DllUser32.GetWindowThreadProcessId(currentHWnd, out _);

            // This programs Thread ID
            uint thisProgramThreadId = DllUser32.GetCurrentThreadId();

            // Attach to active thread so we can get that keyboard state
            if (DllUser32.AttachThreadInput(thisProgramThreadId, currentWindowThreadId, true))
            {
                // Current state of the modifiers in keyboard
                bKeyStateStatus = DllUser32.GetKeyboardState(bKeyState);

                // Detach
                DllUser32.AttachThreadInput(thisProgramThreadId, currentWindowThreadId, false);
            }
            else
            {
                // Could not attach, perhaps it is this process?
                bKeyStateStatus = DllUser32.GetKeyboardState(bKeyState);
            }

            // On failure we return empty string.
            if (!bKeyStateStatus)
            {
                return "";
            }

            // Gets the layout of keyboard
            var hkl = DllUser32.GetKeyboardLayout(currentWindowThreadId);

            // Maps the virtual keycode
            uint lScanCode = DllUser32.MapVirtualKeyEx(vkCode, 0, hkl);

            // Keyboard state goes inconsistent if this is not in place. In other words, we need to call above commands in UP events also.
            if (!isKeyDown)
            {
                return "";
            }

            // Converts the VKCode to unicode
            int relevantKeyCountInBuffer =
                DllUser32.ToUnicodeEx(vkCode, lScanCode, bKeyState, sbString, sbString.Capacity, 0, hkl);

            string ret = string.Empty;

            switch (relevantKeyCountInBuffer)
            {
                // Dead keys (^,`...)
                case -1:
                    isDead = true;

                    // We must clear the buffer because ToUnicodeEx messed it up, see below.
                    ClearKeyboardBuffer(vkCode, lScanCode, hkl);
                    break;

                case 0:
                    break;

                // Single character in buffer
                case 1:
                    ret = sbString[0].ToString();
                    break;

                // Two or more (only two of them is relevant)
                default:
                    ret = sbString.ToString().Substring(0, 2);
                    break;
            }

            // We inject the last dead key back, since ToUnicodeEx removed it.
            // More about this peculiar behavior see e.g: 
            //   http://www.experts-exchange.com/Programming/System/Windows__Programming/Q_23453780.html
            //   http://blogs.msdn.com/michkap/archive/2005/01/19/355870.aspx
            //   http://blogs.msdn.com/michkap/archive/2007/10/27/5717859.aspx
            if (_lastVkCode != 0 && _lastIsDead)
            {
                var sbTemp = new StringBuilder(5);
                DllUser32.ToUnicodeEx(_lastVkCode, _lastScanCode, _lastKeyState, sbTemp, sbTemp.Capacity, 0, hkl);
                _lastVkCode = 0;

                return ret;
            }

            // Save these
            _lastScanCode = lScanCode;
            _lastVkCode = vkCode;
            _lastIsDead = isDead;
            _lastKeyState = (byte[])bKeyState.Clone();

            return ret;
        }

        private static void ClearKeyboardBuffer(uint vk, uint sc, IntPtr hkl)
        {
            var sb = new StringBuilder(10);

            int rc;
            do
            {
                var lpKeyStateNull = new byte[255];
                rc = DllUser32.ToUnicodeEx(vk, sc, lpKeyStateNull, sb, sb.Capacity, 0, hkl);
            } while (rc < 0);
        }

        #endregion

    }
}
using System;
using System.Runtime.InteropServices;
using System.Text;
using LibWinApi.Library.Delegates;

namespace LibWinApi.Library.Import
{
    public class DllUser32
    {
        internal class User32
        {
            [DllImport("user32.dll")]
            internal static extern int GetForegroundWindow();

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lPdwProcessId);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr SetClipboardViewer(IntPtr hWnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool ChangeClipboardChain(
                IntPtr hWndRemove, // handle to window to remove
                IntPtr hWndNewNext // handle to next window
            );

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool EnumWindows(EnumWindowsProc numFunc, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr GetParent(IntPtr hWnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr GetWindow(IntPtr hwnd, int uCmd);

            [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern int GetWindowLongPtr(IntPtr hWnd, int nIndex);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool IsWindowVisible(IntPtr hwnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool RegisterShellHook(IntPtr hWnd, int flags);

            /// <summary>
            ///Реєструє вказане вікно оболонки для отримання певних повідомлень про події або сповіщення, які є корисними
            ///Програми оболонки. Отримані повідомлення про події - це лише ті, що надсилаються у вікно оболонки, пов'язане з
            ///вказаний робочий стіл вікна. Багато повідомлень збігаються з повідомленнями, які можна отримати після дзвінка
            ///функція SetWindowsHookEx та вказівка ​​WH_SHELL для типу хука. Різниця з
            ///RegisterShellHookWindow - це те, що повідомлення надходять через WindowProc вказаного вікна
            ///, а не через процедуру зворотного дзвінка.
            /// </summary>
            /// <param name="hWnd">[in] Зверніться до вікна, щоб зареєструватися для повідомлень гачка Shell.</param>
            /// <returns>TRUE if the function succeeds; FALSE if the function fails. </returns>
            /// <remarks>
            /// Як і у звичайних віконних повідомленнях, другий параметр віконної процедури визначає
            /// повідомлення як "WM_SHELLHOOKMESSAGE". Однак для цих повідомлень про гачок Shell файл
            /// значення повідомлення не є заздалегідь визначеною константою, як інші ідентифікатори повідомлень (ідентифікатори), такі як
            /// WM_COMMAND. Значення потрібно отримувати динамічно за допомогою виклику
            /// RegisterWindowMessage (TEXT ("SHELLHOOK")) ;. Це виключає обробку цих повідомлень за допомогою
            /// традиційний оператор комутатора, який вимагає значень ID, які відомі під час компіляції.
            /// Для обробки повідомлень хука оболонки звичайною практикою є кодування оператора If за замовчуванням
            /// розділу вашого оператора перемикання, а потім обробляти повідомлення, якщо значення ідентифікатора повідомлення
            /// таке саме, як значення, отримане від виклику RegisterWindowMessage.
            /// докладніше див. MSDN
            /// </remarks>
            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool RegisterShellHookWindow(IntPtr hWnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern uint RegisterWindowMessage(string Message);

            [DllImport("user32.dll")]
            internal static extern void SetTaskmanWindow(IntPtr hwnd);
        }
    }
}
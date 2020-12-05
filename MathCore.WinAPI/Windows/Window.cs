using MathCore.WinAPI.PInvoke;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;


namespace MathCore.WinAPI.Windows
{
    public class Window
    {
        private static void ThrowLastWin32Error() => Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        public static Window[] Find(Func<Window, bool> Selector) 
        {
            var result = new List<Window>();

            bool WindowsSelector(IntPtr hWnd, IntPtr lParam) 
            {
                var window = new Window(hWnd);
                if (Selector(window))
                    result.Add(window);
                return true;
            }

            User32.EnumWindows(WindowsSelector, IntPtr.Zero);

            return result.ToArray();
        }
        public IntPtr Handle { get; }
        public string Text 
        {
            get => GetWindowText();
            set 
            {
                if (!SetWindowTest(value))
                    ThrowLastWin32Error();
            }
        }
        public Rectangle Rectangle 
        {
            get
            {
                var rect = new RECT();
                if (!User32.GetWindowRect(Handle, ref rect))
                    ThrowLastWin32Error();
                return rect;
            }
            set 
            {
                if (!User32.MoveWindow(Handle, value.Left, value.Top, value.Width, value.Height, bRepaint: true))
                    ThrowLastWin32Error();
            }
        }

        public Point Location { get => Rectangle.Location; set => Rectangle = new Rectangle(value, Rectangle.Size); }
        public int X { get => Location.X; set => Location = new Point(value, Location.Y); }
        public int Y { get => Location.Y; set => Location = new Point(Location.Y, value); }
        public Size Size { get => Rectangle.Size; set => Rectangle = new Rectangle(Location, value); }
        public int Height { get => Rectangle.Height; set => Size = new Size(Width, value); }
        public int Width { get => Rectangle.Width; set => Size = new Size(value, Height); }

        public Window(IntPtr Handle) => this.Handle = Handle;
        private string GetWindowText() 
        {
            int t = User32.GetWindowTextLength(Handle);
            var buffer = new StringBuilder(t + 1);
            if (buffer.Capacity > 0)
                User32.GetWindowText(Handle, buffer, (uint)buffer.Capacity);
            return buffer.ToString();
        }

        private bool SetWindowTest(string text) => User32.SetWindowText(Handle, text);
        
    }
}

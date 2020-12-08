using MathCore.WinAPI.Windows;
using System;
using System.Diagnostics;
using System.Threading;
using LibWinApi;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {

            var notepad_proc = Process.Start("notepad");

            Console.WriteLine("Wait....");
            Console.ReadLine();

            //var notepad = Window.Find(w => w.Text.EndsWith("Блокнот"));
            //foreach (var w in notepad)
            //{
            //    w.Text = "235";
            //}

            var main_window_hWnd = notepad_proc.MainWindowHandle;
            var window = new Window(main_window_hWnd);

            Console.WriteLine("Text window = {0}", window.Text);
            Console.WriteLine("Coord window = {0}", window.Rectangle);

            for (var i = window.X; i < 365; i += 10)
            {
                window.X = i;
                Thread.Sleep(100);
            }

            window.Text = "Hello World";


            Console.WriteLine("End");
            Console.ReadLine();

            window.Close();
            notepad_proc.CloseMainWindow();


        }
    }
}

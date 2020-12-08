using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibWinApi;

namespace ConsoleTestLibWinApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var monitorHookFactory = new MonitorHookFactory();

            //var kOM = monitorHookFactory.GetKeyboardObserver();
            //kOM.Start();
            //kOM.OnKeyInput += (s, e) =>
            //    { Console.WriteLine($"Key{e.Key.EventType} event of key{e.Key.KeyName}"); };
            //Thread.Sleep(1000);

            //var mOM = monitorHookFactory.GetMouseObserver();
            //mOM.Start();
            //mOM.OnMouseInput += (s, e) =>
            //    {
            //        Console.WriteLine($"Mouse info {e.MouseInfo} at point {e.Point} msg{e.MouseMessage}");
            //    };

            var cOM = monitorHookFactory.GetClipboardObserver();
            cOM.Start();
            cOM.OnClipboardModified += (s, e) =>
            {
                Console.WriteLine("Clipboard updated with data '{0}' of format {1}", e.Data,
                    e.DataFormat.ToString());
            };

            //var aOM = monitorHookFactory.GetApplicationObserver();
            //aOM.Start();
            //aOM.OnAppWindowChange += (s, e) =>
            //{
            //    Console.WriteLine("Application '{0}' window of '{1}' with the title '{2}' was {3}",
            //        e.WindowInfo.AppPath,e.WindowInfo.AppName, e.WindowInfo.AppTitle, e.Events);
            //};
            Console.Read();
            //kOM.Stop();
            //mOM.Stop();
            cOM.Stop();
            //aOM.Stop();
        }
    }
}

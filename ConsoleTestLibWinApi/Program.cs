using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibWinApi;

namespace ConsoleTestLibWinApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var monitorHookFactory = new MonitorHookFactory();
            var keyObsr = monitorHookFactory.GetKeyboardObserver();
            keyObsr.Start();
            keyObsr.OnKeyInput += (s, e) => { Console.WriteLine($"Key{e.Key.EventType} event of key{e.Key.KeyName}"); };

            Console.Read();
            keyObsr.Stop();
            monitorHookFactory.Dispose();
        }
    }
}

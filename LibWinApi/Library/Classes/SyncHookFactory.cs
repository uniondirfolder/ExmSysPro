using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace LibWinApi.Library.Classes
{
    internal class MessageHandler : NativeWindow
    {
        internal MessageHandler()
        {
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
        }
    }
    internal class SyncHookFactory: IDisposable
    {
        private readonly Lazy<MessageHandler> messageHandler;
        private readonly Lazy<TaskScheduler> scheduler;
        private bool hasUIThread;
        internal SyncHookFactory()
        {
            scheduler = new Lazy<TaskScheduler>(() =>
            {
                var dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
                if (dispatcher != null)
                {
                    if (SynchronizationContext.Current != null)
                    {
                        hasUIThread = true;
                        return TaskScheduler.FromCurrentSynchronizationContext();
                    }
                }

                TaskScheduler current = null;

                new Task(() =>
                {
                    Dispatcher.CurrentDispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            Volatile.Write(ref current, TaskScheduler.FromCurrentSynchronizationContext());
                        }), DispatcherPriority.Normal);
                    Dispatcher.Run();
                }).Start();

                while (Volatile.Read(ref current) == null)
                {
                    Thread.Sleep(10);
                }

                return Volatile.Read(ref current);
            });

            messageHandler = new Lazy<MessageHandler>(() =>
            {
                MessageHandler msgHandler = null;
                new Task(e => { Volatile.Write(ref msgHandler, new MessageHandler()); }, GetTaskScheduler()).Start();
                while (Volatile.Read(ref msgHandler) == null)
                {
                    Thread.Sleep(10);
                }
                return Volatile.Read(ref msgHandler);
            });

            Initialize();
        }
        public void Dispose()
        {
            if (messageHandler?.Value != null)
            {
                messageHandler.Value.DestroyHandle();
            }
        }
        private void Initialize()
        {
            GetTaskScheduler();
            GetHandle();
        }
        internal TaskScheduler GetTaskScheduler()
        {
            return scheduler.Value;
        }
        internal IntPtr GetHandle()
        {
            var handle = IntPtr.Zero;

            if (hasUIThread)
            {
                try
                {
                    handle = Process.GetCurrentProcess().MainWindowHandle;

                    if (handle != IntPtr.Zero)
                    {
                        return handle;
                    }
                }
                catch
                {
                }
            }

            return messageHandler.Value.Handle;
        }
    }
}
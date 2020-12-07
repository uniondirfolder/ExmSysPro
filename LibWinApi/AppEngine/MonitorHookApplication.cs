using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Hooks;
using LibWinApi.Library.Import;

namespace LibWinApi.AppEngine
{
    public class MonitorHookApplication
    {
        readonly object _locker = new object();
        public event EventHandler<ApplicationHookEvenArgs> OnAppWindowChange;
        private bool _isRunning;
        private bool _lastEventWasLaunched;
        private IntPtr _lastHwndLaunched;
        private DateTime _prevTimeApplication;
        private readonly SyncHookFactory _shf;
        private Dictionary<IntPtr, WindowInfoObject> _activeWindows;
        private QueueHookConcurrentAsync<object> _applicationsQueue;
        private CancellationTokenSource _taskCancellationTokenSource;
        private WindowEventHook _eventHook;

        internal MonitorHookApplication(SyncHookFactory syncHookFactory)
        {
            _shf = syncHookFactory;
        }
        private void WindowCreated(ShellEventHook shellObject, IntPtr hWnd)
        {
            _applicationsQueue.Enqueue(new WindowInfoObject { HWnd = hWnd, EventType = 0 });
        }
        private void ApplicationStatus(WindowInfoObject wio, EnumApplicationEvents appEvent)
        {
            var timeStamp = DateTime.Now;
            
            wio.AppTitle = appEvent == EnumApplicationEvents.CLOSED ? wio.AppTitle : WindowHookHelper.GetWindowText(wio.HWnd);
            wio.AppPath = appEvent == EnumApplicationEvents.CLOSED ? wio.AppPath : WindowHookHelper.GetAppPath(wio.HWnd);
            wio.AppName = appEvent == EnumApplicationEvents.CLOSED ? wio.AppName : WindowHookHelper.GetAppDescription(wio.AppPath);
            OnAppWindowChange?.Invoke(null,new ApplicationHookEvenArgs() { WindowInfo = wio, Events = appEvent });
        }
        private void WindowCreated(WindowInfoObject wio)
        {
            _activeWindows.Add(wio.HWnd, wio);
            ApplicationStatus(wio, EnumApplicationEvents.LAUNCHED);

            _lastEventWasLaunched = true;
            _lastHwndLaunched = wio.HWnd;
        }
        private void WindowDestroyed(ShellEventHook shellObject, IntPtr hWnd)
        {
            _applicationsQueue.Enqueue(new WindowInfoObject { HWnd = hWnd, EventType = 2 });
        }
        private void WindowActivated(ShellEventHook shellObject, IntPtr hWnd)
        {
            _applicationsQueue.Enqueue(new WindowInfoObject { HWnd = hWnd, EventType = 1 });
        }
        private void WindowActivated(WindowInfoObject wio)
        {
            if (_activeWindows.ContainsKey(wio.HWnd))
            {
                if (!_lastEventWasLaunched && _lastHwndLaunched != wio.HWnd)
                {
                    ApplicationStatus(_activeWindows[wio.HWnd], EnumApplicationEvents.ACTIVATED);
                }
            }

            _lastEventWasLaunched = false;
        }
        private void WindowDestroyed(WindowInfoObject wio)
        {
            if (_activeWindows.ContainsKey(wio.HWnd))
            {
                ApplicationStatus(_activeWindows[wio.HWnd], EnumApplicationEvents.CLOSED);
                _activeWindows.Remove(wio.HWnd);
            }

            _lastEventWasLaunched = false;
        }
        private async Task ApplicationConsumer()
        {
            while (_isRunning)
            {
    
                var item = await _applicationsQueue.DequeueAsync();

                if (item is null)
                {
                    continue;
                }

                if (item is bool)
                {
                    break;
                }

                var wio = (WindowInfoObject)item;
                switch (wio.EventType)
                {
                    case 0:
                        WindowCreated(wio);
                        break;
                    case 1:
                        WindowActivated(wio);
                        break;
                    case 2:
                        WindowDestroyed(wio);
                        break;
                }
            }
        }
        public void Start()
        {
            lock (_locker)
            {
                if (!_isRunning)
                {
                    _prevTimeApplication = DateTime.Now;
                    _activeWindows = new Dictionary<IntPtr, WindowInfoObject>();
                    _taskCancellationTokenSource = new CancellationTokenSource();
                    _applicationsQueue= new QueueHookConcurrentAsync<object>(_taskCancellationTokenSource.Token);
                    
                    Task.Factory.StartNew(() =>
                        {
                            _eventHook = new WindowEventHook(_shf);
                            _eventHook.WindowCreated += WindowCreated;
                            _eventHook.WindowDestroyed += WindowDestroyed;
                            _eventHook.WindowActivated += WindowActivated;
                        },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        _shf.GetTaskScheduler()).Wait();

                    _lastEventWasLaunched = false;
                    _lastHwndLaunched = IntPtr.Zero;

                    Task.Factory.StartNew(ApplicationConsumer);
                    _isRunning = true;
                }
            }
        }
        public void Stop()
        {
            lock (_locker)
            {
                if (_isRunning)
                {
                    Task.Factory.StartNew(() =>
                        {
                            _eventHook.WindowCreated -= WindowCreated;
                            _eventHook.WindowDestroyed -= WindowDestroyed;
                            _eventHook.WindowActivated -= WindowActivated;
                            _eventHook.Destroy();
                        },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        _shf.GetTaskScheduler());

                    _applicationsQueue.Enqueue(false);
                    _isRunning = false;
                    _taskCancellationTokenSource.Cancel();
                }
            }
        }
    }
}
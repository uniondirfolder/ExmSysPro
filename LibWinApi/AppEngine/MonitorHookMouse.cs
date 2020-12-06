using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Hooks;

namespace LibWinApi.AppEngine
{
    public class MonitorHookMouse
    {
        private readonly object _locker = new object();
        private bool _isRunning;
        public event EventHandler<MouseHookEventArgs> OnMouseInput;
        private readonly SyncHookFactory _shf;
        private MouseEventHook _mouseHook;
        private QueueHookConcurrentAsync<object> _mouseQueue;
        private CancellationTokenSource _taskCancellationTokenSource;

        internal MonitorHookMouse(SyncHookFactory shf)
        {
            this._shf = shf;
        }
        private void MouseGetter(object sender, RawMouseEventArgs e)
        {
            _mouseQueue.Enqueue(e);
        }
        private async Task ConsumeKeyAsync()
        {
            while (_isRunning)
            {
                var item = await _mouseQueue.DequeueAsync();

                if (item is null)
                {
                    continue;
                }

                if (item is bool)
                {
                    break;
                }

                MouseGeter_MouseKeyDown(item as RawMouseEventArgs);
            }
        }
        private void MouseGeter_MouseKeyDown(RawMouseEventArgs mkd)
        {
            OnMouseInput?.Invoke(null, new MouseHookEventArgs() { MouseInfo = mkd.MouseInfo, MouseMessage = mkd.MouseMessage, Point = mkd.Point });
        }
        public void Start()
        {
            lock (_locker)
            {
                if (!_isRunning)
                {
                    _taskCancellationTokenSource = new CancellationTokenSource();
                    _mouseQueue = new QueueHookConcurrentAsync<object>(_taskCancellationTokenSource.Token);
                    Task.Factory.StartNew(() =>
                    {
                        _mouseHook = new MouseEventHook();
                        _mouseHook.MouseAction += MouseGetter;
                        _mouseHook.Start();
                    },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        _shf.GetTaskScheduler()).Wait();

                    Task.Factory.StartNew(() => ConsumeKeyAsync());

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
                    if (_mouseHook != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            _mouseHook.MouseAction -= MouseGetter;
                            _mouseHook.Stop();
                            _mouseHook = null;
                        },
                            CancellationToken.None,
                            TaskCreationOptions.None,
                            _shf.GetTaskScheduler());
                    }

                    _mouseQueue.Enqueue(false);
                    _isRunning = false;
                    _taskCancellationTokenSource.Cancel();
                }
            }
        }
    }
}
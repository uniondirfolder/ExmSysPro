using System;
using System.Threading;
using System.Threading.Tasks;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Hooks;

namespace LibWinApi.AppEngine
{
    public class MonitorHookKeyboard
    {
        readonly object _locker = new object();
        public event EventHandler<KeyInputHookEventArgs> OnKeyInput;
        private bool _isRunning;
        private readonly SyncHookFactory _shf;
        private QueueHookConcurrentAsync<object> _keyQueue;
        private CancellationTokenSource _taskCancellationTokenSource;
        private KeyboardEventHook _keyboardEventHook;
        internal MonitorHookKeyboard(SyncHookFactory shf)
        {
            _shf = shf;
        }
        private void KeyGeter(object sender, RawKeyEventArgs e)
        {
            _keyQueue.Enqueue(new KeyInfo()
            {
                UnicodeCharacter = e.Character,
                KeyName = e.Key.ToString(),
                EventType = (EnumKeyHookEvent)e.EventType
            });
        }
        private void KeyGeter_KeyDown(KeyInfo ki)
        {
            OnKeyInput?.Invoke(null, new KeyInputHookEventArgs() { Key = ki });
        }
        private async Task ConsumeKeyAsync()
        {
            while (_isRunning)
            {
                var item = await _keyQueue.DequeueAsync();

                if (item is null)
                {
                    continue;
                }

                if (item is bool)
                {
                    break;
                }

                KeyGeter_KeyDown((KeyInfo)item);
            }
        }
        public void Start()
        {
            lock (_locker)
            {
                if (!_isRunning)
                {
                    _taskCancellationTokenSource = new CancellationTokenSource();
                    _keyQueue = new QueueHookConcurrentAsync<object>(_taskCancellationTokenSource.Token);

                    Task.Factory.StartNew(() =>
                        {
                            _keyboardEventHook = new KeyboardEventHook();
                            _keyboardEventHook.KeyDown += KeyGeter;
                            _keyboardEventHook.KeyUp += KeyGeter;
                            _keyboardEventHook.Start();
                        },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        _shf.GetTaskScheduler()).Wait();

                    Task.Factory.StartNew(ConsumeKeyAsync);

                    _isRunning = true;
                }
            }
        }
        public void Stop()
        {
            lock (_locker)
            {
                if (!_isRunning)
                {
                    if (_keyboardEventHook != null)
                    {
                        Task.Factory.StartNew(() =>
                            {
                                _keyboardEventHook.KeyDown -= KeyGeter;
                                _keyboardEventHook.Stop();
                                _keyboardEventHook = null;
                            },
                            CancellationToken.None,
                            TaskCreationOptions.None,
                            _shf.GetTaskScheduler());
                    }
                    _keyQueue.Enqueue(false);
                    _isRunning = false;
                    _taskCancellationTokenSource.Cancel();
                }
            }
        }
    }
}
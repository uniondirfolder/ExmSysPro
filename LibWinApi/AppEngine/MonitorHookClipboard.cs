using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Hooks;

namespace LibWinApi.AppEngine
{
    public class MonitorHookClipboard
    {
        public bool IsRunning;
        private readonly object _locker = new object();
        private readonly SyncHookFactory _shf;
        private ClipBoardEventHook _clipBoard;
        private QueueHookConcurrentAsync<object> _clipQueue;
        private CancellationTokenSource _taskCancellationTokenSource;
        internal MonitorHookClipboard(SyncHookFactory shf)
        {
            this._shf = shf;
        }
        public event EventHandler<ClipboardHookEventArgs> OnClipboardModified;
        private void ClipboardHandler(object sender, EventArgs e)
        {
            _clipQueue.Enqueue(sender);
        }
        private async Task ClipConsumerAsync()
        {
            while (IsRunning)
            {
                var item = await _clipQueue.DequeueAsync();

                if (item is null)
                {
                    continue;
                }

                if (item is bool)
                {
                    break;
                }

                ClipboardHandler(item);
            }
        }
        private void ClipboardHandler(object sender)
        {
            IDataObject iData = (DataObject)sender;

            var format = default(EnumClipboardContentTypes);

            object data = null;

            bool validDataType = false;
            if (iData.GetDataPresent(DataFormats.Text))
            {
                format = EnumClipboardContentTypes.PLAIN_TEXT;
                data = iData.GetData(DataFormats.Text);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.Rtf))
            {
                format = EnumClipboardContentTypes.RICH_TEXT;
                data = iData.GetData(DataFormats.Rtf);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.CommaSeparatedValue))
            {
                format = EnumClipboardContentTypes.CSV;
                data = iData.GetData(DataFormats.CommaSeparatedValue);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.Html))
            {
                format = EnumClipboardContentTypes.HTML;
                data = iData.GetData(DataFormats.Html);
                validDataType = true;
            }

            else if (iData.GetDataPresent(DataFormats.StringFormat))
            {
                format = EnumClipboardContentTypes.PLAIN_TEXT;
                data = iData.GetData(DataFormats.StringFormat);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.UnicodeText))
            {
                format = EnumClipboardContentTypes.UNICODE_TEXT;
                data = iData.GetData(DataFormats.UnicodeText);
                validDataType = true;
            }

            if (!validDataType)
            {
                return;
            }

            OnClipboardModified?.Invoke(null, new ClipboardHookEventArgs { Data = data, DataFormat = format });
        }
        public void Start()
        {
            lock (_locker)
            {
                if (!IsRunning)
                {
                    _taskCancellationTokenSource = new CancellationTokenSource();
                    _clipQueue = new QueueHookConcurrentAsync<object>(_taskCancellationTokenSource.Token);

                    Task.Factory.StartNew(() =>
                    {
                        _clipBoard = new ClipBoardEventHook();
                        _clipBoard.RegisterClipboardViewer();
                        _clipBoard.ClipBoardChanged += ClipboardHandler;
                    },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        _shf.GetTaskScheduler()).Wait();

                    Task.Factory.StartNew(ClipConsumerAsync);

                    IsRunning = true;
                }
            }
        }
        public void Stop()
        {
            lock (_locker)
            {
                if (IsRunning)
                {
                    if (_clipBoard != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            _clipBoard.ClipBoardChanged -= ClipboardHandler;
                            _clipBoard.UnregisterClipboardViewer();
                            _clipBoard.Dispose();
                        },
                            CancellationToken.None,
                            TaskCreationOptions.None,
                            _shf.GetTaskScheduler());
                    }

                    IsRunning = false;
                    _clipQueue.Enqueue(false);
                    _taskCancellationTokenSource.Cancel();
                }
            }
        }
    }
}

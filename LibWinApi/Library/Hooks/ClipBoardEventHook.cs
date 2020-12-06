using System;
using System.Threading;
using System.Windows.Forms;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Import;

namespace LibWinApi.Library.Hooks
{
    internal class ClipBoardEventHook:Form
    {
        private IntPtr _clipboardViewerNext;
        internal event EventHandler ClipBoardChanged = delegate { };
        internal void RegisterClipboardViewer()
        {
            _clipboardViewerNext = DllUser32.SetClipboardViewer(Handle);
        }
        internal void UnregisterClipboardViewer()
        {
            DllUser32.ChangeClipboardChain(Handle, _clipboardViewerNext);
        }
        private void GetClipboardData()
        {
            Exception threadEx = null;
            var staThread = new Thread(
                delegate ()
                {
                    try
                    {
                        var iData = Clipboard.GetDataObject();
                        ClipBoardChanged(iData, new EventArgs());
                    }

                    catch (Exception ex)
                    {
                        threadEx = ex;
                    }
                });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }
        protected override void WndProc(ref Message m)
        {
            switch ((EnumWinMsgs)m.Msg)
            {
                case EnumWinMsgs.WM_DRAWCLIPBOARD:
                    GetClipboardData();
                    DllUser32.SendMessage(_clipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;
                case EnumWinMsgs.WM_CHANGECBCHAIN:
                    if (m.WParam == _clipboardViewerNext)
                    {
                        _clipboardViewerNext = m.LParam;
                    }
                    else
                    {
                        DllUser32.SendMessage(_clipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
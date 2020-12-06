using LibWinApi.Library.Classes;
using LibWinApi.Library.Delegates;

namespace LibWinApi.Library.Hooks
{
    internal class KeyboardEventHook
    {
        private KeyboardListener _keyboardListener;
        internal event RawKeyEventHandler KeyDown = delegate { };
        internal event RawKeyEventHandler KeyUp = delegate { };

        private void KbListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            KeyDown(sender, args);
        }

        private void KBListener_KeyUp(object sender, RawKeyEventArgs args)
        {
            KeyUp(sender, args);
        }
        internal void Start()
        {
            _keyboardListener= new KeyboardListener();
            _keyboardListener.KeyDown += KbListener_KeyDown;
            _keyboardListener.KeyUp += KBListener_KeyUp;
        }

        internal void Stop()
        {
            _keyboardListener.KeyDown -= KbListener_KeyDown;
            _keyboardListener.KeyUp -= KBListener_KeyUp;
            _keyboardListener.Dispose();
        }
    }
}
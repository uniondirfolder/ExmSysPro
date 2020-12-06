using System;

namespace LibWinApi.Library.Classes
{
    public class KeyInputHookEventArgs:EventArgs
    {
        public KeyInfo Key { get; set; }
    }
}
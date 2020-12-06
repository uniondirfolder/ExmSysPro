using System;
using System.Windows.Input;

namespace LibWinApi.Library.Classes
{
    internal class RawKeyEventArgs:EventArgs
    {
        internal string Character;
        internal int EventType;
        internal bool IsSysKey;
        internal Key Key;
        internal int VkCode;
        internal RawKeyEventArgs(int vkCode, bool isSysKey, string character, int type)
        {
            VkCode = vkCode;
            IsSysKey = isSysKey;
            Character = character;
            Key = KeyInterop.KeyFromVirtualKey(vkCode);
            EventType = type;
        }
        public override string ToString()
        {
            return Character;
        }
    }
}
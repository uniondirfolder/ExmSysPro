﻿using System;
using System.Windows.Input;

namespace LibWinApi.Library.Classes
{
    internal class RawKeyEventArgs:EventArgs
    {
        /// <summary>
        ///     Unicode character of key pressed.
        /// </summary>
        internal string Character;

        /// <summary>
        ///     Up(1) or Down(0)
        /// </summary>
        internal int EventType;

        /// <summary>
        ///     Is the hitted key system key.
        /// </summary>
        internal bool IsSysKey;

        /// <summary>
        ///     WPF Key of the key.
        /// </summary>
        internal Key Key;

        /// <summary>
        ///     VKCode of the key.
        /// </summary>
        internal int VkCode;

        /// <summary>
        ///     Create raw keyevent arguments.
        /// </summary>
        /// <param name="vkCode"></param>
        /// <param name="isSysKey"></param>
        /// <param name="character">Character</param>
        /// <param name="type"></param>
        internal RawKeyEventArgs(int vkCode, bool isSysKey, string character, int type)
        {
            VkCode = vkCode;
            IsSysKey = isSysKey;
            Character = character;
            Key = KeyInterop.KeyFromVirtualKey(vkCode);
            EventType = type;
        }
        /// <summary>
        ///     Convert to string.
        /// </summary>
        /// <returns>Returns string representation of this key, if not possible empty string is returned.</returns>
        public override string ToString()
        {
            return Character;
        }
    }
}
using LibWinApi.Library.Enums;
using LibWinApi.Library.Structs;

namespace LibWinApi.Library.Classes
{
    internal class RawMouseEventArgs
    {
        internal EnumWinMsgs MouseMessage { get; set; }
        internal StructPoint Point { get; set; }
        internal uint MouseData { get; set; }
    }
}
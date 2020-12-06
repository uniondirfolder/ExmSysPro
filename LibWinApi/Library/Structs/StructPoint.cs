using System.Runtime.InteropServices;

namespace LibWinApi.Library.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct StructPoint
    {
        public readonly int x;
        public readonly int y;
    }
}
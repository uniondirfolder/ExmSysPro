using System.Runtime.InteropServices;

namespace LibWinApi.Library.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct StructPoint
    {
        public readonly int x;
        public readonly int y;

        public override string ToString()
        {
            return $"Point X:{x} - Y:{y}";
        }
    }
}
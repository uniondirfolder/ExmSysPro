using System.Drawing;

namespace LibWinApi.Library.Structs
{
    internal struct StructRECT
    {
        internal int left;
        internal int top;
        internal int right;
        internal int bottom;

        public StructRECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        internal Rectangle Rect => new Rectangle(left, top, right - left, bottom - top);

        internal static StructRECT FromXYWH(int x, int y, int width, int height)
        {
            return new StructRECT(x, y, x + width, y + height);
        }

        internal static StructRECT FromRectangle(Rectangle rectangle)
        {
            return new StructRECT(rectangle.Left,rectangle.Top,rectangle.Right,rectangle.Bottom);
        }

        public override string ToString()
        {
            return $"left:{left} top:{top} right:{right} bottom:{bottom}";
        }
    }
}

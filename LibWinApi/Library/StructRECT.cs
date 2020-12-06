using System;
using System.Drawing;

namespace LibWinApi.Library
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
    }
}

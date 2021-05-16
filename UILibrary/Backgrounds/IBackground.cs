using UILibrary.Drawing;
using SharpDX.Mathematics.Interop;
using System;

namespace UILibrary.Backgrounds
{
    public interface IBackground
    {
        void Draw(DrawingContext context, RawRectangleF bounds);
    }
}

using UILibrary.Drawing;
using SharpDX.Mathematics.Interop;

namespace UILibrary.Backgrounds
{
    public class TextureBackground : IBackground
    {
        private string _bitmap;

        public TextureBackground(string bitmap)
        {
            _bitmap = bitmap;
        }

        public void Draw(DrawingContext context, RawRectangleF bounds)
        {
            context.DrawBitmap(_bitmap, bounds);
        }
    }
}

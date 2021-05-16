﻿using UILibrary.Drawing;
using SharpDX;

namespace UILibrary.Elements
{
    public class UIPanel : UIElement
    {
        public UIPanel(Vector2 position, Vector2 size)
            : base(position, size)
        {
        }

        internal override void Draw(DrawingContext context)
        {
        }
    }
}

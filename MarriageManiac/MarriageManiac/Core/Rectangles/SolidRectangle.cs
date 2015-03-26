using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.Core.Rectangles
{
    class SolidRectangle : Drawable
    {
        public SolidRectangle(int x, int y, int width, int height, Color color)
            : base(x, y, GraphicsHelper.CreatePixel(color))
        {
            Bounds = new Rectangle(x, y, width, height);
        }
    }
}
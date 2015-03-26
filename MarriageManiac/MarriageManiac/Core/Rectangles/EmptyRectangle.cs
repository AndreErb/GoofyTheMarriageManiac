using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Core.Rectangles
{
    class EmptyRectangle : Drawable
    {
        public EmptyRectangle(int x, int y, int width, int height, int borderThickness, Color color)
            : base(x, y, null)
        {
            Bounds = new Rectangle(x, y, width, height);
            Color = color;
            Pixel = GraphicsHelper.CreatePixel(color);
            BorderThickness = borderThickness;
        }

        private Texture2D Pixel { get; set; }
        private Color Color { get; set; }
        private int BorderThickness { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                // Draw top line
                spriteBatch.Draw(Pixel, new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, BorderThickness), Color);

                // Draw left line
                spriteBatch.Draw(Pixel, new Rectangle(Bounds.X, Bounds.Y, BorderThickness, Bounds.Height), Color);

                // Draw right line
                spriteBatch.Draw(Pixel, new Rectangle((Bounds.X + Bounds.Width - BorderThickness),
                                                Bounds.Y,
                                                BorderThickness,
                                                Bounds.Height), Color);
                // Draw bottom line
                spriteBatch.Draw(Pixel, new Rectangle(Bounds.X,
                                                Bounds.Y + Bounds.Height - BorderThickness,
                                                Bounds.Width,
                                                BorderThickness), Color);
            }
        }
    }
}
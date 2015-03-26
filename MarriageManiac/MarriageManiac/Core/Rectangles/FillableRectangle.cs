using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Core.Rectangles
{
    class FillableRectangle : EmptyRectangle
    {
        decimal _FillPercentage;

        public FillableRectangle(int x, int y, int width, int height, int boarderThickness, Color borderColor, Color contentColor)
            : base(x, y, width, height, boarderThickness, borderColor)
        {
            Bounds = new Rectangle(x, y, width, height);
            ContentRectangle = new SolidRectangle(x + boarderThickness,
                                                  y + boarderThickness,
                                                  width - (boarderThickness * 2),
                                                  height - (boarderThickness * 2),
                                                  contentColor);

            OriginalContentBounds = ContentRectangle.Bounds;
        }

        private SolidRectangle ContentRectangle { get; set; }
        private Rectangle OriginalContentBounds{ get; set; }

        public decimal FillPercentage
        {
            get { return _FillPercentage; }
            set
            {
                _FillPercentage = value;

                ContentRectangle.Bounds = new Rectangle(OriginalContentBounds.X,
                                                        OriginalContentBounds.Y,
                                                        (int)((OriginalContentBounds.Width / 100m) * _FillPercentage),
                                                        OriginalContentBounds.Height);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the boarder.
            base.Draw(spriteBatch);

            // Draw the content.
            ContentRectangle.Draw(spriteBatch);
        }
    }
}

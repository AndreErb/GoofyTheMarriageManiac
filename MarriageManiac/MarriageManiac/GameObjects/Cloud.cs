using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;

namespace MarriageManiac.GameObjects
{
    class Cloud : Drawable
    {
        public Cloud(int startX, int startY, Texture2D image, float speed)
            : base(startX, startY, image)
        {
            Speed = speed;
        }

        public float Speed { get; set; }

        public override void Update(GameTime gameTime)
        {
            Position = new Vector2(Position.X - Speed, Position.Y);

            if (Position.X + CurrentImage.Width < 0)
            {
                Position = new Vector2(GoofyGame.SCREENWIDTH, Position.Y);
            }
        }
    }
}
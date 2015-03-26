using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Core
{
    public class Drawable : Rotation, IDrawable
    {
        public enum OriginPoint { Center };

        Vector2 _Position;

        public Drawable(int startX, int startY, Texture2D image)
        {
            CurrentImage = image;
            Position = new Vector2(startX, startY);
            Visible = true;

            if (image != null)
            {
                Bounds = new Rectangle(startX, startY, image.Width, image.Height);
            }
        }

        public Texture2D CurrentImage { get; set; }
        public virtual bool Visible { get; set; }
        public Rectangle Bounds { get; set; }

        public Vector2 Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
            }
        }



        public void SetOrigin(OriginPoint point)
        {
            switch (point)
            {
                case OriginPoint.Center:
                    Origin = new Vector2(Bounds.Width / 2, Bounds.Height / 2);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                var destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Bounds.Width, Bounds.Height);
                spriteBatch.Draw(CurrentImage, destinationRectangle, null, Color.White, Angle, Origin, SpriteEffects.None, 0f);
            }
        }
    }
}
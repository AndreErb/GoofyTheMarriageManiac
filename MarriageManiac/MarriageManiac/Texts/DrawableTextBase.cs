using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;

namespace MarriageManiac.Texts
{
    public abstract class DrawableTextBase : Rotation, IDrawableText
    {
        Vector2 _Position;

        public DrawableTextBase(int xPos, int yPos, string font, Color color, string texture)
        {
            _Position = new Vector2(xPos, yPos);
            Text = null;
            Font = ContentStore.LoadFont(font);
            Color = color;
            Scale = 1.0f;
            Visible = true;
            Effect = SpriteEffects.None;
            Bounds = new Rectangle();

            if (!String.IsNullOrEmpty(texture))
            {
                CurrentImage = ContentStore.LoadImage(texture);
            }
        }

        public string Text { get; set; }
        public SpriteFont Font { get; private set; }
        public bool Visible { get; set; }
        public Texture2D CurrentImage { get; set; }
        public Color Color { get; set; }
        public float Scale { get; set; }
        public SpriteEffects Effect { get; set; }
        public event EventHandler Completed;
        public Rectangle Bounds { get; set; }

        public Vector2 Position
        {
            get { return _Position; }
        }

        protected void Complete()
        {
            if (Completed != null)
            {
                Completed(this, new EventArgs());
            }
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                if (CurrentImage != null)
                {
                    var position = new Rectangle(100, 100, CurrentImage.Width, CurrentImage.Height);
                    spriteBatch.Draw(CurrentImage, position, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0f);
                }

                var origin = new Vector2();
                spriteBatch.DrawString(Font, Text, Position, Color, Angle, origin, Scale, Effect, 0.5f);
            }
        }
    }
}
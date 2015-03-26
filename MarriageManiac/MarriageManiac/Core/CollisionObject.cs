using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Core
{
    public class CollisionObject : Rotation, IGameContent
    {
        Rectangle _Bounds = new Rectangle();

        public CollisionObject(Rectangle bounds)
        {
            _Bounds = bounds;
            Visible = true;
            CanCollide = true;
            Origin = new Vector2();
        }

        public bool Visible { get; set; }
        public Texture2D CurrentImage { get; set; }
        public bool CanCollide { get; set; }
        public event EventHandler<WouldCollideEventArgs> WouldCollideWith;

        public Rectangle Bounds 
        { 
            get 
            { 
                return _Bounds; 
            } 
        }

        public Vector2 Position
        {
            get 
            { 
                return new Vector2((float)_Bounds.Location.X, (float)_Bounds.Location.Y); 
            }            
        }


        public ICollidable WouldCollide(IEnumerable<ICollidable> collidables, Point position)
        {
            ICollidable wouldCollideWith = null;

            if (CanCollide)
            {
                var bounds = new Rectangle(position.X, position.Y, Bounds.Width, Bounds.Height);

                wouldCollideWith = collidables.FirstOrDefault(c => c != this && c.CollidesWith(bounds));

                if (wouldCollideWith != null && WouldCollideWith != null)
                {
                    WouldCollideWith(this, new WouldCollideEventArgs(wouldCollideWith));
                }
            }

            return wouldCollideWith;
        }


        public bool CollidesWith(Rectangle bounds)
        {
            if (CanCollide)
            {
                return bounds.Intersects(Bounds);
            }
            else
            {
                return false;
            }
        }

        public virtual void Update(Scene scene, GameTime gameTime)
        {
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

    public class WouldCollideEventArgs : EventArgs
    {
        public WouldCollideEventArgs(ICollidable wouldCollideWith)
        {
            WouldCollideWith = wouldCollideWith;
        }

        public ICollidable WouldCollideWith { get; private set; }
    }
}
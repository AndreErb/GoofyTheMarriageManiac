using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Core
{
    class DrawableMovableCollidable : DrawableMovable, ICollidable
    {
        public DrawableMovableCollidable(int startX, int startY, Texture2D image)
            : base(startX, startY, image)
        {
            if (image != null)
            {

                Bounds = new Rectangle((int)Position.X, (int)Position.Y, image.Width, image.Height);
            }
            else
            {
                Bounds = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            }
        }

        public event EventHandler<WouldCollideEventArgs> WouldCollideWith;

        public bool CanCollide { get; set; }


        public override void Update(GameTime gameTime)
        {
            // Update the collidable object, used by this class.
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Bounds.Width, Bounds.Height);

            base.Update(gameTime);
        }

        public ICollidable WouldCollide(IEnumerable<ICollidable> collidables, Point position)
        {
            ICollidable wouldCollideWith = null;

            if (CanCollide)
            {
                var bounds = new Rectangle(position.X, position.Y, Bounds.Width, Bounds.Height);

                wouldCollideWith = collidables.FirstOrDefault(c => c != this && c.CollidesWith(bounds));

                if (wouldCollideWith != null)
                {
                    OnWouldCollideWith(wouldCollideWith);
                }
            }

            return wouldCollideWith;
        }
        
        public virtual bool CollidesWith(Rectangle bounds)
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
        
        protected void OnWouldCollideWith(ICollidable collidable)
        {
            if (WouldCollideWith != null)
            {
                WouldCollideWith(this, new WouldCollideEventArgs(collidable));
            }
        }
    }
}
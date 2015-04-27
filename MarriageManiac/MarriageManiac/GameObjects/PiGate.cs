using MarriageManiac.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarriageManiac.GameObjects
{
    class PiGate : DrawableMovableCollidable, IGameContent
    {
        public PiGate(int x, int y)
            : base(x, y, ContentStore.LoadImage("pi"))
        {
            CanCollide = true;
        }

        public override bool CollidesWith(Microsoft.Xna.Framework.Rectangle bounds)
        {
            if (CanCollide)
            {
                // We fake the X position, so that Goofy can walk into the middle of the gate, until he collides.
                var fakeBounds = new Rectangle(Bounds.X + (Bounds.Width / 2), Bounds.Y, 1, Bounds.Height);
                return bounds.Intersects(fakeBounds);
            }
            else
            {
                return false;
            }
        }
    }
}

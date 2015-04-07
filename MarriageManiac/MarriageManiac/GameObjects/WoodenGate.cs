using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;
using Microsoft.Xna.Framework;

namespace MarriageManiac.GameObjects
{
    class WoodenGate : DrawableMovableCollidable, IGameContent
    {
        public WoodenGate(int x, int y)
            : base(x, y, ContentStore.LoadImage("WoodenGate"))
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
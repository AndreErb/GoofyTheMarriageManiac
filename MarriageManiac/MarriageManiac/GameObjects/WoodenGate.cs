using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;

namespace MarriageManiac.GameObjects
{
    class WoodenGate : DrawableMovableCollidable, IGameContent
    {
        public WoodenGate(int x, int y)
            : base(x, y, ContentStore.LoadImage("WoodenGate"))
        {
            CanCollide = false;
        }
    }
}
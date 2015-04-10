using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;

namespace MarriageManiac.GameObjects
{
    class Clock : DrawableMovableCollidable, IGameContent
    {
        public Clock(int x, int y)
            : base(x, y, ContentStore.LoadImage("Clock"))
        {
            CanCollide = true;
        }
    }
}
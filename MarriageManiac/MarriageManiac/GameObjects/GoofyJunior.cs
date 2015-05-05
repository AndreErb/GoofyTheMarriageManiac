using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;
using Microsoft.Xna.Framework;

namespace MarriageManiac.GameObjects
{
    class GoofyJunior : DrawableMovableCollidable, IGameContent
    {
        public GoofyJunior(int x, int y)
            : base(x, y, ContentStore.LoadImage("GoofyJunior"))
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.GameObjects
{
    class GoofyIcon : DrawableMovableCollidable, IGameContent
    {
        public GoofyIcon(int x, int y)
            : base(x, y, ContentStore.LoadImage("GoofyLogo"))
        {
        }
    }
}
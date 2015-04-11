using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class Mini : StandardCharacter
    {
        public Mini(int startX, int startY)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("Mini");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
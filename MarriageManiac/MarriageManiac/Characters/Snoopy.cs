using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class Snoopy : StandardCharacter
    {
        public Snoopy(int startX, int startY)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("SnoopyLinks");
            ImageRight = ContentStore.LoadImage("SnoopyRechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
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
        public int ActualQuestion { get; set; }

        public Snoopy(int startX, int startY, bool isLeft)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("SnoopyLinks");
            ImageRight = ContentStore.LoadImage("SnoopyRechts");

            if (isLeft)
            {
                CurrentImage = ImageLeft;
            }
            else
            {
                CurrentImage = ImageRight;
            }
            
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
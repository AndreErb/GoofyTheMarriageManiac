using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class Garloff : StandardCharacter
    {
        public Garloff(int startX, int startY)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("GarloffLinks");
            ImageRight = ContentStore.LoadImage("GarloffRechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
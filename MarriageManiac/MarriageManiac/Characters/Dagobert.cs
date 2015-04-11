using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class Dagobert : StandardCharacter
    {
        public Dagobert(int startX, int startY)
            : base()
        {
            ImageRight = ContentStore.LoadImage("Dagobert");

            CurrentImage = ImageRight;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class Daisy : StandardCharacter
    {
        public Daisy(int startX, int startY)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("DaisyInLove");
            ImageRight = ContentStore.LoadImage("DaisyFear");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);

            SoundStore.Create("FemaleScream");
        }

        public void Scream()
        {
            SoundStore.Sound("FemaleScream").Instance.Play();
        }
    }
}
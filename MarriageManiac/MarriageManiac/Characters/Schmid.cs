using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class Schmid : StandardCharacter
    {
        public Schmid(int startX, int startY)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("SchmidLinks");
            ImageRight = ContentStore.LoadImage("SchmidRechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
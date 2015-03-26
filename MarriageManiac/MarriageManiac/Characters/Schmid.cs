using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Characters
{
    class Schmid : StandardCharacter
    {
        public Schmid(int startX, int startY)
            : base()
        {
            ImageLeft = GoofyGame.CONTENT.Load<Texture2D>("SchmidLinks");
            ImageRight = GoofyGame.CONTENT.Load<Texture2D>("SchmidRechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Characters
{
    class Snoopy : StandardCharacter
    {
        public Snoopy(int startX, int startY)
            : base()
        {
            ImageLeft = GoofyGame.CONTENT.Load<Texture2D>("SnoopyLinks");
            ImageRight = GoofyGame.CONTENT.Load<Texture2D>("SnoopyRechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
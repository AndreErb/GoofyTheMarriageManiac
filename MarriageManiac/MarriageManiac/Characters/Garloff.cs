using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.Characters
{
    class Garloff : StandardCharacter
    {
        public Garloff(int startX, int startY)
            : base()
        {
            ImageLeft = GoofyGame.CONTENT.Load<Texture2D>("GarloffLinks");
            ImageRight = GoofyGame.CONTENT.Load<Texture2D>("GarloffRechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
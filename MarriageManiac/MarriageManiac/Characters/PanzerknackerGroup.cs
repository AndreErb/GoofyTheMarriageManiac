using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.Characters
{
    class PanzerknackerGroup : StandardCharacter
    {
        public PanzerknackerGroup(int startX, int startY)
            : base()
        {
            ImageLeft = GoofyGame.CONTENT.Load<Texture2D>("Panzerknacker_Links");
            ImageRight = GoofyGame.CONTENT.Load<Texture2D>("Panzerknacker_Rechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
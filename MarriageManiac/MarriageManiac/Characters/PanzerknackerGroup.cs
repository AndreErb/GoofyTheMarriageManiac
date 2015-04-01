using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class PanzerknackerGroup : StandardCharacter
    {
        public PanzerknackerGroup(int startX, int startY)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("Panzerknacker_Links");
            ImageRight = ContentStore.LoadImage("Panzerknacker_Rechts");

            CurrentImage = ImageLeft;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    class TickTickTrack : StandardCharacter
    {
        public TickTickTrack(int startX, int startY)
            : base()
        {
            ImageRight = ContentStore.LoadImage("TickTrickTrack");

            CurrentImage = ImageRight;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);
        }
    }
}
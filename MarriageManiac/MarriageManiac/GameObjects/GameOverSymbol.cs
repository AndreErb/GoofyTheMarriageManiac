using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.GameObjects
{
    class GameOverSymbol : Drawable
    {
        private static Texture2D Image;
        
        static GameOverSymbol()
        {
            Image = GoofyGame.CONTENT.Load<Texture2D>("GameOver");
        }

        public GameOverSymbol()
            : base((GoofyGame.SCREENWIDTH - Image.Width) / 2, (GoofyGame.SCREENHEIGHT - Image.Height) / 2, Image)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MarriageManiac.Core
{
    static class ContentStore
    {
        public static Texture2D LoadImage(string imageName)
        {
            return GoofyGame.CONTENT.Load<Texture2D>("Images\\" + imageName);
        }

        public static SoundEffect LoadSound(string soundName)
        {
            return GoofyGame.CONTENT.Load<SoundEffect>("Audio\\" + soundName);
        }

        public static SpriteFont LoadFont(string fontName)
        {
            return GoofyGame.CONTENT.Load<SpriteFont>("Fonts\\" + fontName);
        }
    }
}
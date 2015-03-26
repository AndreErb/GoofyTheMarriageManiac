using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MarriageManiac.Texts
{
    public class Text : DrawableTextBase
    {
        bool _Completed;

        public Text(int xPos, int yPos, string font, Color color, string text, string texture)
            : base(xPos, yPos, font, color, texture)
        {
            Text = text;
            _Completed = false;
        }

        public override void Update(GameTime gameTime)
        {
            // Auto-complete the text.
            if (!_Completed) // Only once!
            {
                Complete();
            }
        }
    }
}
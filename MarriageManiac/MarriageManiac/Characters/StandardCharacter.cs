using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarriageManiac.Characters
{
    abstract class StandardCharacter : CharacterBase
    {
        public StandardCharacter()
            : base()
        { }

        public Texture2D ImageLeft { get; set; }
        public Texture2D ImageRight { get; set; }

        public override void Update(Scene scene, GameTime gameTime)
        {
            if (Direction == Direction.Left)
            {
                CurrentImage = ImageLeft;
            }
            else if (Direction == Direction.Right)
            {
                CurrentImage = ImageRight;
            }

            base.Update(scene, gameTime);
        }
    }
}
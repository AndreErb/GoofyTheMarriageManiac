using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;

namespace MarriageManiac.GameObjects
{
    class Flash : Drawable
    {
        Random _Random;

        public Flash(int startX, int startY, Texture2D image)
            : base(startX, startY, image)
        {
            _Random = new Random();
            Started = false;
        }

        bool Started { get; set; }

        public void Start()
        {
            Started = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Started)
            {
                int randomNumber = _Random.Next(0, 10);

                if (randomNumber > 8)
                {
                    Visible = true;
                }
                else
                {
                    Visible = false;
                }
            }
        }
    }
}
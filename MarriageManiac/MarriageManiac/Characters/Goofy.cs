﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.GameObjects;
using MarriageManiac.Core;

namespace MarriageManiac.Characters
{
    public class Goofy : CharacterBase
    {
        TimeSpan _TimeSinceLastShot = new TimeSpan();

        public Goofy(int startX, int startY)
            : base()
        {
            ImageLeft = ContentStore.LoadImage("GoofyRunningLeft");
            ImageRight = ContentStore.LoadImage("GoofyRunningRight");

            CurrentImage = ImageRight;
            Bounds = new Rectangle(startX, startY, CurrentImage.Width, CurrentImage.Height);

            LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(Goofy_LifeAmountChanged);
            var laughing = SoundStore.Create("GoofyLaugh");
        }
                        
        private Texture2D ImageLeft { get; set; }
        private Texture2D ImageRight { get; set; }
        public bool IsRemoteControlled { get; set; }
        public int Lifes { get; set; }
     
        public override void Update(Scene scene, GameTime gameTime)
        {
            if (!IsRemoteControlled)
            {
                KeyboardState keyboard = Keyboard.GetState();

                MoveUserControlled(keyboard);
                Shoot(keyboard, scene, gameTime);
            }
            
            base.Update(scene, gameTime);
        }

        public void Laugh()
        {
            SoundStore.Sound("GoofyLaugh").Instance.Play();
        }

        private void MoveUserControlled(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Left))
            {
                CurrentImage = ImageLeft;
                Move(Direction.Left);
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                CurrentImage = ImageRight;
                Move(Direction.Right);
            }
            else
            {
                Move(Direction.None);
            }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                Jump(JumpMode.Jump);
            }
            else
            {
                Jump(JumpMode.Not);
            }
        }

        private void Shoot(KeyboardState keyboard, Scene scene, GameTime gameTime)
        {
            var elapsed = gameTime.TotalGameTime - _TimeSinceLastShot;

            if (elapsed >= TimeSpan.FromSeconds(0.5))
            {
                var key = Keyboard.GetState();
                if (key.IsKeyDown(Keys.X))
                {
                    var shot = new LoveShot(this);
                    scene.Add(shot);

                    shot.Shoot();
                    _TimeSinceLastShot = gameTime.TotalGameTime;
                }
            }
        }
        
        void Goofy_LifeAmountChanged(object sender, LifeAmountChangedArgs e)
        {
            if (e.CurrentLifePercentage <= 0)
            {
                Lifes -= 1;          
            }
        }
    }
}
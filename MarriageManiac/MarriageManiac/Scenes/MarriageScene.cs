﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Texts;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using System.Threading;

namespace MarriageManiac.Scenes
{
    public class MarriageScene : Scene
    {
        Daisy _Daisy = null;
        Vector2 _GoofyTempSize;
        Vector2 _DaisyTempSize;

        public MarriageScene()
            : base()
        {
            // Create and play the background music.
            var music = SoundStore.Create("ChurchBell");
            music.Instance.IsLooped = true;
            music.Instance.Play();

            Goofy = new Goofy(10, 660);
            Goofy.IsRemoteControlled = true;
            Goofy.CurrentImage = ContentStore.LoadImage("GoofyRunningBig");
            Goofy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Goofy_WouldCollideWith);

            _Daisy = new Daisy(500, 660);
            _Daisy.CurrentImage = ContentStore.LoadImage("DaisyHappy2");
            //_Daisy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Daisy_WouldCollideWith);                       

            DrawableObjects.Add(new Drawable(0, 0, ContentStore.LoadImage("Church")));
            Add(Goofy);
            Add(_Daisy);
        }

        public override void Update(GameTime gameTime)
        {
            if (Action.IsNotDone("GoofyAtDaisy"))
            {
                Goofy.Move(Direction.Right);
            }
            else if (Action.IsDone("GoofyStanding"))
            {
                Action.Delete("GoofyStanding");
                Goofy.CurrentImage = ContentStore.LoadImage("GoofyStanding");
                Goofy.Bounds = new Rectangle(_Daisy.Bounds.X - Goofy.Bounds.Width, Goofy.Bounds.Y, Goofy.Bounds.Width, Goofy.Bounds.Height);

                

                new Timer(_ => Action.SetDone("MoveCoupleToMiddle")).Change(2000, 0);
            }
            else if (Action.IsDone("MoveCoupleToMiddle"))
            {
                MoveCoupleToMiddle();
            }

            // Update the collidables and drawables
            base.Update(gameTime);
        }

        void _Goofy_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith == _Daisy)
            {
                if (Action.IsNotDone("GoofyAtDaisy"))
                {
                    Action.SetDone("GoofyAtDaisy");
                    Goofy.Move(Direction.None);
                    Goofy.CurrentImage = ContentStore.LoadImage("GoofyHappy");

                    new LoveExplosion(this, Goofy.Bounds.X + 10, Goofy.Bounds.Y + 10);
                    new LoveExplosion(this, _Daisy.Bounds.X - 10, _Daisy.Bounds.Y - 10);

                    ShowGuests();

                    new Timer(_ => Action.SetDone("GoofyStanding")).Change(2000, 0);
                }
            }
        }

        private void ShowGuests()
        {
            var tickTrickTrack = new TickTickTrack(5, 660);
            var dagobert = new Dagobert(0, 660);
            var miniMouse = new Mini(800, 660);
            var mickeyMouse = new Mickey(900, 660);

            Add(dagobert);
            Add(tickTrickTrack);
            Add(miniMouse);
            Add(mickeyMouse);

            tickTrickTrack.MoveToTarget(270, 660, 0.15f);
            dagobert.MoveToTarget(150, 660, 0.07f);
            miniMouse.MoveToTarget(600, 660, 0.1f);
            mickeyMouse.MoveToTarget(700, 660, 0.1f);
        }

        private void MoveCoupleToMiddle()
        {
            if (Action.IsNotDone("MovingCouple"))
            {
                Action.SetDone("MovingCouple");

                SoundStore.Sound("ChurchBell").Instance.Stop();
                var weddingSound = SoundStore.Create("WeddingMarch");                
                weddingSound.Instance.Play();

                // End the scene, when the music stops.
                new Timer(_ => OnEnd(Goofy)).Change(weddingSound.Duration, TimeSpan.Zero);

                Goofy.AllowFall = false;
                Goofy.CanCollide = false;

                _Daisy.AllowFall = false;
                _Daisy.CanCollide = false;

                _GoofyTempSize = new Vector2(Goofy.Bounds.Width, Goofy.Bounds.Height);
                _DaisyTempSize = new Vector2(_Daisy.Bounds.Width, _Daisy.Bounds.Height);

                Goofy.Moved += new EventHandler<MovingEventArgs>(_Goofy_Moved);
                Goofy.MoveToTarget(470, 462, 0.02f);

                _Daisy.Moved += new EventHandler<MovingEventArgs>(_Daisy_Moved);
                _Daisy.MoveToTarget(490, 468, 0.019f);
            }
        }
                
        void _Goofy_Moved(object sender, MovingEventArgs e)
        {
            _GoofyTempSize = Vector2.Multiply(_GoofyTempSize, 0.99725f);

            Goofy.Bounds = new Rectangle((int)e.Position.X, (int)e.Position.Y, (int)_GoofyTempSize.X, (int)_GoofyTempSize.Y);
        }

        void _Daisy_Moved(object sender, MovingEventArgs e)
        {
            _DaisyTempSize = Vector2.Multiply(_DaisyTempSize, 0.9972f);

            _Daisy.Bounds = new Rectangle((int)e.Position.X, (int)e.Position.Y, (int)_DaisyTempSize.X, (int)_DaisyTempSize.Y);
        }
    }
}
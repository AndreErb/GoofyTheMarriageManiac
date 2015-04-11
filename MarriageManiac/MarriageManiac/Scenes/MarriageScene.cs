using System;
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
        Goofy _Goofy = null;
        Daisy _Daisy = null;
        Vector2 _GoofyTempSize;
        Vector2 _DaisyTempSize;

        public MarriageScene()
            : base()
        {
            // Create and play the background music.
            var music = SoundStore.Create("KissMusic", "Kiss_The_Girl");
            music.IsLooped = true;
            music.Play();

            _Goofy = new Goofy(10, 660);
            _Goofy.IsRemoteControlled = true;
            _Goofy.CurrentImage = ContentStore.LoadImage("GoofyRunningBig");
            _Goofy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Goofy_WouldCollideWith);

            _Daisy = new Daisy(500, 660);
            _Daisy.CurrentImage = ContentStore.LoadImage("DaisyHappy2");
            //_Daisy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Daisy_WouldCollideWith);                       

            DrawableObjects.Add(new Drawable(0, 0, ContentStore.LoadImage("Church")));
            Add(_Goofy);
            Add(_Daisy);
        }

        public override void Update(GameTime gameTime)
        {
            if (Action.IsNotDone("GoofyAtDaisy"))
            {
                _Goofy.Move(Direction.Right);
            }
            else if (Action.IsDone("GoofyStanding"))
            {
                Action.Delete("GoofyStanding");
                _Goofy.CurrentImage = ContentStore.LoadImage("GoofyStanding");
                _Goofy.Bounds = new Rectangle(_Daisy.Bounds.X - _Goofy.Bounds.Width, _Goofy.Bounds.Y, _Goofy.Bounds.Width, _Goofy.Bounds.Height);
                
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
                    _Goofy.Move(Direction.None);
                    _Goofy.CurrentImage = ContentStore.LoadImage("GoofyHappy");

                    new Timer(_ => Action.SetDone("GoofyStanding")).Change(2000, 0);
                }
            }
        }

        private void MoveCoupleToMiddle()
        {
            if (Action.IsNotDone("MovingCouple"))
            {
                Action.SetDone("MovingCouple");
                _Goofy.AllowFall = false;
                _Goofy.CanCollide = false;

                _Daisy.AllowFall = false;
                _Daisy.CanCollide = false;

                _GoofyTempSize = new Vector2(_Goofy.Bounds.Width, _Goofy.Bounds.Height);
                _DaisyTempSize = new Vector2(_Daisy.Bounds.Width, _Daisy.Bounds.Height);

                _Goofy.Moved += new EventHandler<MovingEventArgs>(_Goofy_Moved);
                _Goofy.MoveToTarget(472, 465, 0.05f);

                _Daisy.Moved += new EventHandler<MovingEventArgs>(_Daisy_Moved);
                _Daisy.MoveToTarget(490, 468, 0.045f);
            }
        }
                
        void _Goofy_Moved(object sender, MovingEventArgs e)
        {
            _GoofyTempSize = Vector2.Multiply(_GoofyTempSize, 0.9925f);

            _Goofy.Bounds = new Rectangle((int)e.Position.X, (int)e.Position.Y, (int)_GoofyTempSize.X, (int)_GoofyTempSize.Y);
        }

        void _Daisy_Moved(object sender, MovingEventArgs e)
        {
            _DaisyTempSize = Vector2.Multiply(_DaisyTempSize, 0.9925f);

            _Daisy.Bounds = new Rectangle((int)e.Position.X, (int)e.Position.Y, (int)_DaisyTempSize.X, (int)_DaisyTempSize.Y);
        }
    }
}
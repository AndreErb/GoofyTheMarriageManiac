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

namespace MarriageManiac.Scenes
{
    public class PrologScene : Scene
    {
        bool _TurnAround;
        bool _Started = false;
        TimeSpan _Elapsed = new TimeSpan();
        Texture2D _CloudTexture = null;
        Goofy _Goofy = null;
        Daisy _Daisy = null;
        PanzerknackerGroup _PanzerknackerGroup = null;
        Drawable _Sun = null;
        Flash _Flash = null;
        DrawableTextBase _Text = null;
        bool _TextCompleted = false;

        public PrologScene()
            : base()
        {
            // Create and play the background music.
            var music = SoundStore.Create("KissMusic", "Kiss_The_Girl");
            music.IsLooped = true;
            music.Play();
            
            _CloudTexture = GoofyGame.CONTENT.Load<Texture2D>("cloud_PNG13");

            var text = "An einem wunderschönen Frühlingstag:" + Environment.NewLine +
                       "Goofy und Daisy-Dani knutschen rum, " + Environment.NewLine +
                       "träumen von einer unbeschwerten Zukunft" + Environment.NewLine +
                       "und planen Ihre opulente Hochzeit, als plötzlich...";
            _Text = new DelayedText(TimeSpan.FromSeconds(0.06), 300, 300, "Comic", Color.Gold, text, null);
            _Text.Completed += (t, args) => _TextCompleted = true;

            _Sun = new Drawable(700, 40, GoofyGame.CONTENT.Load<Texture2D>("Sun"));

            _Flash = new Flash(0, 0, GoofyGame.CONTENT.Load<Texture2D>("Flash"));
            _Flash.Visible = false;

            _Goofy = new Goofy(10, 660);
            _Goofy.IsRemoteControlled = true;
            _Goofy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Goofy_WouldCollideWith);

            _Daisy = new Daisy(_Goofy.Bounds.Right, 660);
            _Daisy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Daisy_WouldCollideWith);

            _PanzerknackerGroup = new PanzerknackerGroup(850, 660);
            _PanzerknackerGroup.Visible = false;
            _PanzerknackerGroup.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_PanzerknackerGroup_WouldCollideWith);

            DrawableTexts.Add(_Text);
            DrawableObjects.Add(_Sun);
            DrawableObjects.Add(new Cloud(150, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(new Cloud(300, 60, _CloudTexture, 0.7f));
            DrawableObjects.Add(new Cloud(700, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(_Flash);
            DrawableObjects.Add(_Goofy);
            DrawableObjects.Add(_Daisy);
            DrawableObjects.Add(_PanzerknackerGroup);
            CollidableObjects.Add(_Goofy);
            CollidableObjects.Add(_Daisy);
            CollidableObjects.Add(_PanzerknackerGroup);
        }

        void _Goofy_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith == Level.Right)
            {
                //If we have returned to the right wall.
                Remove(_Goofy);

                SoundStore.Sound("HorrorMusic").Stop();
                OnEnd();
            }
        }

        void _Daisy_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith as CollisionObject != null)
            {
                // If we have returned to the right wall.
                Remove(_Daisy);
            }
        }

        void _PanzerknackerGroup_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith as Daisy != null)
            {
                _TurnAround = true;
            }
            else if (e.WouldCollideWith as CollisionObject != null)
            {
                // If we have returned to the right wall.
                Remove(_PanzerknackerGroup);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_TextCompleted)
            {
                _Elapsed = _Elapsed.Add(gameTime.ElapsedGameTime);
            }

            if (_Elapsed >= TimeSpan.FromSeconds(1.5) && !_Started && Action.IsNotDone("PanzerknackerAttack"))
            {
                Action.SetDone("PanzerknackerAttack");
                SoundStore.Sound("KissMusic").Stop();
                var horrorMusic = SoundStore.Create("HorrorMusic", "Haunted_Mansion");
                horrorMusic.IsLooped = true;
                horrorMusic.Pitch = 0.5f;
                horrorMusic.Play();

                Remove(_Text);
                Remove(_Sun);
                _PanzerknackerGroup.Visible = true;
                _PanzerknackerGroup.Move(Direction.Left);
                BackColor = Color.Gray;
                _Flash.Start();
            }

            if (_PanzerknackerGroup.Direction != Direction.None)
            {
                if (_TurnAround)
                {
                    if (Action.IsNotDone("Screamed"))
                    {
                        Action.SetDone("Screamed");
                        _Daisy.Scream();
                    }

                    _Daisy.Bounds = new Rectangle(_PanzerknackerGroup.Bounds.Right + 2, _Daisy.Bounds.Y, _Daisy.Bounds.Width, _Daisy.Bounds.Height);
                    _Daisy.Move(Direction.Right);
                    _PanzerknackerGroup.Move(Direction.Right);
                }
            }

            if (_Daisy.Position.X > 450)
            {
                _Goofy.Jump(JumpMode.Jump);
                _Goofy.Move(Direction.Right);
            }

            // Update the collidables and drawables
            base.Update(gameTime);
        }
    }
}
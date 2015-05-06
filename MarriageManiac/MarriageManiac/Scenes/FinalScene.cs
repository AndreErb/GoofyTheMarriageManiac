using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using MarriageManiac.Core.Rectangles;
using MarriageManiac.Texts;

namespace MarriageManiac
{
    /// <summary>
    /// Test2 für GitHub. für "TestBranch".
    /// </summary>
    public class FinalScene : Scene
    {
        Goofy _Goofy = null;
        Texture2D _CloudTexture = null;
        private bool _LevelSymbolShown = false;
        TimeSpan _LevelSymbolShowTime = new TimeSpan();
        DrawableMovable _LevelSymbol = null;
        Ufo _Ufo = null;
        FillableRectangle _GoofyLifeBar;
        FillableRectangle _UfoLifeBar;        
        const int _LifeBarWidth = 400;
        Text _LifeText;
        Flash _Flash;
        
        public FinalScene() : base()
        {}

        public override void Load(Goofy goofy)
        {
            var horrorMusic = SoundStore.Create("HorrorMusic", "Haunted_Mansion");
            horrorMusic.Instance.IsLooped = true;
            horrorMusic.Instance.Pitch = 0.5f;
            horrorMusic.Instance.Play();

            _CloudTexture = ContentStore.LoadImage("cloud_PNG13");

            _LevelSymbol = new DrawableMovable(-100, -100, ContentStore.LoadImage("FinalLevel"));
            _LevelSymbol.TargetReached += (obj, arg) => { _LevelSymbolShown = true; _LevelSymbol.ResetRotation(); };
            _LevelSymbol.MoveToTarget(350, 300, 2f);
            _LevelSymbol.SetOrigin(Drawable.OriginPoint.Center); // Rotation around the center.
            _LevelSymbol.RotateContiniously(0.1f);

            _Goofy = new Goofy(340, 660) { Lifes = goofy.Lifes };
            _Goofy.LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(_Goofy_LifeAmountChanged);

            _Ufo = new Ufo(400, 50);
            _Ufo.Visible = false;
            _Ufo.LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(_Ufo_LifeAmountChanged);

            var screenMiddle = GoofyGame.SCREENWIDTH / 2;
            var distanceFromMiddle = 10;

            var goofyIcon = new GoofyIcon(0, 0);
            goofyIcon.Position = new Vector2(screenMiddle - distanceFromMiddle - goofyIcon.Bounds.Width, 10);

            var ufoIcon = new UfoIcon(0, 10);
            ufoIcon.Position = new Vector2(screenMiddle + distanceFromMiddle, 10);

            _GoofyLifeBar = new FillableRectangle((int)goofyIcon.Position.X - 1 - _LifeBarWidth, 20, _LifeBarWidth, 25, 1, Color.Yellow, Color.Black);
            _UfoLifeBar = new FillableRectangle((int)ufoIcon.Position.X + ufoIcon.Bounds.Width + 1, 20, _LifeBarWidth, 25, 1, Color.Yellow, Color.Black);

            _LifeText = new Text((int)goofyIcon.Position.X + 5, goofyIcon.Bounds.Bottom + 4, "Comic", Color.Gold, " X " + _Goofy.Lifes, null);

            _Flash = new Flash(0, 0, ContentStore.LoadImage("Flash"));

            BackColor = Color.Gray;
            _Flash.Start();

            DrawableObjects.Add(_Flash);
            Add(_Ufo);
            Add(_Goofy);
            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(_GoofyLifeBar);
            DrawableObjects.Add(_LifeText);
            DrawableObjects.Add(_UfoLifeBar);
            DrawableObjects.Add(ufoIcon);
            DrawableObjects.Add(goofyIcon);
            DrawableObjects.Add(_LevelSymbol);
        }
                        
        public override void Update(GameTime gameTime)
        {
            if (_LevelSymbolShown)
            {
                _LevelSymbolShowTime += gameTime.ElapsedGameTime;

                if (_LevelSymbolShowTime > TimeSpan.FromSeconds(1.5) && !_Ufo.Started)
                {
                    DrawableObjects.Remove(_LevelSymbol);
                    _Ufo.Visible = true;
                    _Ufo.Started = true;
                    _Ufo.Shooting = true;
                }
            }

            if (Action.IsDone("GoofyHasDied"))
            {
                if (_Goofy.Lifes == 0)
                {
                    GameOver();
                }
                else
                {
                    if (Action.Value<TimeSpan>("GoofyHasDied") == TimeSpan.Zero)
                    {
                        Action.SetDone("GoofyHasDied", gameTime.TotalGameTime);
                    }
                    else if (gameTime.TotalGameTime - Action.Value<TimeSpan>("GoofyHasDied") > TimeSpan.FromSeconds(2))
                    {
                        Action.Delete("GoofyHasDied");

                        if (_Goofy.Lifes > 0)
                        {
                            _Goofy.ReviveAt(340, 660);
                            _Ufo.Shooting = true;
                        }
                    }
                }
            }

            // Update the collidables and drawables
            base.Update(gameTime);
        }
        
        void _Goofy_LifeAmountChanged(object sender, LifeAmountChangedArgs e)
        {
            _GoofyLifeBar.FillPercentage = e.CurrentLifePercentage;

            if (e.CurrentLifePercentage <= 0 && Action.IsNotDone("GoofyHasDied"))
            {
                Action.SetDone("GoofyHasDied");

                _Ufo.Shooting = false;

                _Goofy.Die();
                _LifeText.Text = " X " + _Goofy.Lifes;
            }
        }

        void _Ufo_LifeAmountChanged(object sender, LifeAmountChangedArgs e)
        {
            _UfoLifeBar.FillPercentage = e.CurrentLifePercentage;

            if (e.CurrentLifePercentage <= 0)
            {
                SoundStore.Sound("HorrorMusic").Instance.Stop();
                _Goofy.Laugh();
                Remove(_Flash);
                BackColor = Color.CornflowerBlue;
                _Goofy.IsRemoteControlled = true;

                _Goofy.WouldCollideWith += (obj, ev) =>
                {
                    if (ev.WouldCollideWith == Level.Right)
                    {
                        OnEnd(_Goofy);
                    }
                };

                _Goofy.Move(Direction.Right);
            }
        }
    }
}
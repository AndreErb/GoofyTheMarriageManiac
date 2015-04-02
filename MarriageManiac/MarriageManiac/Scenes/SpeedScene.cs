﻿using System;
using MarriageManiac.Texts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using MarriageManiac.Core.Rectangles;

namespace MarriageManiac.Scenes
{
    public class SpeedScene : Scene
    {
        Goofy _Goofy = null;
        Texture2D _CloudTexture = null;
        private bool _LevelSymbolShown = false;
        TimeSpan _LevelSymbolShowTime = new TimeSpan();
        DrawableMovable _LevelSymbol = null;
        Text _Explanation = null;
        FillableRectangle _GoofyLifeBar;
        const int _LifeBarWidth = 400;
        Text _LifeText;
        Counter _Counter;
        const double COUNTER_SECONDS = 45;

        public SpeedScene()
            : base()
        {
            var music = SoundStore.Create("PacmanMusic");
            music.Pitch = -0.4f;
            music.Volume = 0.4f;
            music.IsLooped = true;
            music.Play();

            _CloudTexture = ContentStore.LoadImage("cloud_PNG13");

            //_Goofy = new Goofy(10, 660);
            _Goofy = new Goofy(750, 60);
            _Goofy.Lifes = 2;
            _Goofy.LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(_Goofy_LifeAmountChanged);

            _LevelSymbol = new DrawableMovable(-100, -100, ContentStore.LoadImage("Level1"));
            _LevelSymbol.TargetReached += (obj, arg) => { _LevelSymbolShown = true; _LevelSymbol.ResetRotation(); };
            _LevelSymbol.MoveToTarget(350, 300, 2f);
            _LevelSymbol.SetOrigin(Drawable.OriginPoint.Center); // Rotation around the center.
            _LevelSymbol.RotateContiniously(0.1f);

            var screenMiddle = GoofyGame.SCREENWIDTH / 2;
            var distanceFromMiddle = 10;

            var goofyIcon = new GoofyIcon(0, 0);
            goofyIcon.Position = new Vector2(screenMiddle - distanceFromMiddle - goofyIcon.Bounds.Width, 10);
            _GoofyLifeBar = new FillableRectangle((int)goofyIcon.Position.X - 1 - _LifeBarWidth, 20, _LifeBarWidth, 25, 1, Color.Yellow, Color.Black);
            _LifeText = new Text((int)goofyIcon.Position.X + 5, goofyIcon.Bounds.Bottom + 4, "Comic", Color.Gold, " X " + _Goofy.Lifes, null);

            var countDown = TimeSpan.FromSeconds(COUNTER_SECONDS);
            _Counter = new Counter(countDown, _GoofyLifeBar.Bounds.X + 180, _GoofyLifeBar.Bounds.Y, Color.Gold);
            _Counter.TimeChanged += new EventHandler<TimeChangedEventArgs>(Counter_TimeChanged);

            var gate = new WoodenGate(860, 25);

            DrawableObjects.Add(gate);
            DrawableObjects.Add(_LevelSymbol);
            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(_Goofy);
            DrawableObjects.Add(goofyIcon);
            DrawableObjects.Add(_GoofyLifeBar);
            DrawableObjects.Add(_Counter);
            DrawableObjects.Add(_LifeText);
            CollidableObjects.Add(_Goofy);
            CollidableObjects.Add(gate);
        }
      
        public override void Update(GameTime gameTime)
        {
            if (_LevelSymbolShown)
            {
                _LevelSymbolShowTime += gameTime.ElapsedGameTime;

                if (_LevelSymbolShowTime > TimeSpan.FromSeconds(1.5))
                {
                    DrawableObjects.Remove(_LevelSymbol);
                    ShowExplanation();
                }
            }

            // Update the collidables and drawables
            base.Update(gameTime);
        }

        private void ShowExplanation()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Enter) && Action.IsNotDone("ExplanationClosed"))
            {
                Remove(_Explanation);
                Action.SetDone("ExplanationClosed");
                _Counter.CountDown();
            }

            if (Action.IsNotDone("ExplanationShown"))
            {
                var text = "Goofy braucht deine Hilfe um Daisy-Dani zu befreien." + Environment.NewLine + Environment.NewLine
                         + "Steurerung:" + Environment.NewLine
                         + "Links: <    Rechts: >    Springen: Leertaste" + Environment.NewLine + Environment.NewLine
                         + "Drücke ENTER um fortzufahren.";
                _Explanation = new Text(230, 180, "Comic", Color.Black, text, "Schriftrolle");

                DrawableObjects.Add(_Explanation);
                Action.SetDone("ExplanationShown");
            }
        }

        void Counter_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            var restLife = (e.CurrentTime.TotalSeconds / COUNTER_SECONDS) * 100;
            _Goofy.LifePercentage = (decimal)restLife;
        }

        void _Goofy_LifeAmountChanged(object sender, LifeAmountChangedArgs e)
        {
            _GoofyLifeBar.FillPercentage = e.CurrentLifePercentage;

            if (e.CurrentLifePercentage <= 0 && Action.IsNotDone("GoofyHasDied"))
            {
                Action.SetDone("GoofyHasDied");

                _Goofy.IsRemoteControlled = true;
                _Goofy.Die();
                _LifeText.Text = " X " + _Goofy.Lifes;
            }
        }
    }
}
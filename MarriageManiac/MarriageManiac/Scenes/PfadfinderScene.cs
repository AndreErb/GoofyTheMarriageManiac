using System;
using System.Linq;
using MarriageManiac.Texts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using MarriageManiac.Core.Rectangles;
using System.Collections.Generic;
using System.Threading;

namespace MarriageManiac.Scenes
{
    public class PfadfinderScene : Scene
    {
        Snoopy _Snoopy = null;
        Texture2D _CloudTexture = null;
        private bool _LevelSymbolShown = false;
        TimeSpan _LevelSymbolShowTime = new TimeSpan();
        DrawableMovable _LevelSymbol = null;
        FillableRectangle _GoofyLifeBar;
        const int _LifeBarWidth = 400;
        Text _LifeText;
        private Text _Question;
        Sound _RightSound;
        Sound _WrongSound;
        private List<Keys> _RightKeys = new List<Keys>();
        private List<Keys> _ValidKeys = new List<Keys>() { Keys.A, Keys.B, Keys.C };

        public PfadfinderScene()
            : base()
        { }

        public override void Load(Goofy goofy)
        {
            _RightKeys.Add(Keys.A);
            _RightKeys.Add(Keys.C);
            _RightKeys.Add(Keys.C);
            _RightKeys.Add(Keys.B);
            _RightKeys.Add(Keys.A);

            var fire = SoundStore.Create("fireplace");
            fire.Instance.IsLooped = true;
            fire.Instance.Play();

            _RightSound = SoundStore.Create("rightanswer");
            _WrongSound = SoundStore.Create("wronganswer");
            _CloudTexture = ContentStore.LoadImage("cloud_PNG13");

            Goofy = new Goofy(600, 0) { Lifes = goofy.Lifes };
            Goofy.LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(_Goofy_LifeAmountChanged);
            Goofy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Goofy_WouldCollideWith);

            _Snoopy = new Snoopy(500, 100, false);
            _Snoopy.ActualQuestion = 0;

            _LevelSymbol = new DrawableMovable(-100, -100, ContentStore.LoadImage("Level4"));
            _LevelSymbol.TargetReached += (obj, arg) => { _LevelSymbolShown = true; _LevelSymbol.ResetRotation(); };
            _LevelSymbol.MoveToTarget(350, 300, 2f);
            _LevelSymbol.SetOrigin(Drawable.OriginPoint.Center); // Rotation around the center.
            _LevelSymbol.RotateContiniously(0.1f);

            var screenMiddle = GoofyGame.SCREENWIDTH / 2;
            var distanceFromMiddle = 10;

            var goofyIcon = new GoofyIcon(0, 0);
            goofyIcon.Position = new Vector2(screenMiddle - distanceFromMiddle - goofyIcon.Bounds.Width, 10);
            _GoofyLifeBar = new FillableRectangle((int)goofyIcon.Position.X - 1 - _LifeBarWidth, 20, _LifeBarWidth, 25, 1, Color.Yellow, Color.Black);
            _LifeText = new Text((int)goofyIcon.Position.X + 5, goofyIcon.Bounds.Bottom + 4, "Comic", Color.Gold, " X " + Goofy.Lifes, null);

            var gate = new WoodenGate(1, 600);
            Add(gate);

            CollidableObjects.Add(Goofy);
            CollidableObjects.Add(_Snoopy);
            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.3f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.5f));
            DrawableObjects.Add(Goofy);
            DrawableObjects.Add(_Snoopy);
            DrawableObjects.Add(goofyIcon);
            DrawableObjects.Add(_GoofyLifeBar);
            DrawableObjects.Add(_LifeText);
            DrawableObjects.Add(_LevelSymbol);
        }

        private void _Goofy_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith as Snoopy != null)
            {
                ShowQuestion();

            }
            else if (e.WouldCollideWith is WoodenGate)
            {
                if(_Snoopy.ActualQuestion<5)
                {
                    Goofy.LifePercentage = 0;
                }
                else
                {
                    OnEnd(Goofy);
                }

                
            }
        }

        private void ShowQuestion()
        {
            if (Action.IsNotDone("QuestionShown"))
            {
                string text = "";
                if (_Snoopy.ActualQuestion == 0)
                {
                    text = "Wo gibt es keine Pfadfinder?" + Environment.NewLine + Environment.NewLine
                             + "a) Nordkorea " + Environment.NewLine
                             + "b) Thailand " + Environment.NewLine
                             + "c) Japan" + Environment.NewLine + Environment.NewLine
                             + "Drücke den entsprechenden Buchstaben auf der Tastatur!" + Environment.NewLine + Environment.NewLine
                             + "Hinweis: Pfadfinder sind neugierig!";
                }
                else if (_Snoopy.ActualQuestion == 1)
                {
                    text = "Wer ist kein Pfadfinder?" + Environment.NewLine + Environment.NewLine
                            + "a) Thomas Gottschalk " + Environment.NewLine
                            + "b) Harald Schmidt " + Environment.NewLine
                            + "c) Oliver Pocher " + Environment.NewLine + Environment.NewLine
                            + "Drücke den entsprechenden Buchstaben auf der Tastatur!";
                }
                else if (_Snoopy.ActualQuestion == 2)
                {
                    text = "In welchem Land gibt es mit 17 Millionen die Meisten PfadfinderInnen?" + Environment.NewLine + Environment.NewLine
                            + "a) Indien " + Environment.NewLine
                            + "b) USA " + Environment.NewLine
                            + "c) Indonesien " + Environment.NewLine + Environment.NewLine
                            + "Drücke den entsprechenden Buchstaben auf der Tastatur!";
                }
                else if (_Snoopy.ActualQuestion == 3)
                {
                    text = "Wie lange braucht die Erde um sich um 15 Grad zu drehen?" + Environment.NewLine + Environment.NewLine
                            + "a) 1 Stunde und 3 Minuten " + Environment.NewLine
                            + "b) 1 Stunde " + Environment.NewLine
                            + "c) 3 Stunden " + Environment.NewLine + Environment.NewLine
                            + "Drücke den entsprechenden Buchstaben auf der Tastatur!";
                }
                else if (_Snoopy.ActualQuestion == 4)
                {
                    text = "Zu welchem Sternbild gehört der Polarstern?" + Environment.NewLine + Environment.NewLine
                            + "a) Kleiner Wagen " + Environment.NewLine
                            + "b) Großer Hunde " + Environment.NewLine
                            + "c) Großer Wagen " + Environment.NewLine + Environment.NewLine
                            + "Drücke den entsprechenden Buchstaben auf der Tastatur!";
                }

                _Question = new Text(230, 180, "Comic", Color.Black, text, "Schriftrolle");

                DrawableObjects.Add(_Question);
                Action.SetDone("QuestionShown");
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_LevelSymbolShown)
            {
                _LevelSymbolShowTime += gameTime.ElapsedGameTime;

                if (_LevelSymbolShowTime > TimeSpan.FromSeconds(1.5))
                {
                    DrawableObjects.Remove(_LevelSymbol);
                }
            }

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.GetPressedKeys().Intersect(_ValidKeys).Any() && Action.IsDone("QuestionShown"))
            {
                if (keyboard.IsKeyDown(_RightKeys.ElementAt(_Snoopy.ActualQuestion)))
                {
                    _RightSound.Instance.Play();
                    CollidableObjects.RemoveAll(s => s is Sum);
                    DrawableObjects.RemoveAll(s => s is Sum);
                }
                else
                {
                    Goofy.LifePercentage -= 40;
                    _WrongSound.Instance.Play();
                }

                if (_Snoopy.ActualQuestion == 0)
                {
                    _Snoopy.Move(Direction.Left);
                    _Snoopy.ViewDirection = Direction.Right;
                }
                else if (_Snoopy.ActualQuestion == 1)
                {
                    _Snoopy.MoveToTarget(_Snoopy.Position.X + 200, _Snoopy.Position.Y, 0.2f);
                }
                else if (_Snoopy.ActualQuestion == 2)
                {
                    _Snoopy.MoveToTarget(_Snoopy.Position.X + 300, _Snoopy.Position.Y, 0.2f);
                }
                else if (_Snoopy.ActualQuestion == 3)
                {
                    _Snoopy.Move(Direction.Right);
                    _Snoopy.ViewDirection = Direction.Left;
                }

                _Snoopy.ActualQuestion++;

                if (_Snoopy.ActualQuestion == 5)
                {
                    Remove(_Snoopy);
                }

                Remove(_Question);
                Action.Delete("QuestionShown");


            }

            // Update the collidables and drawables
            base.Update(gameTime);
        }

        private void LetGoofyDieAndRevive()
        {
            Action.SetDone("GoofyHasDied");

            Goofy.IsRemoteControlled = true;
            Goofy.Die();
            _LifeText.Text = " X " + Goofy.Lifes;

            Timer timer = null;

            TimerCallback reviveGoofy = _ =>
            {
                Goofy.ReviveAt(600, 0);
                Goofy.IsRemoteControlled = false;
                timer.Dispose();

                Action.Delete("GoofyHasDied");
            };

            // Revive Goofy after 1,5 secs
            timer = new Timer(reviveGoofy);
            timer.Change(1500, 0);
        }

        void _Goofy_LifeAmountChanged(object sender, LifeAmountChangedArgs e)
        {
            _GoofyLifeBar.FillPercentage = e.CurrentLifePercentage;

            if (e.CurrentLifePercentage <= 0 && Action.IsNotDone("GoofyHasDied"))
            {
                if (Goofy.Lifes > 0)
                {
                    LetGoofyDieAndRevive();
                }
                else
                {
                    GameOver();
                }
            }
        }
    }
}
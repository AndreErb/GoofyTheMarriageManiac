using System;
using MarriageManiac.Texts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using MarriageManiac.Core.Rectangles;
using System.Threading;


//hallo andre
namespace MarriageManiac.Scenes
{
    public class GarloffScene : Scene
    {
        Garloff _Garloff = null;
        Goofy _Goofy = null;
        Texture2D _CloudTexture = null;
        private bool _LevelSymbolShown = false;
        TimeSpan _LevelSymbolShowTime = new TimeSpan();
        DrawableMovable _LevelSymbol = null;
        FillableRectangle _GoofyLifeBar;
        const int _LifeBarWidth = 400;
        Text _LifeText;
        Text _Question = null;
        bool _RightAnswer;
        Sound _RightSound;
        Sound _WrongSound;
        Text _Answer;

        public GarloffScene()
            : base()
        { }

        public override void Load()
        {
            var spaceMusic = SoundStore.Create("space");
            spaceMusic.Instance.IsLooped = true;
            spaceMusic.Instance.Play();

            _RightSound = SoundStore.Create("rightanswer");
            _WrongSound = SoundStore.Create("wronganswer");

            _CloudTexture = ContentStore.LoadImage("cloud_PNG13");

            _Goofy = new Goofy(10, 660);
            _Goofy.Lifes = 2;
            _Goofy.LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(_Goofy_LifeAmountChanged);
            _Goofy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Goofy_WouldCollideWith);

            _Garloff = new Garloff(530, 400);

            _LevelSymbol = new DrawableMovable(-100, -100, ContentStore.LoadImage("Level3"));
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

            var gate = new PiGate(800, 585);
            Add(gate);

            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(_Goofy);
            DrawableObjects.Add(goofyIcon);
            DrawableObjects.Add(_GoofyLifeBar);
            DrawableObjects.Add(_LifeText);
            DrawableObjects.Add(_Garloff);
            CollidableObjects.Add(_Goofy);
            CollidableObjects.Add(_Garloff);
            DrawableObjects.Add(_LevelSymbol);
        }

        void _Goofy_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith as Garloff != null)
            {
                ShowQuestion();
            }
            else if (e.WouldCollideWith is PiGate)
            {
                OnEnd();
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

            if (keyboard.IsKeyDown(Keys.Enter) && Action.IsDone("AnswerShown"))
            {
                Remove(_Answer);
            }

            if (_RightAnswer && keyboard.IsKeyDown(Keys.Enter))
            {
                Remove(_Answer);
                Remove(_Garloff);
            }

            if ((keyboard.IsKeyDown(Keys.NumPad1) || keyboard.IsKeyDown(Keys.D1) ||
                 keyboard.IsKeyDown(Keys.NumPad2) || keyboard.IsKeyDown(Keys.D2) ||
                 keyboard.IsKeyDown(Keys.NumPad3) || keyboard.IsKeyDown(Keys.D3) ||
                 keyboard.IsKeyDown(Keys.NumPad4) || keyboard.IsKeyDown(Keys.D4) ||
                 keyboard.IsKeyDown(Keys.NumPad5) || keyboard.IsKeyDown(Keys.D5) ||
                 keyboard.IsKeyDown(Keys.NumPad6) || keyboard.IsKeyDown(Keys.D6) ||
                 keyboard.IsKeyDown(Keys.NumPad7) || keyboard.IsKeyDown(Keys.D7) ||
                 keyboard.IsKeyDown(Keys.NumPad8) || keyboard.IsKeyDown(Keys.D8))
                 && Action.IsNotDone("AnswerShown"))
            {
                Remove(_Question);
                Action.Delete("QuestionShown");

                _Answer = new Text(230, 400, "Comic", Color.Black,
                    "Das ist leider falsch !!! " + Environment.NewLine + "Drücke Enter zum fortfahren!",
                    "Schriftrolle");
                DrawableObjects.Add(_Answer);
                Action.SetDone("AnswerShown");
                _Goofy.LifePercentage -= 30;
                _WrongSound.Instance.Play();
               
            }

            if ((keyboard.IsKeyDown(Keys.NumPad9) || keyboard.IsKeyDown(Keys.D9)) && Action.IsNotDone("AnswerShown"))
            {
                Remove(_Question);
                _Answer = new Text(230, 400, "Comic", Color.Black,
                                        "Da ist ja doch was hängen geblieben!" + Environment.NewLine +
                                        "Gauss, Euler und Co mögen dir beistehen!" + Environment.NewLine +
                                        "Schreite nun durch Pi, das wird dich weiterbringen" + Environment.NewLine + Environment.NewLine +
                                        "Drücke Enter zum fortfahren!",
                                        "Schriftrolle");
                DrawableObjects.Add(_Answer);
                Action.SetDone("AnswerShown");
                _RightAnswer = true;
                _RightSound.Instance.Play();
            }


            // Update the collidables and drawables
            base.Update(gameTime);
        }

        private void ShowQuestion()
        {
            if (Action.IsNotDone("QuestionShown"))
            {
                var text = "Wie lautet die 5. Nachkommstelle von Pi?" + Environment.NewLine + Environment.NewLine
                           + "Drücke die entsprechende Ziffer!";

                _Question = new Text(230, 180, "Comic", Color.Black, text, "Schriftrolle");

                DrawableObjects.Add(_Question);
                Action.SetDone("QuestionShown");
                Action.Delete("AnswerShown");
            }
        }

        void _Goofy_LifeAmountChanged(object sender, LifeAmountChangedArgs e)
        {
            _GoofyLifeBar.FillPercentage = e.CurrentLifePercentage;

            if (e.CurrentLifePercentage <= 0 && Action.IsNotDone("GoofyHasDied"))
            {
                if (_Goofy.Lifes > 0)
                {
                    LetGoofyDieAndRevive();
                }
                else
                {
                    GameOver();
                }
            }
        }

        private void LetGoofyDieAndRevive()
        {
            Action.SetDone("GoofyHasDied");

            _Goofy.IsRemoteControlled = true;
            _Goofy.Die();
            _LifeText.Text = " X " + _Goofy.Lifes;

            Timer timer = null;

            TimerCallback reviveGoofy = _ =>
            {
                _Goofy.ReviveAt(10, 660);
                _Goofy.IsRemoteControlled = false;
                timer.Dispose();

                Action.Delete("GoofyHasDied");
            };

            // Revive Goofy after 1,5 secs
            timer = new Timer(reviveGoofy);
            timer.Change(1500, 0);
        }

    }
}
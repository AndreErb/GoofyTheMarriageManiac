using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Texts;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using MarriageManiac.Core.Rectangles;
using System.Threading;

//hallo
namespace MarriageManiac.Scenes
{
    public class SchmidScene : Scene
    {
        Goofy _Goofy = null;
        Schmid _Schmid = null;
        Texture2D _CloudTexture = null;
        private bool _LevelSymbolShown = false;
        TimeSpan _LevelSymbolShowTime = new TimeSpan();
        DrawableMovable _LevelSymbol = null;
        DrawableMovable _Diagram = null;
        Text _Question = null;
        Text _Answer = null;
        bool _RightAnswer = false;
        FillableRectangle _GoofyLifeBar;
        const int _LifeBarWidth = 400;
        Text _LifeText;
        Sound _RightSound;
        Sound _WrongSound;

        public SchmidScene() : base()
        {}

        public override void Load()
        {
            var spaceMusic = SoundStore.Create("meer_rauschen");
            spaceMusic.Instance.IsLooped = true;
            spaceMusic.Instance.Play();

            _RightSound = SoundStore.Create("rightanswer");
            _WrongSound = SoundStore.Create("wronganswer");
            _CloudTexture = ContentStore.LoadImage("cloud_PNG13");

            _Goofy = new Goofy(10, 660);
            _Goofy.Lifes = 2;
            _Goofy.LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(_Goofy_LifeAmountChanged);
            _Goofy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Goofy_WouldCollideWith);
            _Schmid = new Schmid(200, 0);

            _LevelSymbol = new DrawableMovable(-100, -100, ContentStore.LoadImage("Level2"));
            _LevelSymbol.TargetReached += (obj, arg) => { _LevelSymbolShown = true; _LevelSymbol.ResetRotation(); };
            _LevelSymbol.MoveToTarget(350, 300, 2f);
            _LevelSymbol.SetOrigin(Drawable.OriginPoint.Center); // Rotation around the center.
            _LevelSymbol.RotateContiniously(0.1f);

            _Diagram = new DrawableMovable(100, 100, ContentStore.LoadImage("klassendiag"));
            _Diagram.RotateContiniously(0.01f);
            _Diagram.TargetReached += (obj, arg) => { _Diagram.Position = new Vector2(0, 0); };
            _Diagram.MoveToTarget(1000, 700, 0.1f);

            var screenMiddle = GoofyGame.SCREENWIDTH / 2;
            var distanceFromMiddle = 10;

            var goofyIcon = new GoofyIcon(0, 0);
            goofyIcon.Position = new Vector2(screenMiddle - distanceFromMiddle - goofyIcon.Bounds.Width, 10);
            _GoofyLifeBar = new FillableRectangle((int)goofyIcon.Position.X - 1 - _LifeBarWidth, 20, _LifeBarWidth, 25, 1, Color.Yellow, Color.Black);
            _LifeText = new Text((int)goofyIcon.Position.X + 5, goofyIcon.Bounds.Bottom + 4, "Comic", Color.Gold, " X " + _Goofy.Lifes, null);

            var gate = new WoodenGate(860, 165);
            Add(gate);

            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(_Diagram);
            DrawableObjects.Add(_Goofy);
            DrawableObjects.Add(_Schmid);
            DrawableObjects.Add(goofyIcon);
            DrawableObjects.Add(_GoofyLifeBar);
            DrawableObjects.Add(_LifeText);
            CollidableObjects.Add(_Goofy);
            CollidableObjects.Add(_Schmid);
            DrawableObjects.Add(_LevelSymbol);
        }

        void _Goofy_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith as Schmid != null)
            {
                ShowQuestion();
            }
            else if (e.WouldCollideWith is WoodenGate)
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

            if (_RightAnswer && keyboard.IsKeyDown(Keys.Enter))
            {
                Remove(_Answer);

                Texture2D texture = ContentStore.LoadImage("Brick_Block");

                var co1 = new CollisionObject(new Rectangle(300, 520, 20, 20)) { CurrentImage = texture };
                var co2 = new CollisionObject(new Rectangle(400, 480, 20, 20)) { CurrentImage = texture };
                DrawableObjects.Add(co1);
                DrawableObjects.Add(co2);
                CollidableObjects.Add(co1);
                CollidableObjects.Add(co2);
                Add(new Explosion(300, 520));
                Add(new Explosion(400, 480));

            }

            if (keyboard.IsKeyDown(Keys.Enter) && Action.IsDone("AnswerShown"))
            {
                Remove(_Answer);
            }

            if (keyboard.IsKeyDown(Keys.A) && Action.IsNotDone("AnswerShown"))
            {
                Remove(_Question);
                _Answer = new Text(230, 400, "Comic", Color.Black,
                                        "So sieht's aus, weiter gehts..." + Environment.NewLine + "Drücke Enter zum fortfahren!",
                                        "Schriftrolle");
                DrawableObjects.Add(_Answer);
                Action.SetDone("AnswerShown");
                _RightAnswer = true;
                _RightSound.Instance.Play();
                SoundStore.Create("frage2").Instance.Play();
            }

            if ((keyboard.IsKeyDown(Keys.B) || keyboard.IsKeyDown(Keys.C) || keyboard.IsKeyDown(Keys.D)) && Action.IsNotDone("AnswerShown"))
            {
                Remove(_Question);
                Action.Delete("QuestionShown");

                _Answer = new Text(230, 400, "Comic", Color.Black,
                    "Das ist leider falsch !!! " + Environment.NewLine + "Drücke Enter zum fortfahren!",
                    "Schriftrolle");
                DrawableObjects.Add(_Answer);
                Action.SetDone("AnswerShown");
                _Goofy.LifePercentage = 0;
                _WrongSound.Instance.Play();
            }


            // Update the collidables and drawables
            base.Update(gameTime);
        }

        private void ShowQuestion()
        {
            if (Action.IsNotDone("QuestionShown"))
            {
                var text = "Für was steht HTWG Konstanz?" + Environment.NewLine + Environment.NewLine
                         + "a) Hochschule für Technik, Wirtschaft und Gestaltung " + Environment.NewLine
                         + "b) Hochschule für Technologie, Wissenschaft und Grafik " + Environment.NewLine
                         + "c) Hochschule für Technologie, Wirtschaft und Geisteswissenschaften " + Environment.NewLine
                         + "d) Hangout für Tölpel, Wichte und Gesindel " + Environment.NewLine + Environment.NewLine

                         + "Drücke den entsprechenden Buchstaben auf der Tastatur!";
                _Question = new Text(230, 180, "Comic", Color.Black, text, "Schriftrolle");

                DrawableObjects.Add(_Question);
                Action.SetDone("QuestionShown");
                Action.Delete("AnswerShown");
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
    }
}
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

namespace MarriageManiac.Scenes
{
    public class Level2Scene : Scene
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
        bool _QuestionShown = false;
        bool _AnswerShown = false;
        bool _RightAnswer = false;

        public Level2Scene()
            : base()
        {
            _CloudTexture = GoofyGame.CONTENT.Load<Texture2D>("cloud_PNG13");

            _Goofy = new Goofy(10, 660);
            _Goofy.WouldCollideWith += new EventHandler<WouldCollideEventArgs>(_Goofy_WouldCollideWith);
            _Schmid = new Schmid(200, 1);

            _LevelSymbol = new DrawableMovable(-100, -100, GoofyGame.CONTENT.Load<Texture2D>("Level1"));
            _LevelSymbol.TargetReached += (obj, arg) => { _LevelSymbolShown = true; _LevelSymbol.ResetRotation(); };
            _LevelSymbol.MoveToTarget(350, 300, 2f);
            _LevelSymbol.SetOrigin(Drawable.OriginPoint.Center); // Rotation around the center.
            _LevelSymbol.RotateContiniously(0.1f);

            _Diagram = new DrawableMovable(100, 100, GoofyGame.CONTENT.Load<Texture2D>("klassendiag"));
            _Diagram.RotateContiniously(0.01f);
            _Diagram.TargetReached += (obj, arg) => { _Diagram.Position = new Vector2(0, 0); };
            _Diagram.MoveToTarget(1000, 700, 0.1f);

            DrawableObjects.Add(_LevelSymbol);
            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(_Diagram);
            DrawableObjects.Add(_Goofy);
            DrawableObjects.Add(_Schmid);
            CollidableObjects.Add(_Goofy);
            CollidableObjects.Add(_Schmid);
        }

        void _Goofy_WouldCollideWith(object sender, WouldCollideEventArgs e)
        {
            if (e.WouldCollideWith as Schmid != null)
            {
                ShowQuestion();
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

                Texture2D texture = GoofyGame.CONTENT.Load<Texture2D>("Brick_Block");

                var co1 = new CollisionObject(new Rectangle(300, 520, 20, 20)) { CurrentImage = texture };
                var co2 = new CollisionObject(new Rectangle(400, 480, 20, 20)) { CurrentImage = texture };
                DrawableObjects.Add(co1);
                DrawableObjects.Add(co2);
                CollidableObjects.Add(co1);
                CollidableObjects.Add(co2);

            }

            if (!_RightAnswer && keyboard.IsKeyDown(Keys.Enter))
            {
                Remove(_Answer);
                // -1 Leben
            }

            if (keyboard.IsKeyDown(Keys.A) && _AnswerShown == false)
            {
                Remove(_Question);
                _Answer = new Text(230, 400, "Comic", Color.Black,
                                        "sauba alta, weiter gehts..." + Environment.NewLine + "Drücke Enter zum fortfahren!",
                                        "Schriftrolle");
                DrawableTexts.Add(_Answer);
                _AnswerShown = true;
                _RightAnswer = true;
            }

            if ((keyboard.IsKeyDown(Keys.B) || keyboard.IsKeyDown(Keys.C)) && _AnswerShown == false)
            {
                Remove(_Question);
                _Answer = new Text(230, 400, "Comic", Color.Black,
                    "dumpfknödel, mann, mann, mann" + Environment.NewLine + "Drücke Enter zum fortfahren!",
                    "Schriftrolle");
                DrawableTexts.Add(_Answer);
                _AnswerShown = true;
                _QuestionShown = false;
            }


            // Update the collidables and drawables
            base.Update(gameTime);
        }

        private void ShowQuestion()
        {

            if (!_QuestionShown)
            {
                var text = "warum ist die banane krumm?" + Environment.NewLine + Environment.NewLine
                         + "a) deshalb " + Environment.NewLine
                         + "b) weil halt " + Environment.NewLine
                         + "c) is halt so " + Environment.NewLine + Environment.NewLine
                         + "Drücke den entsprechenden Buchstaben auf der Tastatur!";
                _Question = new Text(230, 180, "Comic", Color.Black, text, "Schriftrolle");

                DrawableTexts.Add(_Question);
                _QuestionShown = true;
            }
        }
    }
}
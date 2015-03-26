using System;
using MarriageManiac.Texts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;

namespace MarriageManiac.Scenes
{
    public class Level1Scene : Scene
    {
        Goofy _Goofy = null;
        Snoopy _Snoopy = null;
        Texture2D _CloudTexture = null;
        private bool _LevelSymbolShown = false;
        TimeSpan _LevelSymbolShowTime = new TimeSpan();
        DrawableMovable _LevelSymbol = null;
        Text _Explanation = null;
        bool _ExplanationShown = false;

        public Level1Scene()
            : base()
        {
            var music = SoundStore.Create("PacmanMusic");
            music.Pitch = -0.4f;
            music.Volume = 0.4f;
            music.IsLooped = true;
            music.Play();

            _CloudTexture = GoofyGame.CONTENT.Load<Texture2D>("cloud_PNG13");

            _Goofy = new Goofy(10, 660);
            _Snoopy = new Snoopy(500, 600);

            _LevelSymbol = new DrawableMovable(-100, -100, GoofyGame.CONTENT.Load<Texture2D>("Level1"));
            _LevelSymbol.TargetReached += (obj, arg) => { _LevelSymbolShown = true; _LevelSymbol.ResetRotation(); };
            _LevelSymbol.MoveToTarget(350, 300, 2f);
            _LevelSymbol.SetOrigin(Drawable.OriginPoint.Center); // Rotation around the center.
            _LevelSymbol.RotateContiniously(0.1f);

            DrawableObjects.Add(_LevelSymbol);
            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(_Goofy);
            DrawableObjects.Add(_Snoopy);
            CollidableObjects.Add(_Goofy);
            CollidableObjects.Add(_Snoopy);
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

            if (keyboard.IsKeyDown(Keys.Enter))
            {
                Remove(_Explanation);
            }

            if (!_ExplanationShown)
            {
                var text = "Goofy braucht deine Hilfe um Daisy-Dani zu befreien." + Environment.NewLine + Environment.NewLine
                         + "Steurerung:" + Environment.NewLine
                         + "Links: <    Rechts: >    Springen: Leertaste" + Environment.NewLine + Environment.NewLine
                         + "Drücke ENTER um fortzufahren.";
                _Explanation = new Text(230, 180, "Comic", Color.Black, text, "Schriftrolle");

                DrawableTexts.Add(_Explanation);
                _ExplanationShown = true;
            }
        }
    }
}
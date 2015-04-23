using System;
using MarriageManiac.Texts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using MarriageManiac.Core.Rectangles;


//hallo andre
namespace MarriageManiac.Scenes
{
    public class GarloffScene : Scene
    {
        

        Goofy _Goofy = null;
        Texture2D _CloudTexture = null;
        private bool _LevelSymbolShown = false;
        TimeSpan _LevelSymbolShowTime = new TimeSpan();
        DrawableMovable _LevelSymbol = null;
        FillableRectangle _GoofyLifeBar;
        const int _LifeBarWidth = 400;
        Text _LifeText;

        public GarloffScene() : base()
        {}

        public override void Load()
        {
            _CloudTexture = ContentStore.LoadImage("cloud_PNG13");

            _Goofy = new Goofy(10, 660);
            _Goofy.LifeAmountChanged += new EventHandler<LifeAmountChangedArgs>(_Goofy_LifeAmountChanged);

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
            _LifeText = new Text((int)goofyIcon.Position.X + 5, goofyIcon.Bounds.Bottom + 4, "Comic", Color.Gold, " X " + _Goofy.Lifes, null);

            DrawableObjects.Add(new Cloud(400, 70, _CloudTexture, 0.5f));
            DrawableObjects.Add(new Cloud(200, 20, _CloudTexture, 0.8f));
            DrawableObjects.Add(_Goofy);
            DrawableObjects.Add(goofyIcon);
            DrawableObjects.Add(_GoofyLifeBar);
            DrawableObjects.Add(_LifeText);
            CollidableObjects.Add(_Goofy);
            DrawableObjects.Add(_LevelSymbol);
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

            // Update the collidables and drawables
            base.Update(gameTime);
        }
        
        void _Goofy_LifeAmountChanged(object sender, LifeAmountChangedArgs e)
        {
            _GoofyLifeBar.FillPercentage = e.CurrentLifePercentage;

            if (e.CurrentLifePercentage <= 0 && Action.IsNotDone("GoofyHasDied"))
            {
                Action.SetDone("GoofyHasDied");
                
                _Goofy.Die();
                _LifeText.Text = " X " + _Goofy.Lifes;
            }
        }
    }
}
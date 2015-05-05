using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using MarriageManiac.Scenes;
using MarriageManiac.Core;

namespace MarriageManiac
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GoofyGame : Microsoft.Xna.Framework.Game
    {
        public const int SCREENWIDTH = 1000;
        public const int SCREENHEIGHT = 760;
        public static ContentManager CONTENT { get; private set; }
        public static GraphicsDeviceManager GRAPHICS;
     
        SpriteBatch _SpriteBatch;
        Level _Level = null;
        int _LevelIndex = 0;
        
        public GoofyGame()
        {
            GRAPHICS = new GraphicsDeviceManager(this);
            GRAPHICS.PreferredBackBufferWidth = SCREENWIDTH;
            GRAPHICS.PreferredBackBufferHeight = SCREENHEIGHT;
            
            Content.RootDirectory = "Content";
            CONTENT = Content;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            LoadLevel(_LevelIndex);          
        }
                
        void scene_Ended(object sender, EventArgs e)
        {
            LoadLevel(++_LevelIndex);
        }
        
        private void LoadLevel(int level)
        {
            string levelName = String.Format("Level{0}.txt", _LevelIndex);

            var levelFile = Directory.GetFiles(@"Levels\", levelName).FirstOrDefault();
            if (levelFile != null)
            {
                _Level = null;
                IScene scene = null;

                switch (_LevelIndex)
                {
                    case 0: // Prolog
                        scene = new PrologScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                    case 1: // Speed-Level
                        scene = new SpeedScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                    case 2: // Schmid
                        scene = new SchmidScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                    case 3: // Garloff
                        scene = new GarloffScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                    case 4: // Pfadfinder
                        scene = new PfadfinderScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                    case 5: // Endboss
                        scene = new FinalScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                    case 6: // Hochzeitsszene
                        scene = new MarriageScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                    case 7: // Credits
                        scene = new CreditsScene();
                        scene.Ended += new EventHandler(scene_Ended);
                        _Level = new Level(scene, levelFile);
                        break;
                }

                if (_Level != null)
                {
                    _Level.Load();
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here            
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                GRAPHICS.ToggleFullScreen();
            }

            _Level.Update(gameTime);
                
            base.Update(gameTime);
        }
                
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_Level.Scene.BackColor);

            float horScaling = (float)GRAPHICS.GraphicsDevice.PresentationParameters.BackBufferWidth / (float)SCREENWIDTH;
            float verScaling = (float)GRAPHICS.GraphicsDevice.PresentationParameters.BackBufferHeight / (float)SCREENHEIGHT;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);

            Matrix globalTransformation = Matrix.CreateScale(screenScalingFactor);

            _SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, globalTransformation);                        
            //_SpriteBatch.Begin();

            #region Rotation Verständnis-Hilfe

            //Color[] colors = new Color[] { Color.White };
            //var texture = new Texture2D(GraphicsDevice, 1, 1);
            //texture.SetData<Color>(colors);

            //var angle = (float)Math.PI * 0.5f;


            //var green = new Rectangle(x:200, y:0, width:100, height:200);
            //var origin = Vector2.Zero;
            ////origin = new Vector2(green.Width / 2, green.Height / 2);
            //_SpriteBatch.Draw(texture, green, null, Color.Green, 0, origin, SpriteEffects.None, 0);

            //var red = new Rectangle(x: 500, y: 0, width: 100, height: 200);

            //var originRed = Vector2.Zero;
            //originRed = new Vector2(0, red.Height);

            //var pos = new Vector2(red.X + originRed.X, red.Y + originRed.Y);

            //_SpriteBatch.Draw(texture, pos, red, Color.Red, angle, originRed, 1, SpriteEffects.None, 0);

            //_SpriteBatch.Draw(texture, new Rectangle(0, 200, 1000, 1), null, Color.Black);

            #endregion
            
            for (int i = 0; i < _Level.Scene.DrawableObjects.Count(); i++)
            {
                _Level.Scene.DrawableObjects.ElementAt(i).Draw(_SpriteBatch);
            }

            _SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
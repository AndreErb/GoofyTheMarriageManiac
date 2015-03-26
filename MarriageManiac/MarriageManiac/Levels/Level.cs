using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;

namespace MarriageManiac
{        
    public class Level
    {
        private const int ROWS = GoofyGame.SCREENHEIGHT / BLOCKSIZE;
        private const int COLUMNS = GoofyGame.SCREENWIDTH / BLOCKSIZE;
        private const int BLOCKSIZE = 20;
        private Texture2D _BlockTexture = null;

        public Vector2 StartPoint { get; private set; }
        public Vector2 GuestPoint { get; private set; }
        public Vector2 GoalPoint { get; private set; }
        public CollisionObject Top { get; private set; }
        public CollisionObject Bottom { get; private set; }
        public CollisionObject Left { get; private set; }
        public CollisionObject Right { get; private set; }
        public IScene Scene { get; set; }

        private string LevelPath { get; set; }

        public Level(IScene scene, string levelPath)
        {
            Scene = scene;
            Scene.Level = this;
            LevelPath = levelPath;
        }


        public void Load()
        {
            SetWindowCollisions();

            if (File.Exists(LevelPath))
            {
                var rows = File.ReadAllLines(LevelPath);

                if (rows.Count() < ROWS)
                {
                    var message = String.Format("Der Level in Datei \"{0}\" enthält zu wenige Zeilen ({1} statt {2}).", LevelPath, rows.Count(), ROWS);
                    throw new InvalidDataException(message);
                }

                for (int y = 0; y < ROWS; y++)
                {
                    for (int x = 0; x < COLUMNS; x++)
                    {
                        var columns = rows[y].Count();

                        if (columns < COLUMNS)
                        {
                            var message = String.Format("Fehler in Level aus Datei \"{0}\": \r\nZeile {1} enthält zu wenige Zeichen ({2} statt {3}).", LevelPath, y + 1, columns, COLUMNS);
                            throw new InvalidDataException(message);
                        }

                        char symbol = rows[y][x];
                        var content = GetContent(x, y, symbol);

                        if (content != null)
                        {
                            Scene.CollidableObjects.Add(content);
                            Scene.DrawableObjects.Add(content);
                        }
                    }
                }
            }
        }


        public bool IsWall(ICollidable c)
        {
            return c == Left || c == Right || c == Top || c == Bottom;
        }

        public void Update(GameTime gameTime)
        {
            if (Scene != null)
            {
                Scene.Update(gameTime);
            }
        }

        private void SetWindowCollisions()
        {
            Top = new CollisionObject(new Rectangle(0, 0, COLUMNS * BLOCKSIZE, 1)) { Visible = false };
            Bottom = new CollisionObject(new Rectangle(0, ROWS * BLOCKSIZE, COLUMNS * BLOCKSIZE, 1)) { Visible = false };
            Left = new CollisionObject(new Rectangle(0, 0, 1, ROWS * BLOCKSIZE)) { Visible = false };
            Right = new CollisionObject(new Rectangle(COLUMNS * BLOCKSIZE, 0, 1, ROWS * BLOCKSIZE)) { Visible = false };

            Scene.CollidableObjects.Add(Top);
            Scene.CollidableObjects.Add(Bottom);
            Scene.CollidableObjects.Add(Left);
            Scene.CollidableObjects.Add(Right);
        }


        private IGameContent GetContent(int x, int y, char symbol)
        {
            switch (symbol)            
            {
                case '#':
                    {
                        if (_BlockTexture == null)
                        {
                            _BlockTexture = GoofyGame.CONTENT.Load<Texture2D>("Brick_Block");
                        }

                        var bounds = new Rectangle(x * BLOCKSIZE, y * BLOCKSIZE, BLOCKSIZE, BLOCKSIZE);
                        return new CollisionObject(bounds) 
                        {
                            CurrentImage = _BlockTexture
                        };
                    }

                default: 
                    return null;
            }
        }
    }
}

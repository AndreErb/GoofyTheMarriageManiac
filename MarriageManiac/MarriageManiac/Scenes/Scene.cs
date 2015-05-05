using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Texts;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;

namespace MarriageManiac
{
    public abstract class Scene : IScene
    {
        public Scene()
        {
            CollidableObjects = new List<ICollidable>();
            DrawableObjects = new List<MarriageManiac.Core.IDrawable>();
            BackColor = Color.CornflowerBlue;
            Started = true;
            Action = new ActionStore();
            SoundStore = new SoundStore();
        }

        public List<ICollidable> CollidableObjects { get; private set; }
        public List<MarriageManiac.Core.IDrawable> DrawableObjects { get; private set; }
        public Level Level { get; set; }
        public Color BackColor { get; set; }
        public virtual event EventHandler Ended;
        private bool Started { get; set; }
        protected ActionStore Action { get; private set; }
        protected SoundStore SoundStore { get; private set; }

        public virtual void Update(GameTime gameTime)
        {
            if (Started)
            {
                for (int i = 0; i < CollidableObjects.Count(); i++)
                {
                    var collidable = CollidableObjects.ElementAt(i);
                    collidable.Update(this, gameTime);
                }

                for (int i = 0; i < DrawableObjects.Count(); i++)
                {
                    var drawable = DrawableObjects.ElementAt(i);
                    drawable.Update(gameTime);
                }
            }
        }

        public void Add(IGameContent content)
        {
            CollidableObjects.Add(content);
            DrawableObjects.Add(content);
        }

        public void Remove(IGameContent content)
        {
            CollidableObjects.Remove(content);
            DrawableObjects.Remove(content);
        }

        public void Remove(MarriageManiac.Core.IDrawable content)
        {
            DrawableObjects.Remove(content);
        }

        public void Clear(bool reloadLevel)
        {
            CollidableObjects.Clear();
            DrawableObjects.Clear();

            if (reloadLevel)
            {
                Level.Load();
            }
        }

        protected virtual void OnEnd()
        {
            SoundStore.StopAll();

            if (Ended != null)
            {
                Ended(this, new EventArgs());
            }
        }

        public virtual void Load()
        {
            // Should be overwritten in sub classes!
        }
        
        public virtual void Start()
        {
            Started = true;
        }

        public virtual void Stop()
        {
            Started = false;
        }

        protected void GameOver()
        {
            Clear(false);
            BackColor = Color.DarkRed;
            DrawableObjects.Add(new GameOverSymbol());
        }
    } 
}
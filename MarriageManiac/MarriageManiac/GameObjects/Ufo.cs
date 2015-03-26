using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;
using MarriageManiac.Characters;

namespace MarriageManiac.GameObjects
{
    class Ufo : DrawableMovableCollidable, IGameContent
    {
        decimal _LifePercentage = 0;
        bool _Started = false;

        public Ufo(int startX, int startY)
            : base(startX, startY, GoofyGame.CONTENT.Load<Texture2D>("UFO_Links"))
        {
            MoveArea = new Rectangle(400, 50, 500, 400);
            LastMoveTime = new TimeSpan();
            Random = new Random();
            TargetReached += (obj, e) => Moving = false;

            CanCollide = true;            
        }

        public event EventHandler<LifeAmountChangedArgs> LifeAmountChanged;
        public bool Shooting { get; set; }
        private Rectangle MoveArea { get; set; }
        private TimeSpan LastMoveTime { get; set; }
        private TimeSpan LastShotTime { get; set; }
        private Random Random { get; set; }
        private bool Moving { get; set; }
        

        public bool Started
        {
            get { return _Started; }
            set
            {
                _Started = value;
                LifePercentage = 100m;
            }
        }

        public decimal LifePercentage
        {
            get { return _LifePercentage; }
            set 
            {
                _LifePercentage = value;
                OnLifeChanged(_LifePercentage);

                if (_LifePercentage <= 0)
                {
                    CrashDown();
                }
            }
        }

        private void Shoot(Scene scene, GameTime gameTime)
        {
            if (Shooting)
            {
                var elapsed = gameTime.TotalGameTime - LastShotTime;
                if (elapsed >= TimeSpan.FromSeconds(1.2))
                {
                    LastShotTime = gameTime.TotalGameTime;
                    var goofy = scene.CollidableObjects.First(c => c is Goofy);
                    var shot = new PryBarShot(this);
                    shot.ShootAt(goofy);
                    scene.Add(shot);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            var elapsed = gameTime.TotalGameTime - LastMoveTime;
            if (elapsed >= TimeSpan.FromSeconds(2.5))
            {
                MoveRandomly(gameTime);
            }
            
            base.Update(gameTime);
        }

        public override void Update(Scene scene, GameTime gameTime)
        {
            HitCheck(scene, gameTime);

            Shoot(scene, gameTime);

            base.Update(scene, gameTime);
        }
           
        private void MoveRandomly(GameTime gameTime)
        {
            if (Started && !Moving && _LifePercentage > 0)
            {
                var target = GetNextTarget();
                MoveToTarget(target.X, target.Y, 0.5f);

                Moving = true;
                LastMoveTime = gameTime.TotalGameTime;
            }
        }
                
        private Vector2 GetNextTarget()
        {
            var x = Random.Next(MoveArea.Left, MoveArea.Right);
            var y = Random.Next(MoveArea.Top, MoveArea.Bottom);

            return new Vector2(x, y);
        }

        private void HitCheck(Scene scene, GameTime gameTime)
        {
            for (int i = 0; i < scene.CollidableObjects.Count(); i++)
            {
                var c = scene.CollidableObjects.ElementAt(i);
                if (CollidesWith(c.Bounds))
                {
                    var hurt = c as IHurt;

                    if (hurt != null && hurt.Originator != this)
                    {
                        LifePercentage -= hurt.LifeCostPercentage;

                        // If we are hit we instantly try to move away, 
                        // without waiting for the next move cycle.
                        MoveRandomly(gameTime);
                    }
                    else if (c == scene.Level.Bottom && LifePercentage <= 0)
                    {
                        scene.Remove(this);
                    }

                    base.OnWouldCollideWith(c);
                }
            }
        }
               
        private void OnLifeChanged(decimal currentLifePercentage)
        {
            if (LifeAmountChanged != null)
            {
                LifeAmountChanged(this, new LifeAmountChangedArgs(currentLifePercentage));
            }
        }

        private void CrashDown()
        {
            SetOrigin(OriginPoint.Center);
            RotateContiniously(-0.1f);
            MoveToTarget(500, 700, 0.4f);
        }
    }


    public class LifeAmountChangedArgs : EventArgs
    {
        public LifeAmountChangedArgs(decimal currentLifePercentage)
        {
            CurrentLifePercentage = currentLifePercentage;
        }

        public decimal CurrentLifePercentage { get; private set; }
    } 
}
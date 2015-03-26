using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.GameObjects
{
    class PryBarShot : DrawableMovableCollidable, IGameContent, IHurt
    {
        public PryBarShot(Ufo shooter)
            : base(shooter.Bounds.Right, shooter.Bounds.Top, GoofyGame.CONTENT.Load<Texture2D>("Brecheisen"))
        {
            Ufo = shooter;
            Visible = false;
            CanCollide = true;
            SetOrigin(OriginPoint.Center);            
            RotateContiniously(-0.075f);
        }

        public IGameContent Originator { get { return Ufo; } }
        public Ufo Ufo { get; private set; }
        public decimal LifeCostPercentage { get { return 15; } }

        private bool Shot { get; set; }
        private bool Exploded { get; set; }

        public void ShootAt(ICollidable victim)
        {
            if (!Shot)
            {
                Visible = Shot = true;

                Position = new Vector2(Ufo.Bounds.Left - this.Bounds.Width, Ufo.Bounds.Bottom);

                MoveInDirection(victim.Bounds.X, victim.Bounds.Top, 0.25f);
            }
        }

        public override void Update(Scene scene, GameTime gameTime)
        {
            if (Ufo.LifePercentage <= 0)
            {
                scene.Remove(this);
                ExplodeIn(scene);
            }
            else
            {
                CheckHit(scene);
            }

            base.Update(scene, gameTime);
        }

        private void CheckHit(Scene scene)
        {
            if (!Exploded)
            {
                var collision = scene.CollidableObjects.FirstOrDefault(c => !(c is PryBarShot) && c != Ufo && CollidesWith(c.Bounds));
                if (collision != null)
                {
                    scene.Remove(this);

                    if (!scene.Level.IsWall(collision))
                    {
                        ExplodeIn(scene);
                    }

                    base.OnWouldCollideWith(collision);
                }
            }
        }

        private void ExplodeIn(Scene scene)
        {
            Exploded = true;
            scene.Add(new Explosion(Bounds.X, Bounds.Y - 15));
        }
    }
}
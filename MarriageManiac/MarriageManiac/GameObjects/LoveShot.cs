using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;

namespace MarriageManiac.GameObjects
{
    class LoveShot : DrawableMovableCollidable, IGameContent, IHurt
    {
        public LoveShot(CharacterBase shooter)
            : base(shooter.Bounds.Right, shooter.Bounds.Top, ContentStore.LoadImage("HeartShotRight"))
        {
            Goofy = shooter;
            Visible = false;
            CanCollide = true;
        }

        public IGameContent Originator { get { return Goofy; } }
        public CharacterBase Goofy { get; private set; }
        public decimal LifeCostPercentage { get { return 5; } }

        private bool Shot { get; set; }
        private bool Exploded { get; set; }

        public void Shoot()
        {
            if (!Shot)
            {
                Visible = Shot = true;
                Position = new Vector2(Goofy.Bounds.Right, Goofy.Bounds.Top);

                int xDirection = 1;
                if (Goofy.ViewDirection == Direction.Left)
                {
                    xDirection *= -1;
                    Position = new Vector2(Goofy.Bounds.Left - this.Bounds.Width, Goofy.Bounds.Top);
                    CurrentImage = ContentStore.LoadImage("HeartShotLeft");
                }
                else
                {
                    CurrentImage = ContentStore.LoadImage("HeartShotRight");
                }

                MoveToTarget(1200 * xDirection, Goofy.Bounds.Top, 0.35f);
            }
        }

        public override void Update(Scene scene, GameTime gameTime)
        {
            if (!Exploded)
            {
                for (int i = 0; i < scene.CollidableObjects.Count(); i++)
                {
                    var c = scene.CollidableObjects.ElementAt(i);
                    if (c != this && c != Goofy && CollidesWith(c.Bounds))
                    {
                        Exploded = true;
                        scene.Remove(this);

                        if (!scene.Level.IsWall(c))
                        {
                            scene.Add(new Explosion(Bounds.X, Bounds.Y - 15));
                        }

                        base.OnWouldCollideWith(c);
                        break;
                    }
                }
            }

            base.Update(scene, gameTime);
        }
    }
}
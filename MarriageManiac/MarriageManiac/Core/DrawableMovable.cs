using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MarriageManiac.Core
{
    class DrawableMovable : Drawable, IMovable
    {
        public enum MoveMode { FixedTarget, Direction };

        public DrawableMovable(int xPos, int yPos, Texture2D image)
            : base(xPos, yPos, image)
        {
            Visible = true;
            Position = new Vector2(xPos, yPos);
            TargetPosition = new Vector2();
            Speed = 0;
            CurrentImage = image;
        }

        public Vector2 TargetPosition { get; protected set; }
        public float Speed { get; protected set; }
        public event EventHandler<MovingEventArgs> Moved;
        public event EventHandler TargetReached;
        private MoveMode Mode { get; set; }
        private Vector2 DirectionVector { get; set; }

        public virtual void Move(Direction direction, float steps)
        {
            if (direction == Direction.Left)
            {
                Position -= new Vector2(steps, 0);
            }
            else if (direction == Direction.Right)
            {
                Position += new Vector2(steps, 0);
            }
        }


        public virtual void MoveToTarget(float targetX, float targetY, float speed)
        {
            Mode = MoveMode.FixedTarget;
            TargetPosition = new Vector2(targetX, targetY);
            Speed = speed;
        }

        public virtual void MoveInDirection(float targetX, float targetY, float speed)
        {
            Mode = MoveMode.Direction;
            TargetPosition = new Vector2(targetX, targetY);
            Speed = speed;
        }


        public override void Update(GameTime gameTime)
        {
            if (Mode == MoveMode.FixedTarget)
            {
                MoveToTargetBy(gameTime);
            }
            else if (Mode == MoveMode.Direction)
            {
                MoveInDirection(gameTime);
            }

            base.Update(gameTime);
        }


        private void MoveToTargetBy(GameTime gameTime)
        {
            if (Position != TargetPosition)
            {
                var nextPosition = Position + CalculateMovement(gameTime);

                if (IsBeyondTargetX(nextPosition.X))
                {
                    nextPosition.X = TargetPosition.X;
                }

                if (IsBeyondTargetY(nextPosition.Y))
                {
                    nextPosition.Y = TargetPosition.Y;
                }

                Position = nextPosition;
                OnMoved();
            }
            else
            {
                OnTargetReached();
            }
        }

        private void MoveInDirection(GameTime gameTime)
        {
            if (DirectionVector == Vector2.Zero)
            {
                Vector2 direction = TargetPosition - Position;

                direction.Normalize(); // Create a "Unit Vecor" (Einheitsvektor) 

                DirectionVector = direction;
            }

            Position = Position + DirectionVector * Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            OnMoved();
        }


        private Vector2 CalculateMovement(GameTime gameTime)
        {
            Vector2 distance = TargetPosition - Position;

            distance.Normalize(); // Create a "Unit Vecor" (Einheitsvektor) 

            return distance * Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }


        private bool IsBeyondTargetX(float nextPos)
        {
            if ((Position.X < TargetPosition.X && nextPos > TargetPosition.X) ||
                (Position.X > TargetPosition.X && nextPos < TargetPosition.X))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private bool IsBeyondTargetY(float nextPos)
        {
            if ((Position.Y < TargetPosition.Y && nextPos > TargetPosition.Y) ||
                (Position.Y > TargetPosition.Y && nextPos < TargetPosition.Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void OnMoved()
        {
            if (Moved != null)
            {
                Moved(this, new MovingEventArgs(Position));
            }
        }

        protected virtual void OnTargetReached()
        {
            if (TargetReached != null)
            {
                TargetReached(this, new EventArgs());
            }
        }
    }


    public class MovingEventArgs : EventArgs
    {
        public MovingEventArgs(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; set; } 
    }
}
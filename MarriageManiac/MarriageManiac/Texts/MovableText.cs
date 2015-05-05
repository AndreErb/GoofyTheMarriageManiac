using MarriageManiac.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarriageManiac.Texts
{
    public class MovableText : Text
    {
        public enum MoveMode { FixedTarget, Direction };

        public MovableText(int xPos, int yPos, string font, Color color, string text, string texture) 
            : base(xPos, yPos, font, color, text, texture)
        {
        }

        public Vector2 TargetPosition { get; protected set; }
        public float Speed { get; protected set; }
        public event EventHandler<MovingEventArgs> Moved;
        public event EventHandler TargetReached;
        private MoveMode Mode { get; set; }
        private Vector2 DirectionVector { get; set; }
        public bool IsTargetReached { get; private set; }

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
            IsTargetReached = false;
            Mode = MoveMode.FixedTarget;
            TargetPosition = new Vector2(targetX, targetY);
            Speed = speed;
        }

        public virtual void MoveInDirection(float targetX, float targetY, float speed)
        {
            IsTargetReached = false;
            Mode = MoveMode.Direction;
            TargetPosition = new Vector2(targetX, targetY);
            Speed = speed;
        }


        public override void Update(GameTime gameTime)
        {
            if (Mode == MoveMode.FixedTarget && !IsTargetReached)
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
            IsTargetReached = true;

            if (TargetReached != null)
            {
                TargetReached(this, new EventArgs());
            }
        }
    }
}

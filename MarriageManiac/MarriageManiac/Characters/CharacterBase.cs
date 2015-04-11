using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;

namespace MarriageManiac
{
    public enum Direction
    {
        None,
        Left,
        Right
    }

    public enum JumpMode
    {
        Not,
        Jump
    }


    abstract class CharacterBase : Rotation, IGameContent
    {
        const float STEP = 10;
        int gravity = 3;
        bool moving = false;
        bool jumping = false;
        bool falling = false;
        Vector2 _Velocity = new Vector2();
        Texture2D _CurrentImage = null;
        Rectangle _Bounds;
        decimal _LifePercentage;
        DrawableMovable _TargetMovement;

        public CharacterBase()
        {
            Direction = Direction.None;
            ViewDirection = Direction.None;
            JumpMode = JumpMode.Not;
            _Bounds = new Rectangle();
            Visible = true;
            CanCollide = true;
            Origin = new Vector2();
            SoundStore = new SoundEngine();
            AllowFall = true;
            LifePercentage = 100;
            InitSounds();
        }
        
        public event EventHandler<LifeAmountChangedArgs> LifeAmountChanged; 
        public event EventHandler<WouldCollideEventArgs> WouldCollideWith;
        public event EventHandler<MovingEventArgs> Moved;

        public Direction Direction { get; private set; }
        public Direction ViewDirection { get; protected set; }
        public JumpMode JumpMode { get; private set; }
        public bool Visible { get; set; }
        public bool CanCollide { get; set; }
        public bool AllowFall { get; set; }
        protected SoundEngine SoundStore { get; private set; }

        public virtual decimal LifePercentage
        {
            get { return _LifePercentage; }
            set
            {
                _LifePercentage = value;
                OnLifeChanged(_LifePercentage);
            }
        }
      
        public Texture2D CurrentImage
        {
            get { return _CurrentImage; }
            set
            {
                Bounds = new Rectangle(Bounds.X, Bounds.Y, value.Width, value.Height);
                _CurrentImage = value;
            }
        }

        public Rectangle Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; }
        }

        public Vector2 Position
        {
            get { return new Vector2(Bounds.X, Bounds.Y); }
        }

        public virtual void Die()
        {
            CanCollide = false;
            Jump(JumpMode.Jump);
        }

        public virtual void ReviveAt(int x, int y)
        {
            LifePercentage = 100;
            jumping = false;
            CanCollide = true;
            Bounds = new Rectangle(x, y, CurrentImage.Width, CurrentImage.Height);
        }

        public virtual void Update(Scene scene, GameTime gameTime)
        {
            CheckHurt(scene);
            Jump(scene.CollidableObjects);

            if (_TargetMovement != null)
            {
                _TargetMovement.Update(gameTime);
            }
            else
            {
                Move(scene.CollidableObjects);
            }
            
            Fall(scene.CollidableObjects);
        }
        
        public virtual void Jump(JumpMode mode)
        {
            JumpMode = mode;
        }
        
        public virtual void MoveToTarget(float targetX, float targetY, float speed)
        {
            _TargetMovement = new DrawableMovable((int)Position.X, (int)Position.Y, null);

            _TargetMovement.Moved += (obj, e) =>
            {
                _Bounds.X = (int)e.Position.X;
                _Bounds.Y = (int)e.Position.Y;
                OnMoved();
            };

            _TargetMovement.MoveToTarget(targetX, targetY, speed);
        }

        public virtual void Move(Direction direction)
        {
            Direction = direction;
            _TargetMovement = null;

            if (direction == Direction.Left || direction == Direction.Right)
            {
                ViewDirection = direction;
            }
        }

        private void InitSounds()
        {
            var jump = SoundStore.Create("Jump");
            jump.Volume = 0.1f;
        }

                
        private void Move(IEnumerable<ICollidable> collidables)
        {
            if (!moving && Direction == Direction.Left)
            {
                var nextPosition = new Point(Bounds.X - (int)STEP, Bounds.Y);

                if (CanMoveTo(nextPosition, collidables))
                {
                    moving = true;
                    _Velocity.X = -STEP;
                }
            }
            else if (!moving && Direction == Direction.Right)
            {
                var nextPosition = new Point(Bounds.X + (int)STEP, Bounds.Y);

                if (CanMoveTo(nextPosition, collidables))
                {
                    moving = true;
                    _Velocity.X = STEP;
                }
            }
            else
            {
                moving = false;
            }

            if (moving)
            {
                _Bounds.X = Bounds.X + (int)_Velocity.X;
            }
        }

        private void Fall(IEnumerable<ICollidable> collidables)
        {
            if (AllowFall && CanFall(collidables))
            {
                if (!falling)
                {
                    _Velocity.Y = gravity;
                }

                var preview = new Point((int)Bounds.X, (int)(Bounds.Y + _Velocity.Y));
                var collision = WouldCollide(collidables, preview);

                if (collision == null)
                {
                    _Bounds.Y = Bounds .Y + (int)_Velocity.Y;
                    _Velocity.Y += gravity;
                    falling = true;
                }
                else // The "Floor" is also included in the collision objects.
                {
                    _Bounds.Y = collision.Bounds.Y - Bounds.Height;
                    falling = false;
                }
            }
        }

        private bool CanMoveTo(Point toPosition, IEnumerable<ICollidable> collidables)
        {
            var collision = WouldCollide(collidables, toPosition);
            return collision == null;
        }

        private bool CanFall(IEnumerable<ICollidable> collidables)
        {
            bool canFall = false;

            if (!jumping)
            {
                var leftFoot = new Rectangle(Bounds.Left, Bounds.Bottom, 1, 1);
                var rightFoot = new Rectangle(Bounds.Right, Bounds.Bottom, 1, 1);

                var firstBottom = collidables.
                                  FirstOrDefault(c =>
                                                 c != this &&
                                                 c.CollidesWith(leftFoot) && c.CollidesWith(rightFoot));
                canFall = firstBottom == null;
            }

            return canFall;
        }

        private void Jump(IEnumerable<ICollidable> collidables)
        {
            if (!falling)
            {
                if (!jumping && JumpMode == JumpMode.Jump)
                {
                    _Velocity.Y = -25;
                    jumping = true;
                    SoundStore.Sound("Jump").Play();
                }

                if (jumping)
                {
                    var preview = new Point(Bounds.X, (int)(Bounds.Y + _Velocity.Y));
                    var collision = WouldCollide(collidables, preview);

                    if (collision == null || collision is IHurt) // We should NOT be able to land and stand on shots (= IHurt) etc (Is this true for all IHurt???)
                    {                        
                        _Bounds.Y = Bounds.Y + (int)_Velocity.Y;
                    }
                    else if (collision.Bounds.Bottom > Bounds.Y) // Prevents jumping onto bricks, which are far above the character.
                    {
                        SoundStore.Sound("Jump").Stop();
                        // Jumped an landed on something.
                        _Bounds.Y = collision.Bounds.Y - Bounds.Height;
                        jumping = false;
                    }

                    _Velocity.Y += gravity; // Reduce jump velocity.
                }
            }
        }

        public ICollidable WouldCollide(IEnumerable<ICollidable> collidables, Point position)
        {
            ICollidable wouldCollideWith = null;

            if (CanCollide)
            {
                var bounds = new Rectangle(position.X, position.Y, Bounds.Width, Bounds.Height);

                wouldCollideWith = collidables.FirstOrDefault(c => c != this && c.CollidesWith(bounds));

                if (wouldCollideWith != null && WouldCollideWith != null)
                {
                    WouldCollideWith(this, new WouldCollideEventArgs(wouldCollideWith));
                }
            }

            return wouldCollideWith;
        }

        public bool CollidesWith(Rectangle bounds)
        {
            if (CanCollide)
            {
                return Bounds.Intersects(bounds);
            }
            else
            {
                return false;
            }
        }
                
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                var destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Bounds.Width, Bounds.Height);
                spriteBatch.Draw(CurrentImage, destinationRectangle, null, Color.White, Angle, Origin, SpriteEffects.None, 0f);
            }
        }


        private void CheckHurt(Scene scene)
        {
            for (int i = 0; i < scene.CollidableObjects.Count(); i++)
            {
                var c = scene.CollidableObjects.ElementAt(i);
                if (CollidesWith(c.Bounds))
                {
                    var hurtable = c as IHurt;
                    if (hurtable != null)
                    {
                        var hurted = true;

                        // If there is an originator of the hurt, we have to check, that its not ourself.
                        if (hurtable.Originator != null && hurtable.Originator == this)
                        {
                            // We cannot hurt ourselves!
                            hurted = false;
                        }

                        if (hurted)
                        {
                            LifePercentage -= hurtable.LifeCostPercentage;                            
                        }
                    }
                }
            }
        }

        protected void OnLifeChanged(decimal currentLifePercentage)
        {
            if (LifeAmountChanged != null)
            {
                LifeAmountChanged(this, new LifeAmountChangedArgs(currentLifePercentage));
            }
        }

        protected virtual void OnMoved()
        {
            if (Moved != null)
            {
                Moved(this, new MovingEventArgs(Position));
            }
        }
    }
}
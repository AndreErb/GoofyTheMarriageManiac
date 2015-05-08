using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MarriageManiac.Core;
using MarriageManiac.Characters;

namespace MarriageManiac.Core
{
    public interface IScene
    {
        List<ICollidable> CollidableObjects { get; }
        List<IDrawable> DrawableObjects { get; }
        Level Level { get; set; }
        Color BackColor { get; set; }
        void Load(Goofy goofy);
        void Start();
        void Stop();
        void Update(GameTime gameTime);
        event EventHandler<SceneEndArgs> Ended;
    }
    
    public interface IDrawable
    {
        float Angle { get; }
        Vector2 Origin { get; set; }
        void RotateOnce(float angle);
        void RotateContiniously(float angle);
        void ResetRotation();
        bool Visible { get; set; }
        Texture2D CurrentImage { get; set; }
        Vector2 Position { get; }
        Rectangle Bounds { get; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public interface IMovable 
    {
        float Speed { get; }
        Vector2 TargetPosition { get; }
        void Move(Direction direction, float steps);
        void MoveToTarget(float targetX, float targetY, float speed);        
        event EventHandler TargetReached;
    }

    public interface IDrawableText : IDrawable
    {
        string Text { get; set; }
        SpriteFont Font { get; }
        Color Color { get; set; }
        float Scale { get; set; }
        SpriteEffects Effect { get; set; }
        event EventHandler Completed;
    }

    public interface ICollidable 
    {
        Rectangle Bounds { get; }
        bool CanCollide { get; set; }
        bool CollidesWith(Rectangle bounds);
        ICollidable WouldCollide(IEnumerable<ICollidable> collidables, Point position);
        event EventHandler<WouldCollideEventArgs> WouldCollideWith;
        void Update(Scene scene, GameTime gameTime);        
    }
    
    /// <summary>
    /// Something that makes aua.
    /// </summary>
    public interface IHurt
    {
        /// <summary>
        /// Gets the life cost percentage.
        /// </summary>
        /// <value>
        /// The life cost percentage.
        /// </value>
        decimal LifeCostPercentage { get; }

        /// <summary>
        /// Gets the object, that caused the hurting.
        /// </summary>
        /// <value>
        /// The hurt originator, or null if no originator exists.
        /// </value>
        IGameContent Originator { get; }
    }
        
    public interface IGameContent : IDrawable, ICollidable
    {        
    }    
}
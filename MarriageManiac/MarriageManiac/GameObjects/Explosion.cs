using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.GameObjects
{
    class Explosion : DrawableMovableCollidable, IGameContent
    {
        TimeSpan _TimeShown = new TimeSpan();

        public Explosion(int startX, int startY)
            : base(startX, startY, ContentStore.LoadImage("Explosion"))
        {
            CanCollide = false;

            var explosion = new SoundStore().Create("SmallExplosion");
            explosion.Instance.Volume = 0.15f;
            explosion.Instance.Play();
        }
                
        public override void Update(Scene scene, GameTime gameTime)
        {
            _TimeShown += gameTime.ElapsedGameTime;
            
            if (_TimeShown >= TimeSpan.FromSeconds(0.25))
            {
                scene.Remove(this);
            }

            base.Update(scene, gameTime);
        }
    }
}
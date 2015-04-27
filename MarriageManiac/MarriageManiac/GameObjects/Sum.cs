using MarriageManiac.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarriageManiac.GameObjects
{
    class Sum : DrawableMovableCollidable, IGameContent
    {
        public Sum(int x, int y) :
            base(x, y, ContentStore.LoadImage("summe"))
        {
            CanCollide = true;

        }

        //public override void Update(Scene scene, GameTime gameTime)
        //{
        //    HitCheck(scene, gameTime);
        //    base.Update(scene, gameTime);
        //}

        //private void HitCheck(Scene scene, GameTime gameTime)
        //{
        //    for (int i = 0; i < scene.CollidableObjects.Count(); i++)
        //    {
        //        var c = scene.CollidableObjects.ElementAt(i);
        //        if (c is LoveShot && CollidesWith(c.Bounds))
        //            scene.Remove(this);

        //        base.OnWouldCollideWith(c);
        //    }
        //}
    }
}

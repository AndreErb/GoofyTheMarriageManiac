using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarriageManiac.Core;
using Microsoft.Xna.Framework;

namespace MarriageManiac.GameObjects
{
    class LoveExplosion
    {
        DrawableMovable left;
        DrawableMovable leftUp;
        DrawableMovable leftDown;
        DrawableMovable right;
        DrawableMovable rightUp;
        DrawableMovable rightDown;
        DrawableMovable up;
        DrawableMovable down;

        public LoveExplosion(Scene scene, int x, int y)
        {
            var heart = ContentStore.LoadImage("HeartShotLeft");

            left = new DrawableMovable(x, y, heart);
            leftUp = new DrawableMovable(x, y, heart);
            leftDown = new DrawableMovable(x, y, heart);
            right = new DrawableMovable(x, y, heart);
            rightUp = new DrawableMovable(x, y, heart);
            rightDown = new DrawableMovable(x, y, heart);
            up = new DrawableMovable(x, y, heart);
            down = new DrawableMovable(x, y, heart);

            scene.DrawableObjects.Add(left);
            scene.DrawableObjects.Add(leftUp);
            scene.DrawableObjects.Add(leftDown);
            scene.DrawableObjects.Add(right);
            scene.DrawableObjects.Add(rightUp);
            scene.DrawableObjects.Add(rightDown);
            scene.DrawableObjects.Add(up);
            scene.DrawableObjects.Add(down);

            var speed = 0.2f;

            left.MoveToTarget(-300, y, speed);
            leftUp.MoveToTarget(-300, -300, speed);
            leftDown.MoveToTarget(-300, 1060, speed);

            right.MoveToTarget(1300, y, speed);
            rightUp.MoveToTarget(1300, -300, speed);
            rightDown.MoveToTarget(1300, 1060, speed);

            up.MoveToTarget(x, -300, speed);
            down.MoveToTarget(x, 1060, speed);
        }
    }
}
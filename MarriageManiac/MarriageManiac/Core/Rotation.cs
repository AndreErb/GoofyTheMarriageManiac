using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MarriageManiac.Core
{
    public class Rotation
    {
        private enum RotationMode { None, Once, Continious };

        public Rotation()
        {
            ResetRotation();
        }

        /// <summary>
        /// Angle in radiants.
        /// In contrast to normal math, XNA counts Radiants clockwise!
        /// In XNA: 0.5f = PI/2 = 90°  (rotation right)
        /// In XNA: -0.5f = PI/2 = 90°  (rotation left)
        /// In XNA: 1f = PI = 180°  (rotation right)
        /// </summary>
        public float Angle { get; private set; }
        public Vector2 Origin { get; set; }
        private RotationMode Mode { get; set; }
        private float ContiniousAngle {get; set;}
        private Func<bool> Until { get; set; }

        public virtual void RotateOnce(float angle)
        {
            Mode = RotationMode.Once;
            Angle = (float)Math.PI * angle;
        }

        public virtual void RotateContiniously(float angle)
        {
            Mode = RotationMode.Continious;
            ContiniousAngle = angle;
        }


        public virtual void RotateContiniously(float angle, Func<bool> until)
        {
            ContiniousAngle = angle;
            Mode = RotationMode.Continious;
            Until = until;
        }
        
        
        public virtual void Update(GameTime gameTime)
        {
            if (Mode == RotationMode.Continious)
            {
                var carryOn = true;

                if (Until != null)
                {
                    carryOn = !Until();
                }

                if (carryOn)
                {
                    Angle += (float)Math.PI * ContiniousAngle;
                }
            }
        }
        
        public virtual void ResetRotation()
        {
            Angle = 0;
            ContiniousAngle = 0;
            Mode = RotationMode.None;
        }
    }
}
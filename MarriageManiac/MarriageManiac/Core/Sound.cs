using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarriageManiac.Core
{
    public class Sound
    {
        public Sound(SoundEffect soundEffect)
        {
            Instance = soundEffect.CreateInstance();
            Duration = soundEffect.Duration;
        }

        public SoundEffectInstance Instance { get; private set; }
        public TimeSpan Duration { get; private set; }
    }
}
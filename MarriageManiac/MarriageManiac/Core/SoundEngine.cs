using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace MarriageManiac.Core
{
    public class SoundEngine
    {
        public SoundEngine()
        {
            Store = new Dictionary<string, SoundEffectInstance>();
        }

        private Dictionary<string, SoundEffectInstance> Store { get; set; }

        public SoundEffectInstance Create(string soundName, string fileName)
        {
            if (!Store.ContainsKey(soundName))
            {
                var sound = GoofyGame.CONTENT.Load<SoundEffect>("Audio\\" + fileName).CreateInstance();
                Store.Add(soundName, sound);
            }
            
            return Store[soundName];
        }

        public SoundEffectInstance Sound(string soundName)
        {
            return Store[soundName];
        }
    }
}
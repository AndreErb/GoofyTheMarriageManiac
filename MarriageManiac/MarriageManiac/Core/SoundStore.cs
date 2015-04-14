using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace MarriageManiac.Core
{
    public class SoundStore
    {
        public SoundStore()
        {
            Store = new Dictionary<string, Sound>();
        }

        private Dictionary<string, Sound> Store { get; set; }

        public Sound Create(string soundName)
        {
            return Create(soundName, soundName);
        }

        public Sound Create(string soundName, string fileName)
        {
            if (!Store.ContainsKey(soundName))
            {
                var sound = ContentStore.LoadSound(fileName);
                Store.Add(soundName, new Sound(sound));
            }
            
            return Store[soundName];
        }

        public Sound Sound(string soundName)
        {
            return Store[soundName];
        }
    }
}
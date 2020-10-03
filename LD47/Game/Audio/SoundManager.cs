using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LD47.Game.Audio
{
    public class SoundManager
    {
        private static SoundManager _i = new SoundManager();
        public static SoundManager I => _i;

        private Dictionary<string, SoundEffect> _sfx;

        private SoundManager()
        {
            _sfx = new Dictionary<string, SoundEffect>();
        }

        public void Load(ContentManager content)
        {
            LoadSound(content, "die");
            LoadSound(content, "game_over");
            LoadSound(content, "jump");
            LoadSound(content, "land");
            LoadSound(content, "switch");
            LoadSound(content, "time_travel");
            LoadSound(content, "attack");
        }

        private void LoadSound(ContentManager content, string name)
        {
            _sfx.Add(name, content.Load<SoundEffect>($"Audio/{name}"));
        }
        
        public void PlaySfx(string name, float volume = 1f)
        {
            if(!_sfx.TryGetValue(name, out var effect))
            {
                return;
            }

            effect.Play(volume, 0f, 0f);
        }
    }
}
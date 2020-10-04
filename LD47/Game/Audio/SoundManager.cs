using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace LD47.Game.Audio
{
    public class SoundManager
    {
        private static SoundManager _i = new SoundManager();
        public static SoundManager I => _i;

        private Dictionary<string, SoundEffect> _sfx;
        private Song _song;

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
            LoadSound(content, "tick");
            LoadSound(content, "win");

            _song = content.Load<Song>("Audio/music");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.7f;
            MediaPlayer.Play(_song);
        }

        public void ResetSong()
        {
            MediaPlayer.Play(_song, TimeSpan.Zero);
        }

        private void LoadSound(ContentManager content, string name)
        {
            _sfx.Add(name, content.Load<SoundEffect>($"Audio/{name}"));
        }

        public void PlaySfx(string name, float volume = 1f)
        {
            if (!_sfx.TryGetValue(name, out var effect))
            {
                return;
            }

            effect.Play(volume, 0f, 0f);
        }
    }
}
using System;
using Microsoft.Xna.Framework;

namespace LD47.Game.Stages
{
    public class Stage
    {
        public readonly Rectangle[] Platforms;

        public Stage(Rectangle[] platforms)
        {
            Platforms = platforms;
        }

        public Vector2 GetRandomSpawnPoint(Random random)
        {
            var rect = Platforms[random.Next(0, Platforms.Length)];

            var x = random.Next(rect.Left, rect.Right);
            return new Vector2(MathHelper.Clamp(x, 16, 512 - 16), rect.Top - 1);
        }
    }
}
using System;
using Microsoft.Xna.Framework;

namespace LD47.Game.Collision
{
    public static class CollisionHelper
    {
        public static Vector2 GetMinimumTranslationDistance(Rectangle first, Rectangle second)
        {
            var minimumTranslation = Vector2.Zero;

            if (first.Center.Y < second.Center.Y)
            {
                minimumTranslation.Y = second.Top - first.Bottom;
            }
            else if (first.Center.Y >= second.Center.Y)
            {
                minimumTranslation.Y = second.Bottom - first.Top;
            }

            if (first.Center.X < second.Center.X)
            {
                minimumTranslation.X = second.Left - first.Right;
            }
            else if (first.Center.X >= second.Center.X)
            {
                minimumTranslation.X = second.Right - first.Left;
            }

            var x = MathF.Abs(minimumTranslation.X);
            var y = MathF.Abs(minimumTranslation.Y);

            if (x < y)
            {
                minimumTranslation.Y = 0;
            }
            else if (y < x)
            {
                minimumTranslation.X = 0;
            }

            return minimumTranslation;
        }
    }
}
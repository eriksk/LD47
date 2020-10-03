
using System;

namespace LD47
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random rand, float min, float max)
        {
            return min + (max - min) * (float)rand.NextDouble();
        }
    }
}
using System;

using GameTools.Graph;

namespace GameTools.Noise3D
{
    static class SimpleNoise3D
    {
        public static float GenFloatNoise(int x, int y, int z, int seed)
        {
            int n;

            n = x + y * 9194 + z * 72217 + seed * 211;
            n = (n << 13) ^ n;

            return (1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
        }
        public static double GenDoubleNoise(int x, int y, int z, int seed)
        {
            int n;

            n = x + y * 9194 + z * 72217 + seed * 211;
            n = (n << 13) ^ n;

            return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }

        public static double GenSmoothNoise(int x, int y, int z, int seed)
        {
            double corners, sides, center;

            center = 3 * GenDoubleNoise(x, y, z, seed) / 8.0;

            sides = (GenDoubleNoise(x + 1, y, z, seed) +
                       GenDoubleNoise(x - 1, y, z, seed) +
                       GenDoubleNoise(x, y + 1, z, seed) +
                       GenDoubleNoise(x, y - 1, z, seed) +
                       GenDoubleNoise(x, y, z + 1, seed) +
                       GenDoubleNoise(x, y, z - 1, seed)) / 12.0;

            corners = (GenDoubleNoise(x + 1, y + 1, z + 1, seed) +
                       GenDoubleNoise(x + 1, y + 1, z - 1, seed) +
                       GenDoubleNoise(x + 1, y - 1, z + 1, seed) +
                       GenDoubleNoise(x + 1, y - 1, z - 1, seed) +
                       GenDoubleNoise(x - 1, y + 1, z + 1, seed) +
                       GenDoubleNoise(x - 1, y + 1, z - 1, seed) +
                       GenDoubleNoise(x - 1, y - 1, z + 1, seed) +
                       GenDoubleNoise(x - 1, y - 1, z - 1, seed)) / 64.0;

            return center + sides + corners;
        }

        public static double GenInterpolatedNoise(double x, double y, double z, int seed)
        {
            int floorX = (x > 0) ? (int)x : (int)Math.Floor(x);
            int floorY = (y > 0) ? (int)y : (int)Math.Floor(y);
            int floorZ = (z > 0) ? (int)z : (int)Math.Floor(z);

            double fractionalX = x - floorX;
            double fractionalY = y - floorY;
            double fractionalZ = z - floorZ;

            double center, centerRight, bottom, bottomRight;
            double centerAbove, centerRightAbove, bottomAbove, bottomRightAbove;
            double centerInter, bottomInter, belowInter, aboveInter;

            center = GenSmoothNoise(floorX, floorY, floorZ, seed);
            bottom = GenSmoothNoise(floorX, floorY + 1, floorZ, seed);
            centerRight = GenSmoothNoise(floorX + 1, floorY, floorZ, seed);
            bottomRight = GenSmoothNoise(floorX + 1, floorY + 1, floorZ, seed);

            centerAbove = GenSmoothNoise(floorX, floorY, floorZ + 1, seed);
            bottomAbove = GenSmoothNoise(floorX, floorY + 1, floorZ + 1, seed);
            centerRightAbove = GenSmoothNoise(floorX + 1, floorY, floorZ + 1, seed);
            bottomRightAbove = GenSmoothNoise(floorX + 1, floorY + 1, floorZ + 1, seed);

            centerInter = GraphMath.CosineInterpolate(center, centerRight, fractionalX);
            bottomInter = GraphMath.CosineInterpolate(bottom, bottomRight, fractionalX);
            belowInter = GraphMath.CosineInterpolate(centerInter, bottomInter, fractionalY);

            centerInter = GraphMath.CosineInterpolate(centerAbove, centerRightAbove, fractionalX);
            bottomInter = GraphMath.CosineInterpolate(bottomAbove, bottomRightAbove, fractionalX);
            aboveInter = GraphMath.CosineInterpolate(centerInter, bottomInter, fractionalY);

            return GraphMath.CosineInterpolate(belowInter, aboveInter, fractionalZ);
        }
    }
}

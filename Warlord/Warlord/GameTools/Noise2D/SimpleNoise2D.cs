
using GameTools.Graph;

namespace GameTools.Noise2D
{
    static class SimpleNoise2D
    {
        public static float GenFloatNoise(int x, int y,  int seed)
        {
            int n;

            n = x + y * 9194 + seed * 211;
            n = (n << 13) ^ n;

            return (1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
        }
        public static double GenDoubleNoise(int x, int y, int seed)
        {
            int n;

            n = x + y * 9194 + seed * 211;
            n = (n << 13) ^ n;

            return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }

        public static double GenSmoothNoise(int x, int y, int seed)
        {
            double corners, sides, center;

            // This influences the number by doing a weighted adverage with it
            // and its eight neighbhors
            center = GenDoubleNoise(x, y, seed) / 4.0;

            sides = (GenDoubleNoise(x + 1, y, seed) +
                       GenDoubleNoise(x - 1, y, seed) +
                       GenDoubleNoise(x, y + 1, seed) +
                       GenDoubleNoise(x, y - 1, seed)) / 8.0;

            corners = (GenDoubleNoise(x + 1, y + 1, seed) +
                       GenDoubleNoise(x + 1, y - 1, seed) +
                       GenDoubleNoise(x - 1, y + 1, seed) +
                       GenDoubleNoise(x - 1, y - 1, seed)) / 16.0;

            return corners + sides + center;
        }

        public static double GenInterpolatedNoise(double x, double y, int seed)
        {
            int floorX = (int)x;
            int floorY = (int)y;

            double center, centerRight, bottom, bottomRight;
            double centerInter, bottomInter;

            center = GenSmoothNoise(floorX, floorY, seed);
            centerRight = GenSmoothNoise(floorX + 1, floorY, seed);
            bottom = GenSmoothNoise(floorX, floorY + 1, seed);
            bottomRight = GenSmoothNoise(floorX + 1, floorY + 1, seed);

            centerInter = GraphMath.CosineInterpolate(center, centerRight, floorX - x); //Interpolates the two 
            bottomInter = GraphMath.CosineInterpolate(bottom, bottomRight, floorX - x); // Horizontal parts.

            return GraphMath.CosineInterpolate(centerInter, bottomInter, y - floorY); //Interpolatse the interpolated
            //Horizontal parts vertically
        }
    }
}

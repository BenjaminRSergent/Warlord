using System.Threading;

using Microsoft.Xna.Framework;

namespace GameTools.Noise2D
{
    static class PerlinNoise2D
    {
        static public double[,] GenPerlinNoise2D(PerlinNoiseSettings2D settings, int numThreads)
        {
            double[,] noise = new double[settings.size.X, settings.size.Y];

            if(settings.numThreads < 1)
                settings.numThreads = 1;

            if(settings.numThreads == 1)
            {
                MakePerlinNoise2D(noise, Point.Zero, settings);

                return noise;
            }

            int rowsPerThread = settings.size.X / numThreads;
            int extraRows = settings.size.X % numThreads;

            PerlinNoiseSettings2D threadSettings = new PerlinNoiseSettings2D(settings);

            threadSettings.size.X = rowsPerThread;
            threadSettings.zoom = settings.zoom / 4;

            Thread[] noiseGenThreads = new Thread[numThreads];

            for(int thread = 0; thread < numThreads; thread++)
            {
                int arrayStartX = rowsPerThread * thread;

                if(thread == numThreads - 1)
                {
                    threadSettings.size.X += extraRows;
                }

                noiseGenThreads[thread] = new Thread(() => MakePerlinNoise2D(noise, new Point(arrayStartX, 0), threadSettings));
                noiseGenThreads[thread].Name = "PerlinNoise2D Thread " + thread;
                noiseGenThreads[thread].Start();
            }

            for(int thread = 0; thread < numThreads; thread++)
            {
                noiseGenThreads[thread].Join();
            }

            return noise;
        }

        static void MakePerlinNoise2D(double[,] toFill, Point fillStart, PerlinNoiseSettings2D settings)
        {
            int width;
            int height;

            int effectiveX;
            int effectiveY;

            double regionSize;

            double frequency;
            double amplitude;

            width = settings.size.X;
            height = settings.size.Y;

            regionSize = width / settings.zoom;

            for(int x = fillStart.X; x < fillStart.X + width; x++)
            {
                for(int y = fillStart.Y; y < fillStart.Y + height; y++)
                {
                    effectiveX = x + settings.startingPoint.X;
                    effectiveY = y + settings.startingPoint.Y;

                    frequency = 1;
                    amplitude = 1;

                    for(int oct = 0; oct < settings.octaves; oct++)
                    {
                        toFill[x, y] += amplitude * SimpleNoise2D.GenInterpolatedNoise(effectiveX / (float)width * regionSize * frequency,
                                                                                      effectiveY / (float)width * regionSize * frequency, settings.seed);

                        frequency *= settings.frequencyMulti;
                        amplitude *= settings.persistence;
                    }
                }
            }
        }
    }
}

using System.Threading;
using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace GameTools.Noise3D
{
    static class PerlinNoise3D
    {
        public static double[, ,] GenPerlinNoise3D(PerlinNoiseSettings3D settings)
        {
            double[, ,] noise = new double[(int)settings.size.X, (int)settings.size.Y, (int)settings.size.Z];
            MakePerlinNoise3D(noise, Vector3.Zero, settings);

            return noise;
        }
        public static double[, ,] GenPerlinNoise3D(PerlinNoiseSettings3D settings, int numThreads)
        {
            double[, ,] noise = new double[(int)settings.size.X, (int)settings.size.Y, (int)settings.size.Z];

            int rowsPerThread = (int)settings.size.X / numThreads;
            int extraRows = (int)settings.size.X % numThreads;

            PerlinNoiseSettings3D threadSettings = new PerlinNoiseSettings3D(settings);

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

                noiseGenThreads[thread] = new Thread(() => MakePerlinNoise3D(noise, new Vector3(arrayStartX, 0, 0), threadSettings));
                noiseGenThreads[thread].Start();
            }

            for(int thread = 0; thread < numThreads; thread++)
            {
                noiseGenThreads[thread].Join();
            }

            return noise;
        }
        static void MakePerlinNoise3D(double[, ,] toFill, Vector3 fillStart, PerlinNoiseSettings3D settings)
        {
            int width;
            int height;
            int length;

            int effectiveX;
            int effectiveY;
            int effectiveZ;

            Vector3 regionSize = new Vector3();

            double frequency;
            double amplitude;

            width = (int)settings.size.X;
            height = (int)settings.size.Y;
            length = (int)settings.size.Z;

            regionSize.X = width / settings.zoom;
            regionSize.Y = width / settings.zoom;
            regionSize.Z = width / settings.zoom;

            for(int x = (int)fillStart.X; x < fillStart.X + width; x++)
            {
                for(int y = (int)fillStart.Y; y < fillStart.Y + height; y++)
                {
                    for(int z = (int)fillStart.Z; z < fillStart.Z + length; z++)
                    {
                        effectiveX = x + (int)settings.startingPoint.X;
                        effectiveY = y + (int)settings.startingPoint.Y;
                        effectiveZ = z + (int)settings.startingPoint.Z;

                        frequency = 1;
                        amplitude = 1;

                        for(int oct = 0; oct < settings.octaves; oct++)
                        {
                            double noise = SimpleNoise3D.GenInterpolatedNoise(effectiveX / settings.zoom * frequency,
                                                                              effectiveY / settings.zoom * frequency,
                                                                              effectiveZ / settings.zoom * frequency,
                                                                              settings.seed);
                            toFill[x, y, z] += amplitude * noise;

                            frequency *= settings.frequencyMulti;
                            amplitude *= settings.persistence;
                        }
                    }
                }
            }
        }
    }
}

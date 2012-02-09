using System;

using GameTools.Graph;

namespace GameTools.Noise2D
{
    class PerlinNoiseSettings2D
    {
        Random rng;

        public Vector2i size;
        public Vector2i startingPoint;
        public float frequencyMulti;
        public float persistence;
        public float zoom;
        public int octaves;
        public int seed;

        public PerlinNoiseSettings2D()
        {
            rng = new Random();

            size = new Vector2i(100, 100);
            startingPoint = Vector2i.Zero;

            frequencyMulti = 2;
            persistence = 0.5f;
            zoom = 40;
            octaves = 6;
            seed = 0;

            GenerateNewSeed();
        }

        public PerlinNoiseSettings2D(PerlinNoiseSettings2D settings)
        {
            size = new Vector2i(settings.size);
            startingPoint = new Vector2i(settings.startingPoint);

            frequencyMulti = settings.frequencyMulti;
            persistence = settings.persistence;
            zoom = settings.zoom;
            octaves = settings.octaves;
            seed = settings.seed;
        }

        public void GenerateNewSeed()
        {
            seed = rng.Next();
        }

    }
}

using System;

using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace GameTools.Noise2D
{
    class PerlinNoiseSettings2D
    {
        Random rng;

        public Vector2 size;
        public Vector2 startingPoint;
        public float frequencyMulti;
        public float persistence;
        public float zoom;
        public int octaves;
        public int seed;

        public PerlinNoiseSettings2D()
        {
            rng = new Random();

            size = new Vector2(100, 100);
            startingPoint = Vector2.Zero;

            frequencyMulti = 2;
            persistence = 0.5f;
            zoom = 40;
            octaves = 6;
            seed = 0;

            GenerateNewSeed();
        }

        public PerlinNoiseSettings2D(PerlinNoiseSettings2D settings)
        {
            size = settings.size;
            startingPoint = settings.startingPoint;

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

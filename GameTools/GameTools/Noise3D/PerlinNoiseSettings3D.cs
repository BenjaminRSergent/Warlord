using System;

using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace GameTools.Noise3D
{
    public class PerlinNoiseSettings3D
    {
        Random rng;

        public Vector3 size;
        public Vector3 startingPoint;
        public float frequencyMulti;
        public float persistence;
        public float zoom;
        public int octaves;
        public int seed;

        public PerlinNoiseSettings3D()
        {
            rng = new Random();

            size = new Vector3(100, 100, 100);
            startingPoint = Vector3.Zero;

            frequencyMulti = 2;
            persistence = 0.5f;
            zoom = 40;
            octaves = 6;
            seed = 0;

            GenerateNewSeed();
        }

        public PerlinNoiseSettings3D(PerlinNoiseSettings3D settings)
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

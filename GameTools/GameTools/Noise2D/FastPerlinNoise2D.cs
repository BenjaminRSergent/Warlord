using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace GameTools.Noise2D
{
    public class FastPerlinNoise2D
    {
        private const int premutationSize = 100;
        private float[] flatPremutationList;

        private Random rng;

        private Dictionary<Vector2, FastPerlinInterpolatedNoise2D> calcLookup;

        private PerlinNoiseSettings2D settings;

        public FastPerlinNoise2D(PerlinNoiseSettings2D settings)
        {
            this.settings = settings;
            rng = new Random(settings.seed);

            populatePremutations();
        }
        public void FillWithPerlinNoise2D(float[] toFill)
        {
            int width = (int)settings.size.X;
            int height = (int)settings.size.Y;

            int effectiveX;
            int effectiveY;

            calcLookup = new Dictionary<Vector2, FastPerlinInterpolatedNoise2D>();

            for(int x = 0; x < 0 + width; x++)
            {
                for(int y = 0; y < 0 + height; y++)
                {
                    effectiveX = x + (int)settings.startingPoint.X;
                    effectiveY = y + (int)settings.startingPoint.Y;

                    toFill[x * height + y] = GetPerlinNoise3D(effectiveX, effectiveY);
                }
            }
        }
        public float GetPerlinNoise3D(float x, float y)
        {
            float result;

            float frequency = 1;
            float amplitude = 1;

            x /= settings.zoom;
            y /= settings.zoom;

            result = 0;
            for(int oct = 0; oct < settings.octaves; oct++)
            {
                result += amplitude * GenInterpolatedNoise(x * frequency, y * frequency);

                frequency *= settings.frequencyMulti;
                amplitude *= settings.persistence;
            }

            return result;
        }
        private float GenInterpolatedNoise(float x, float y)
        {
            int floorX = (x > 0) ? (int)x : (int)x - 1;
            int floorY = (y > 0) ? (int)y : (int)y - 1;

            float fractionalX = x - floorX;
            float fractionalY = y - floorY;

            float centerInter, bottomInter;

            FastPerlinInterpolatedNoise2D noise;

            Vector2 key = new Vector2(floorX, floorY);

            calcLookup.TryGetValue(key, out noise);

            if(noise == null)
            {
                noise = new FastPerlinInterpolatedNoise2D();
                noise.center = GenSmoothNoise(floorX, floorY);
                noise.bottom = GenSmoothNoise(floorX, floorY + 1);
                noise.centerRight = GenSmoothNoise(floorX + 1, floorY);
                noise.bottomRight = GenSmoothNoise(floorX + 1, floorY + 1);

                calcLookup.Add(key, noise);
            }

            centerInter = GraphMath.LinearInterpolateFloat(noise.center, noise.centerRight, fractionalX);
            bottomInter = GraphMath.LinearInterpolateFloat(noise.bottom, noise.bottomRight, fractionalX);

            return GraphMath.LinearInterpolateFloat(centerInter, bottomInter, fractionalY);
        }
        private float GenSmoothNoise(int x, int y)
        {
            float center, sides;

            int adjustedX = x;
            int adjustedY = y;

            adjustedX = adjustedX % (premutationSize - 1);
            adjustedY = adjustedY % (premutationSize - 1);

            while(adjustedX < 1)
                adjustedX = premutationSize + adjustedX - 2;
            while(adjustedY < 1)
                adjustedY = premutationSize + adjustedY - 2;


            center = GetCenter(adjustedX, adjustedY);
            sides = GetSides(adjustedX, adjustedY);

            return sides + center;
        }

        private float GetCenter(int adjustedX, int adjustedY)
        {
            return TwoIndexIntoArray(adjustedX, adjustedY) / 2;
        }
        private float GetSides(int adjustedX, int adjustedY)
        {
            float right = TwoIndexIntoArray(adjustedX + 1, adjustedY);
            float left = TwoIndexIntoArray(adjustedX - 1, adjustedY);
            float up = TwoIndexIntoArray(adjustedX, adjustedY + 1);
            float down = TwoIndexIntoArray(adjustedX, adjustedY - 1);

            return (right + left + up + down) / 8;
        }
        private float TwoIndexIntoArray(int x, int y)
        {
            return flatPremutationList[x * premutationSize + y];
        }
        private void populatePremutations()
        {
            flatPremutationList = new float[premutationSize * premutationSize * premutationSize];
            int index;
            for(int x = 0; x < premutationSize; x++)
            {
                for(int y = 0; y < premutationSize; y++)
                {
                    index = x * premutationSize + y;
                    flatPremutationList[index] = SimpleNoise2D.GenFloatNoise(rng.Next(), rng.Next(), settings.seed);
                }
            }
        }
    }
}


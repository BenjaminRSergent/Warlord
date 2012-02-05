using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace GameTools.Noise3D
{
    class FastPerlinNoise
    {
        private const int premutationSize = 100;
        private Random rng;
        private double[] flatPremutationList;

        private Dictionary<Vector3, FastPerlinInterpolatedNoise3D> calcLookup;

        private PerlinNoiseSettings3D settings;

        public FastPerlinNoise(PerlinNoiseSettings3D settings)
        {            
            this.settings = settings;
            rng = new Random( settings.seed );

            populatePremutations( );
        }
        public void FillWithPerlinNoise3D(double[,,] toFill)
        {
            int width = settings.size.X;
            int height = settings.size.Y;
            int length = settings.size.Z;

            int effectiveX;
            int effectiveY;
            int effectiveZ;

            calcLookup = new Dictionary<Vector3,FastPerlinInterpolatedNoise3D>( premutationSize );

            for(int x = 0; x < 0 + width; x++)
            {
                for(int y = 0; y < 0 + height; y++)
                {
                    for(int z = 0; z < 0 + length; z++)
                    {
                        effectiveX = x + settings.startingPoint.X;
                        effectiveY = y + settings.startingPoint.Y;
                        effectiveZ = z + settings.startingPoint.Z;

                        toFill[x, y, z] = GetPerlinNoise3D(effectiveX, effectiveY, effectiveZ);
                    }
                }
            }
        }
        public double GetPerlinNoise3D(double x, double y, double z)
        {
            double result;

            double frequency = 1;
            double amplitude = 1;

            const int testZoomDivisor = 100;

            x /= testZoomDivisor;
            y /= testZoomDivisor;
            z /= testZoomDivisor;

            result = 0;
            for(int oct = 0; oct < settings.octaves; oct++)
            {
                result += GenInterpolatedNoise(x * frequency, y * frequency, z * frequency);

                frequency *= settings.frequencyMulti;
                amplitude *= settings.persistence;
            }

            return result;
        }
        private double GenInterpolatedNoise(double x, double y, double z)
        {
            int floorX = (x > 0) ? (int)x : (int)x - 1;
            int floorY = (y > 0) ? (int)y : (int)y - 1;
            int floorZ = (z > 0) ? (int)z : (int)z - 1;

            double fractionalX = x - floorX;
            double fractionalY = y - floorY;
            double fractionalZ = z - floorZ;

            double centerInter, bottomInter, belowInter, aboveInter;

            FastPerlinInterpolatedNoise3D noise = new FastPerlinInterpolatedNoise3D( );

            Vector3 key = new Vector3(floorX, floorY, floorZ);

            if(calcLookup.ContainsKey(key))
            {
                noise = calcLookup[key];
            }
            else
            {
                noise = new FastPerlinInterpolatedNoise3D();
                noise.center = GenSmoothNoise(floorX, floorY, floorZ);
                noise.bottom = GenSmoothNoise(floorX, floorY + 1, floorZ);
                noise.centerRight = GenSmoothNoise(floorX + 1, floorY, floorZ);
                noise.bottomRight = GenSmoothNoise(floorX + 1, floorY + 1, floorZ);

                noise.centerAbove = GenSmoothNoise(floorX, floorY, floorZ + 1);
                noise.bottomAbove = GenSmoothNoise(floorX, floorY + 1, floorZ + 1);
                noise.centerRightAbove = GenSmoothNoise(floorX + 1, floorY, floorZ + 1);
                noise.bottomRightAbove = GenSmoothNoise(floorX + 1, floorY + 1, floorZ + 1);

                calcLookup.Add(key, noise);
            }  

            centerInter = GraphMath.CosineInterpolate(noise.center, noise.centerRight, fractionalX);
            bottomInter = GraphMath.CosineInterpolate(noise.bottom, noise.bottomRight, fractionalX);
            belowInter = GraphMath.CosineInterpolate(centerInter, bottomInter, fractionalY);

            centerInter = GraphMath.CosineInterpolate(noise.centerAbove, noise.centerRightAbove, fractionalX);
            bottomInter = GraphMath.CosineInterpolate(noise.bottomAbove, noise.bottomRightAbove, fractionalX);
            aboveInter = GraphMath.CosineInterpolate(centerInter, bottomInter, fractionalY);

            return GraphMath.CosineInterpolate(belowInter, aboveInter, fractionalZ);
        }
        private double GenSmoothNoise(int x, int y, int z)
        {
            double corners, sides, center;

            int adjustedX = x;
            int adjustedY = y;
            int adjustedZ = z;

            // Offset based on division?
            adjustedX = adjustedX % (premutationSize - 1);
            adjustedY = adjustedY % (premutationSize - 1);
            adjustedZ = adjustedZ % (premutationSize - 1);

            // Offset based on division?
            while(adjustedX < 1)
                adjustedX = premutationSize + adjustedX - 2;
            while(adjustedY < 1)
                adjustedY = premutationSize + adjustedY - 2;
            while(adjustedZ < 1)
                adjustedZ = premutationSize + adjustedZ - 2;         


            center = GetCenter(adjustedX, adjustedY, adjustedZ);

            sides = GetSides(adjustedX, adjustedY, adjustedZ);

            corners = GetCorners(adjustedX, adjustedY, adjustedZ);

            return corners + sides + center;
        }       

        private double GetCenter(int adjustedX, int adjustedY, int adjustedZ)
        {
           return 3 * ThreeIndexIntoArray(adjustedX, adjustedY, adjustedZ);
        }        
        private double GetSides(int adjustedX, int adjustedY, int adjustedZ)
        {
            double right = ThreeIndexIntoArray(adjustedX + 1, adjustedY, adjustedZ);
            double left = ThreeIndexIntoArray(adjustedX - 1, adjustedY, adjustedZ);
            double up = ThreeIndexIntoArray(adjustedX, adjustedY + 1, adjustedZ);
            double down = ThreeIndexIntoArray(adjustedX, adjustedY - 1, adjustedZ);

            double forward = ThreeIndexIntoArray(adjustedX, adjustedY, adjustedZ + 1);
            double back = ThreeIndexIntoArray(adjustedX, adjustedY, adjustedZ - 1);

            return (right + left + up + down + forward + back) / 12.0;
        }
        private double GetCorners(int adjustedX, int adjustedY, int adjustedZ)
        {
            double rightUpForward = ThreeIndexIntoArray(adjustedX + 1, adjustedY + 1, adjustedZ + 1);
            double rightUpBack = ThreeIndexIntoArray(adjustedX + 1, adjustedY + 1, adjustedZ - 1);
            double rightDownForward = ThreeIndexIntoArray(adjustedX + 1, adjustedY - 1, adjustedZ + 1);
            double rightDownBack = ThreeIndexIntoArray(adjustedX + 1, adjustedY - 1, adjustedZ - 1);

            double leftUpForward = ThreeIndexIntoArray(adjustedX - 1, adjustedY + 1, adjustedZ + 1);
            double leftUpBack = ThreeIndexIntoArray(adjustedX - 1, adjustedY + 1, adjustedZ - 1);
            double leftDownForward = ThreeIndexIntoArray(adjustedX - 1, adjustedY - 1, adjustedZ + 1);
            double leftDownBack = ThreeIndexIntoArray(adjustedX - 1, adjustedY - 1, adjustedZ - 1);

            return (rightUpForward + rightUpBack + rightDownForward + rightDownBack +
                    leftUpForward + leftUpBack + leftDownForward + leftDownBack)/32;
        }

        private double ThreeIndexIntoArray(int x, int y, int z)
        {
            return flatPremutationList[x * premutationSize * premutationSize + y * premutationSize + z];
        }        
        private void populatePremutations()
        {
            flatPremutationList = new double[premutationSize * premutationSize * premutationSize];
            int index;
            for(int x = 0; x < premutationSize; x++)
            {
                for(int y = 0; y < premutationSize; y++)
                {
                    for(int z = 0; z < premutationSize; z++)
                    {
                        index = x * premutationSize * premutationSize + y * premutationSize + z;
                        flatPremutationList[index] = SimpleNoise3D.GenDoubleNoise(rng.Next(), rng.Next(), rng.Next(), settings.seed);
                    }
                }
            }
        }
    }
}


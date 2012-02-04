using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;

namespace GameTools.Noise3D
{
    class FastPerlinNoise
    {
        private const int premutationSize = 20;
        private Random rng;
        private double[,,] premutationList;

        private PerlinNoiseSettings3D settings;

        public FastPerlinNoise(PerlinNoiseSettings3D settings)
        {
            this.settings = settings;
            rng = new Random( settings.seed );

            populatePremutations( );
        }
        public void FillWithPerlinNoise3D( double[,,] toFill )
        {
            int width  = settings.size.X;  
            int height = settings.size.Y;
            int length = settings.size.Z;

            int effectiveX;
            int effectiveY;
            int effectiveZ;

            for( int x = 0; x < 0+width; x++ )
            {
                for( int y = 0; y < 0+height; y++ )
                {
                    for( int z = 0; z < 0+length; z++ )
                    {
                        effectiveX = x + settings.startingPoint.X;
                        effectiveY = y + settings.startingPoint.Y;
                        effectiveZ = z + settings.startingPoint.Z;

                        toFill[ x,y,z] = GetPerlinNoise3D( effectiveX, effectiveY, effectiveZ );
                    }
                }
            }
        }
        public double GetPerlinNoise3D( double x, double y, double z )
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
                result += GenInterpolatedNoise(x*frequency,y*frequency,z*frequency);

                frequency *= settings.frequencyMulti;
                amplitude *= settings.persistence;
            }

            return result;
        }
        private double GenSmoothNoise(int x, int y, int z)
        {
            double corners, sides, center;

            int adjustedX = x;
            int adjustedY = y;
            int adjustedZ = z;           

            // Offset based on division?
            adjustedX = adjustedX % (premutationSize-1);
            adjustedY = adjustedY % (premutationSize-1);
            adjustedZ = adjustedZ % (premutationSize-1);

            // Offset based on division?
            while( adjustedX < 1 )
                adjustedX = premutationSize + adjustedX - 2;
            while( adjustedY < 1 )
                adjustedY = premutationSize + adjustedY - 2;
            while( adjustedZ < 1 )
                adjustedZ = premutationSize + adjustedZ - 2;

            center = 3 * premutationList[adjustedX, adjustedY, adjustedZ] / 8.0;

            sides = (  premutationList[adjustedX + 1, adjustedY, adjustedZ] +
                       premutationList[adjustedX - 1, adjustedY, adjustedZ] +
                       premutationList[adjustedX, adjustedY + 1, adjustedZ] +
                       premutationList[adjustedX, adjustedY - 1, adjustedZ] +
                       premutationList[adjustedX, adjustedY, adjustedZ + 1] +
                       premutationList[adjustedX, adjustedY, adjustedZ - 1]) / 12.0;

            corners = (premutationList[adjustedX + 1, adjustedY + 1, adjustedZ + 1] +
                       premutationList[adjustedX + 1, adjustedY + 1, adjustedZ - 1] +
                       premutationList[adjustedX + 1, adjustedY - 1, adjustedZ + 1] +
                       premutationList[adjustedX + 1, adjustedY - 1, adjustedZ - 1] +
                       premutationList[adjustedX - 1, adjustedY + 1, adjustedZ + 1] +
                       premutationList[adjustedX - 1, adjustedY + 1, adjustedZ - 1] +
                       premutationList[adjustedX - 1, adjustedY - 1, adjustedZ + 1] +
                       premutationList[adjustedX - 1, adjustedY - 1, adjustedZ - 1]) / 64.0;

            return corners + sides + center;
        }

        public double GenInterpolatedNoise(double x, double y, double z)
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

            center = GenSmoothNoise(floorX, floorY, floorZ);
            bottom = GenSmoothNoise(floorX, floorY + 1, floorZ);
            centerRight = GenSmoothNoise(floorX + 1, floorY, floorZ);
            bottomRight = GenSmoothNoise(floorX + 1, floorY + 1, floorZ);

            centerAbove = GenSmoothNoise(floorX, floorY, floorZ + 1);
            bottomAbove = GenSmoothNoise(floorX, floorY + 1, floorZ + 1);
            centerRightAbove = GenSmoothNoise(floorX + 1, floorY, floorZ + 1);
            bottomRightAbove = GenSmoothNoise(floorX + 1, floorY + 1, floorZ + 1);

            centerInter = GraphMath.CosineInterpolate(center, centerRight, fractionalX);
            bottomInter = GraphMath.CosineInterpolate(bottom, bottomRight, fractionalX);
            belowInter = GraphMath.CosineInterpolate(centerInter, bottomInter, fractionalY);

            centerInter = GraphMath.CosineInterpolate(centerAbove, centerRightAbove, fractionalX);
            bottomInter = GraphMath.CosineInterpolate(bottomAbove, bottomRightAbove, fractionalX);
            aboveInter = GraphMath.CosineInterpolate(centerInter, bottomInter, fractionalY);

            return GraphMath.CosineInterpolate(belowInter, aboveInter, fractionalZ);
        }
        private void populatePremutations()
        {            
            premutationList = new double[premutationSize,premutationSize,premutationSize];

            for(int x = 0; x < premutationSize; x++)
            {
                for(int y = 0; y < premutationSize; y++)
                {
                    for(int z = 0; z < premutationSize; z++)
                    {
                        premutationList[x,y,z] = SimpleNoise3D.GenDoubleNoise( rng.Next( ), rng.Next( ), rng.Next( ), settings.seed );
                    }
                }
            }
        }
    }
}


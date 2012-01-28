using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameTools.Noise2D;
using Microsoft.Xna.Framework;
using GameTools.Graph;
using Warlord.Logic.Data;

namespace Warlord.Logic
{
    class BasicFiniteWorldGenerator : FiniteWorldGenerator
    {
        public FiniteWorld GetDefaultWorld()
        {
            throw new NotImplementedException();
        }

        public FiniteWorld GetDebugWorld()
        {
            throw new NotImplementedException();
        }

        public FiniteWorld GetSimpleWorld( Vector2i worldSize, Vector3i RegionSize )
        {
            double[,] noise;
            PerlinNoiseSettings2D settings = new PerlinNoiseSettings2D( );

            FiniteWorld world = new FiniteWorld( worldSize, RegionSize );

            int height;

            settings.frequencyMulti = 2;
            settings.octaves = 6;
            settings.persistence = 0.5;
            settings.seed = 1;
            settings.size = new Point( worldSize.X*RegionSize.X, worldSize.Y*RegionSize.Z );
            settings.startingPoint = Point.Zero;
            settings.zoom = 300;            
            
            noise = PerlinNoise2D.GenPerlinNoise2D( settings, 4 );

            for( int x = 0; x < settings.size.X-1; x++ )
            {
                for( int z = 0; z < settings.size.Y-1; z++ )
                {
                    height = 64 + (int)(32*noise[x,z]);

                    height = Math.Max( height, 1 );

                    for( int y = 0; y < height/2; y++ )
                    {
                        world.AddBlock( new Vector3i(x,y,z), BlockType.Stone );
                    }

                    for( int y = height/2; y < height-1; y++ )
                    {
                        world.AddBlock( new Vector3i(x,y,z), BlockType.Dirt );
                    }

                    world.AddBlock( new Vector3i( x, height-1, z ), BlockType.Grass );
                }
            }

            return world;
        }
    }
}

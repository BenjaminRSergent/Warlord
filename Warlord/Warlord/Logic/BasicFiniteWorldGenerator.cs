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

        public FiniteWorld GetSimpleWorld()
        {
            double[,] noise;
            PerlinNoiseSettings2D settings = new PerlinNoiseSettings2D( );

            FiniteWorld world = new FiniteWorld( new Vector2i(8,8), new Vector3i(16,128,16) );

            int height;

            settings.frequencyMulti = 2;
            settings.octaves = 4;
            settings.persistence = 0.5;
            settings.seed = 1;
            settings.size = new Point( 128, 128 );
            settings.startingPoint = Point.Zero;
            settings.zoom = 200;            
            
            noise = PerlinNoise2D.GenPerlinNoise2D( settings, 1 );

            for( int x = 0; x < settings.size.X-1; x++ )
            {
                for( int z = 0; z < settings.size.Y-1; z++ )
                {
                    height = 64 + (int)(64*noise[x,z]);

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

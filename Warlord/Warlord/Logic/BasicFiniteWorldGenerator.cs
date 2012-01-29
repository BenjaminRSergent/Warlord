using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameTools.Noise2D;
using Microsoft.Xna.Framework;
using GameTools.Graph;
using Warlord.Logic.Data;
using GameTools.Noise3D;

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

        public FiniteWorld GetSimpleWorldWith2D( Vector2i worldSize, Vector3i RegionSize )
        {
            double[,] noise;
            PerlinNoiseSettings2D settings = new PerlinNoiseSettings2D( );

            FiniteWorld world = new FiniteWorld( worldSize, RegionSize );

            int height;

            settings.frequencyMulti = 2.2;
            settings.octaves = 8;
            settings.persistence = 0.75;
            settings.size = new Point( worldSize.X*RegionSize.X, worldSize.Y*RegionSize.Z );
            settings.startingPoint = Point.Zero;
            settings.zoom = 5*RegionSize.X;            
            
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

        public FiniteWorld GetSimpleWorldWith3D( Vector2i worldSize, Vector3i RegionSize )
        {
            double[,,] noise;
            PerlinNoiseSettings3D settings = new PerlinNoiseSettings3D( );

            FiniteWorld world = new FiniteWorld( worldSize, RegionSize );

            settings.frequencyMulti = 2;
            settings.octaves = 6;
            settings.persistence = 0.5;
            settings.seed = 3;
            settings.size = new Vector3i( worldSize.X*RegionSize.X, RegionSize.Y,worldSize.Y*RegionSize.Z );
            settings.startingPoint = Vector3i.Zero;
            settings.zoom = 160;   

            double density;

            noise = PerlinNoise3D.GenPerlinNoise3D( settings, 4 );

            BlockType blockType;
            for( int x = 0; x < settings.size.X-1; x++ )
            {
                for( int y = 0; y < settings.size.Y-1; y++ )
                {
                    for( int z = 0; z < settings.size.Z-1; z++ )
                    {
                        density = noise[x,y,z];
                        blockType = GetBlockFromNoiseTwo(density, y, 2*settings.size.Y / 7, 7 * settings.size.Y / 8);

                        if (blockType != BlockType.Air)
                            world.AddBlock(new Vector3i(x, y, z), blockType);
                    }
                }
            }

            AddGrassToTop( world, new Vector3i( worldSize.X * RegionSize.X,
                                                RegionSize.Y,
                                                worldSize.Y * RegionSize.Z) );

            return world;
        }
        public void AddGrassToTop( FiniteWorld world, Vector3i volume )
        {
            Vector3i currentPosition;
            Block currentBlock;

            bool recentAir;

            recentAir = false;
            for( int x = 0; x < volume.X-1; x++ )
            {
                for( int z = 0; z < volume.Z-1; z++ )
                {
                    for( int y = volume.Y - 1; y > 0; y-- )
                    {
                        currentPosition = new Vector3i(x,y,z);

                        currentBlock = world.GetBlock( currentPosition );

                        if( currentBlock.Type != BlockType.Air && recentAir )
                        {
                            world.AddBlock( currentPosition, BlockType.Grass );
                            recentAir = false;
                        }
                        else if( currentBlock.Type == BlockType.Air )
                            recentAir = true;
                    }
                }
            }        
        }
        public BlockType GetBlockFromNoiseOne( double noise, int height )
        {
            double effectiveNoise = (noise + 1)/2;

            if( height < 32 )
            {
                if( effectiveNoise > 0.2 )
                    return BlockType.Stone;
                else if( effectiveNoise > 0.05 )
                    return BlockType.Dirt;
                else
                    return BlockType.Air;
            }
            else
            {
                if( effectiveNoise > 0.4+(height/320.0) )
                    return BlockType.Stone;
                else if( effectiveNoise > 0.35+((height-64)/320.0) )
                    return BlockType.Dirt;
                else
                    return BlockType.Air;
            }
        }
        public BlockType GetBlockFromNoiseTwo(double noise, int height, int DirtLevel, int AirLevel)
        {
            float heightMod;

            if (height < DirtLevel)
            {
                heightMod = -(height / (float)DirtLevel);
                if (noise + heightMod > 0)
                    return BlockType.Stone;
                else if(noise + heightMod < -0.3)
                    return BlockType.Dirt;
                else
                    return BlockType.Air;
            }
            else if(height < AirLevel)
            {
                heightMod = -(height - DirtLevel) / (float)(AirLevel - DirtLevel);

                if(noise + heightMod > 1)
                    return BlockType.Stone;
                else if(noise + heightMod > -0.1)
                    return BlockType.Dirt;
                else
                    return BlockType.Air;
            }
            else
            {
                heightMod = -(height - AirLevel) / (float)AirLevel;

                if(noise > 0.6)
                    return BlockType.Dirt;
                else
                    return BlockType.Air;
            }
        }
    }
}

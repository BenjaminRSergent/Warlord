using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using GameTools.Noise3D;

namespace Warlord.Logic.Data.World
{
    class RegionGenerator
    {
        private int seed;
        private GeneratorSettings generatorSettings;
        private PerlinNoiseSettings3D noiseSettings;

        public RegionGenerator( int seed )
        {
            this.seed = seed;
            LoadDefaultSettings( );
        }
        public RegionGenerator( int seed, Vector3i regionSize )
        {
            this.seed = seed;
            LoadDefaultSettings( );
            generatorSettings.RegionSize = regionSize;            
        }
        private void LoadDefaultSettings()
        {
            generatorSettings = new GeneratorSettings( );
            generatorSettings.RegionSize = new Vector3i( 16, 128, 16 );
            generatorSettings.DirtLevel = 32;
            generatorSettings.AirLevel = 110;

            noiseSettings = new PerlinNoiseSettings3D( );
            noiseSettings.frequencyMulti = 2.2;
            noiseSettings.octaves = 6;
            noiseSettings.persistence = 0.5;
            noiseSettings.seed = 3;
            noiseSettings.size = generatorSettings.RegionSize;
            noiseSettings.startingPoint = Vector3i.Zero;
            noiseSettings.zoom = 120;   
        }
        public RegionGenerator( int seed, PerlinNoiseSettings3D noiseSettings, GeneratorSettings generatorSettings )
        {
            this.seed = seed;
            this.noiseSettings = noiseSettings;
            this.generatorSettings = generatorSettings;

            noiseSettings.seed = seed;
        }

        public void GenerateRegion( RegionUpdater ownerWorld, Vector3i origin )
        {
            double[,,] noise;

            noiseSettings.startingPoint = origin;              

            noise = PerlinNoise3D.GenPerlinNoise3D( noiseSettings, 2 );

            PlaceBlocks(ownerWorld, origin, noise);

            AddGrassToTop( ownerWorld, origin );
        }

        private void PlaceBlocks(RegionUpdater ownerWorld, Vector3i origin, double[,,] noise)
        {
            double density;
            BlockType blockType;
            for(int x = 0; x < generatorSettings.RegionSize.X; x++)
            {
                for(int y = 0; y < generatorSettings.RegionSize.Y-1; y++)
                {
                    for(int z = 0; z < generatorSettings.RegionSize.Z; z++)
                    {
                        density = noise[x, y, z];
                        blockType = GetBlockFromNoise(density, y);
                        if(blockType != BlockType.Air)
                            ownerWorld.ChangeBlock(origin + new Vector3i(x, y, z), blockType);
                    }
                }
            }
        }
        public void AddGrassToTop( RegionUpdater ownerWorld, Vector3i origin )
        {
            Vector3i currentPosition;
            Block currentBlock;

            bool recentAir;

            recentAir = false;
            for( int x = 0; x < generatorSettings.RegionSize.X; x++ )
            {
                for( int z = 0; z < generatorSettings.RegionSize.Z; z++ )
                {
                    for( int y = generatorSettings.RegionSize.Y - 1; y > 0; y-- )
                    {
                        currentPosition = origin + new Vector3i(x,y,z);

                        currentBlock = ownerWorld.GetBlock( currentPosition );

                        if( currentBlock.Type != BlockType.Air && recentAir )
                        {
                            ownerWorld.ChangeBlock( currentPosition, BlockType.Grass );
                            recentAir = false;
                        }
                        else if( currentBlock.Type == BlockType.Air )
                            recentAir = true;
                    }
                }
            }        
        }
        public BlockType GetBlockFromNoise(double noise, int height)
        {           
            if (height < generatorSettings.DirtLevel)
            {
                return StoneLevelBlock( noise, height );
            }
            else if(height < generatorSettings.AirLevel)
            {
                return DirtLevelBlock( noise, height );
            }
            else
            {
                return AirLevelBlock( noise, height );
            }
        }
        public BlockType StoneLevelBlock(double noise, int height)
        {
            float heightMod = -(height / (float)generatorSettings.DirtLevel);
            if (noise + heightMod > 0)
                return BlockType.Stone;
            else
                return BlockType.Dirt;
        }
        public BlockType DirtLevelBlock(double noise, int height)
        {
            float heightMod = -(height - generatorSettings.DirtLevel) / (float)(generatorSettings.AirLevel - generatorSettings.DirtLevel);

            if(noise + heightMod > 1)
                return BlockType.Stone;
            else if(noise + heightMod > -0.3)
                return BlockType.Dirt;
            else
                return BlockType.Air;            
        }        
        public BlockType AirLevelBlock(double noise, int height)
        {
            float heightMod = -(height - generatorSettings.AirLevel) / (float)generatorSettings.AirLevel;

            if(noise + heightMod > 0.6)
                return BlockType.Dirt;
            else
                return BlockType.Air;
        }
        public int Seed
        {
            get { return seed; }
        }        
    }
}

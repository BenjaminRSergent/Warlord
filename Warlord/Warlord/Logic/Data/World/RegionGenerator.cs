using System;
using System.Diagnostics;
using GameTools.Graph;
using GameTools.Noise3D;
using GameTools.Noise2D;

namespace Warlord.Logic.Data.World
{
    class RegionGenerator
    {
        private int seed;
        private GeneratorSettings generatorSettings;
        private PerlinNoiseSettings3D noiseSettings3D;
        private FastPerlinNoise3D fastNoise3D;

        private float[] flatNoise;

        public RegionGenerator(int seed)
        {
            this.seed = seed;
            LoadDefaultSettings();
        }
        public RegionGenerator(int seed, Vector3i regionSize)
        {
            this.seed = seed;
            generatorSettings = new GeneratorSettings();
            generatorSettings.RegionSize = regionSize;
            LoadDefaultSettings();

            flatNoise = new float[generatorSettings.RegionSize.X *
                                  generatorSettings.RegionSize.Y *
                                  generatorSettings.RegionSize.Z];
        }
        private void LoadDefaultSettings()
        {
            noiseSettings3D = new PerlinNoiseSettings3D();
            noiseSettings3D.frequencyMulti = 2.0f;
            noiseSettings3D.octaves = 4;
            noiseSettings3D.persistence = 0.5f;
            noiseSettings3D.seed = 3;
            noiseSettings3D.size = generatorSettings.RegionSize;
            noiseSettings3D.startingPoint = Vector3i.Zero;
            noiseSettings3D.zoom = 60;

            fastNoise3D = new FastPerlinNoise3D(noiseSettings3D);

            generatorSettings.midLevel = generatorSettings.RegionSize.Y / 10;
            generatorSettings.highLevel = (6 * generatorSettings.RegionSize.Y) / 7;

            generatorSettings.lowLevelZone = new ZoneBlockSettings(-1, -0.7f, GetDefaultHeightMod(0, generatorSettings.midLevel));
            generatorSettings.midLevelZone = new ZoneBlockSettings(0.1f, 1, GetDefaultHeightMod(generatorSettings.midLevel, generatorSettings.highLevel));
            generatorSettings.highLevelZone = new ZoneBlockSettings(0.6f, 1, GetDefaultHeightMod(generatorSettings.highLevel, generatorSettings.RegionSize.Y));
        }
        private ZoneBlockSettings.ModifyDensity GetDefaultHeightMod(int heightZoneStart, int heightZoneEnd)
        {
            return ((float density, float height) =>
                density - ((height - heightZoneStart) / (heightZoneEnd - heightZoneStart)) / 2);
        }
        public void FastGenerateRegion3D(RegionController ownerWorld, Vector3i origin)
        {
            noiseSettings3D.startingPoint = origin;

            fastNoise3D.FillWithPerlinNoise3D(flatNoise);
            PlaceBlocks(ownerWorld, origin, flatNoise);

            AddGrassToTop(ownerWorld, origin);
        }
        public void FakeGenerator(RegionController ownerWorld, Vector3i origin)
        {
            float density;
            BlockType blockType;
            for(int x = 0; x < generatorSettings.RegionSize.X; x++)
            {
                for(int y = 0; y < generatorSettings.RegionSize.Y - 1; y++)
                {
                    for(int z = 0; z < generatorSettings.RegionSize.Z; z++)
                    {
                        density = 0;
                        blockType = GetBlockFromNoise(density, y);
                        if(blockType != BlockType.Air)
                            ownerWorld.ChangeBlock(origin + new Vector3i(x, y, z), blockType);
                    }
                }
            }
        }
        private void PlaceBlocks(RegionController ownerWorld, Vector3i origin, float[] noise)
        {
            float density;
            BlockType blockType;
            for(int x = 0; x < generatorSettings.RegionSize.X; x++)
            {
                for(int y = 0; y < generatorSettings.RegionSize.Y - 1; y++)
                {
                    for(int z = 0; z < generatorSettings.RegionSize.Z; z++)
                    {
                        density = noise[x * generatorSettings.RegionSize.Y * generatorSettings.RegionSize.Z +
                                        y * generatorSettings.RegionSize.Z +
                                        z];

                        blockType = GetBlockFromNoise(density, y);
                        if(blockType != BlockType.Air)
                            ownerWorld.ChangeBlock(origin + new Vector3i(x, y, z), blockType);
                    }
                }
            }
        }
        public void AddGrassToTop(RegionController ownerWorld, Vector3i origin)
        {
            Vector3i currentPosition;
            Block currentBlock;

            bool recentAir;

            recentAir = false;
            for(int x = 0; x < generatorSettings.RegionSize.X; x++)
            {
                for(int z = 0; z < generatorSettings.RegionSize.Z; z++)
                {
                    for(int y = generatorSettings.RegionSize.Y - 1; y > 0; y--)
                    {
                        currentPosition = origin + new Vector3i(x, y, z);

                        currentBlock = ownerWorld.GetBlock(currentPosition);

                        if(currentBlock.Type != BlockType.Air && recentAir)
                        {
                            ownerWorld.ChangeBlock(currentPosition, BlockType.Grass);
                            recentAir = false;
                        }
                        else if(currentBlock.Type == BlockType.Air)
                            recentAir = true;
                    }
                }
            }
        }
        public BlockType GetBlockFromNoise(float noise, int height)
        {
            Debug.Assert(Math.Abs(noise) <= 1.1);

            if(height < generatorSettings.midLevel)
                return generatorSettings.lowLevelZone.GetBlockFromDensity(noise, height);

            if(height < generatorSettings.highLevel)
                return generatorSettings.midLevelZone.GetBlockFromDensity(noise, height);

            return generatorSettings.highLevelZone.GetBlockFromDensity(noise, height);
        }

        public int Seed
        {
            get { return seed; }
        }
    }
}

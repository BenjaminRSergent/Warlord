using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using System.Threading;
using Warlord.GameTools;
using Warlord.Event;

namespace Warlord.Logic.Data.World
{
    class RegionDatabase
    {
        Vector3i regionSize;
        RegionGenerator generator;

        HashSet<Vector2i> createdWorlds;
        Dictionary<Vector2i, Region> regionMap;
        
        int seed;

        bool generating;

        public RegionDatabase(int seed, Vector3i regionSize)
        {
            createdWorlds = new HashSet<Vector2i>();
            regionMap = new Dictionary<Vector2i, Region>();
            generating = false;

            this.seed = seed;
            this.regionSize = regionSize;
            generator = new RegionGenerator(seed, regionSize);
        }
        public bool CreateRegion(RegionUpdater updater, Vector2i regionCoordiants)
        {
            if(!regionMap.Keys.Contains(regionCoordiants))
            { 
                generating = true;
                Vector3i newOrigin = new Vector3i(regionCoordiants.X*regionSize.X, 0, regionCoordiants.Y*regionSize.Z);
                regionMap.Add(regionCoordiants, new Region(newOrigin,  regionSize));
                //generator.FastGenerateRegion(updater, newOrigin );
                generator.FakeGenerator( updater, newOrigin );
                generating = false;

                GlobalApplication.Application.GameEventManager.SendEvent( new GameEvent( new Optional<object>(this),
                                                                          "region_added",
                                                                          regionMap[regionCoordiants],
                                                                          0 ) );

                return true;
            }

            return false;
        }
        public void UnloadRegion(Vector2i regionCoordiants)
        {
            if(regionMap.Keys.Contains(regionCoordiants))
            {                
                GlobalApplication.Application.GameEventManager.SendEvent( new GameEvent( new Optional<object>(this),
                                                                          "region_removed",
                                                                          regionMap[regionCoordiants],
                                                                          0 ) );

                regionMap.Remove(regionCoordiants);
            }
        }
        public void ChangeBlock(Vector3i absolutePosition, BlockType type)
        {
            Optional<Region> currentRegion = GetRegionFromAbsolute(absolutePosition);            

            if(currentRegion.Valid)
            {
                Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.Data.RegionOrigin;

                currentRegion.Data.AddBlock(currentBlockRelativePosition, type);

                Block theBlock = currentRegion.Data.GetBlock(currentBlockRelativePosition);

                GlobalApplication.Application.GameEventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this),
                                                                         "block_added",
                                                                         new KeyValuePair<Region, Block>(currentRegion.Data, theBlock),
                                                                         0));
            }
            else
            {
                GlobalApplication.Application.ReportError( "The region at absolute position " + absolutePosition +
                                                           " does not exist and something tried to change one of its block from it");
                throw new NullReferenceException("Invalid operation on a null region");
            }
        }        
        public Block GetBlock(Vector3i absolutePosition)
        {
            Optional<Region> currentRegion = GetRegionFromAbsolute(absolutePosition);
            if(currentRegion.Valid)
            {
                Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.Data.RegionOrigin;
                return currentRegion.Data.GetBlock(currentBlockRelativePosition);
            }
            else
            {
                GlobalApplication.Application.ReportError( "The region at absolute position " + absolutePosition +
                                                           " does not exist and something tried to ask for a block from it");
                throw new NullReferenceException("Invalid operation on a null region");
            }            
        }
        public void ChangeFacing(Vector3i absolutePosition, BlockFaceField facing, bool active)
        {
            Optional<Region> region = GetRegionFromAbsolute(absolutePosition);
            if(region.Valid)
            {
                Vector3i relativePosition = absolutePosition - region.Data.RegionOrigin;
                if(active)
                    region.Data.AddFace(relativePosition, facing);
                else
                    region.Data.RemoveFace(relativePosition, facing);
            }
            else
            {
                GlobalApplication.Application.ReportError( "The region at absolute position " + absolutePosition +
                                                           " does not exist and something tried to change it's facing");
            }
            
        }
        public Optional<Region> GetRegionFromAbsolute(Vector3i absolutePosition)
        {
            Vector2i coordinates = GetRegionCoordiantes(absolutePosition);
            return GetRegionFromCoordiantes(coordinates);

        }
        public Optional<Region> GetRegionFromCoordiantes(Vector2i coordiantes)
        {
            if(regionMap.Keys.Contains(coordiantes))
                return new Optional<Region>(regionMap[coordiantes]);
            else
                return new Optional<Region>();
        }
        public Vector2i GetRegionCoordiantes(Vector3i absolutePosition)
        {
            Vector2i regionCoordinates = new Vector2i(absolutePosition.X, absolutePosition.Z);

            if(absolutePosition.X > 0)
                regionCoordinates.X /= regionSize.X;
            else
            {
                double doubleX = regionCoordinates.X / (double)regionSize.X;
                regionCoordinates.X = (int)Math.Floor(doubleX);
            }
            if(absolutePosition.Z > 0)
                regionCoordinates.Y /= regionSize.Z;
            else
            {
                double doubleZ = regionCoordinates.Y / (double)regionSize.Z;
                regionCoordinates.Y = (int)Math.Floor(doubleZ);
            }

            return regionCoordinates;
        }
        public Vector3i RegionSize
        {
            get { return regionSize; }
        }

        public bool Generating
        {
            get { return generating; }
        }
        public Dictionary<Vector2i, Region> RegionMap
        {
            get { return regionMap; }
        }
    }
}

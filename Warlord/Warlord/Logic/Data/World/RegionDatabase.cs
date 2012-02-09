using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameTools.Graph;
using Warlord.Application;
using Warlord.Event.EventTypes;
using Warlord.GameTools;
using Warlord.Event;

namespace Warlord.Logic.Data.World
{
    class RegionDatabase
    {
        Vector3i regionSize;
        RegionGenerator generator;
        Stack<Region> RegionPool;

        Dictionary<Vector3i, Region> regionMap;

        int seed;

        public RegionDatabase(int seed, Vector3i regionSize)
        {
            Debug.Assert(regionSize.X > 0 && regionSize.Y > 0 && regionSize.Z > 0);

            regionMap = new Dictionary<Vector3i, Region>();
            RegionPool = new Stack<Region>();
            generator = new RegionGenerator(seed, regionSize);

            this.seed = seed;
            this.regionSize = regionSize;            

            GlobalSystems.EventManager.Subscribe(SendCurrentRegions, "refresh_region_graphics");
        }
        public bool CreateRegion(RegionController updater, Vector3i regionCoordiants)
        {
            if(!regionMap.Keys.Contains(regionCoordiants))
            {
                Vector3i newOrigin = new Vector3i(regionCoordiants.X * regionSize.X,
                                                  regionCoordiants.Y * regionSize.Y,
                                                  regionCoordiants.Z * regionSize.Z);

                Region newRegion = GetNewRegion(newOrigin, regionSize);
                regionMap.Add(regionCoordiants, newRegion);
                generator.FastGenerateRegion(updater, newOrigin);

                RegionCreatedData creationData = new RegionCreatedData(newRegion);

                GlobalSystems.EventManager.SendEvent(new RegionCreatedEvent(new Optional<object>(this),
                                                      0,
                                                      creationData));

                return true;
            }

            return false;
        }

        private Region GetNewRegion(Vector3i newOrigin, Vector3i regionSize)
        {
            Region newRegion;

            if(RegionPool.Count == 0)
                newRegion = new Region(newOrigin, regionSize);
            else
            {
                newRegion = RegionPool.Pop();
                newRegion.Reinit(newOrigin, regionSize);
            }

            return newRegion;
        }
        public void UnloadRegion(Vector3i regionCoordiants)
        {
            if(regionMap.Keys.Contains(regionCoordiants))
            {
                Region theRegion = regionMap[regionCoordiants];
                GlobalSystems.EventManager.SendEvent(new RegionRemovedEvent(new Optional<object>(this),
                                                     0,
                                                     theRegion));

                regionMap.Remove(regionCoordiants);

                theRegion.Deactivate();
                RegionPool.Push(theRegion);
            }
        }
        public void ChangeBlock(Vector3i absolutePosition, BlockType type)
        {
            Optional<Region> currentRegion = GetRegionFromAbsolute(absolutePosition);

            Debug.Assert(currentRegion.Valid);

            Vector3i currentBlockRelativePosition = Transformation.AbsoluteToRelative(absolutePosition,
                                                                                      currentRegion.Data.RegionOrigin);

            Block oldBlock = currentRegion.Data.GetBlock(currentBlockRelativePosition);
            currentRegion.Data.AddBlock(currentBlockRelativePosition, type);
            Block newBlock = currentRegion.Data.GetBlock(currentBlockRelativePosition);

            BlockChangedData blockChangedData = new BlockChangedData(oldBlock, newBlock);

            GlobalSystems.EventManager.SendEvent(new BlockChangedEvent(new GameTools.Optional<object>(this),
                                                                       0,
                                                                       blockChangedData));
        }
        public Block GetBlock(Vector3i absolutePosition)
        {
            Optional<Region> currentRegion = GetRegionFromAbsolute(absolutePosition);

            Debug.Assert(currentRegion.Valid);

            Vector3i currentBlockRelativePosition = Transformation.AbsoluteToRelative(absolutePosition,
                                                                                      currentRegion.Data.RegionOrigin);

            return currentRegion.Data.GetBlock(currentBlockRelativePosition);

        }
        public void ChangeFacing(Vector3i absolutePosition, BlockFaceField facing, bool activate)
        {
            Optional<Region> currentRegion = GetRegionFromAbsolute(absolutePosition);

            Debug.Assert(currentRegion.Valid);

            Vector3i relativePosition = Transformation.AbsoluteToRelative(absolutePosition,
                                                                          currentRegion.Data.RegionOrigin);

            if(activate)
                currentRegion.Data.AddFace(relativePosition, facing);
            else
                currentRegion.Data.RemoveFace(relativePosition, facing);

        }
        public bool IsRegionLoaded( Vector3i regionCoordiantes )
        {
            return regionMap.ContainsKey(regionCoordiantes);
        }
        public Optional<Region> GetRegionFromAbsolute(Vector3i absolutePosition)
        {
            Vector3i coordinates = GetRegionCoordiantes(absolutePosition);
            return GetRegionFromCoordiantes(coordinates);
        }
        public Optional<Region> GetRegionFromCoordiantes(Vector3i coordiantes)
        {
            if(regionMap.Keys.Contains(coordiantes))
                return new Optional<Region>(regionMap[coordiantes]);
            else
                return new Optional<Region>();
        }
        public Vector3i GetRegionCoordiantes(Vector3i absolutePosition)
        {
            return Transformation.ChangeVectorScale(absolutePosition, regionSize);
        }

        public void SendCurrentRegions(BaseGameEvent theEvent)
        {
            SendingRegionListEvent sendingRegionsEvent = new SendingRegionListEvent( new Optional<object>(this),
                                                                                     0,
                                                                                     regionMap.Values.ToList( ));
        }

        public Vector3i RegionSize
        {
            get { return regionSize; }
        }

        internal IEnumerable<Vector3i> GetCoordiantesOfLoadedRegions()
        {
            return regionMap.Keys;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using System.Threading;
using Warlord.GameTools;
using Warlord.Event;
using System.IO;
using Warlord.Event.EventTypes;
using System.Diagnostics;
using Warlord.Application;

namespace Warlord.Logic.Data.World
{
    class RegionDatabase
    {
        Vector3i regionSize;
        RegionGenerator generator;
        Stack<Region> RegionPool;

        HashSet<Vector2i> createdWorlds;
        Dictionary<Vector2i, Region> regionMap;

        int seed;

        bool generating;

        public RegionDatabase(int seed, Vector3i regionSize)
        {
            Debug.Assert(regionSize.X > 0 && regionSize.Y > 0 && regionSize.Z > 0);

            createdWorlds = new HashSet<Vector2i>();
            regionMap = new Dictionary<Vector2i, Region>();
            generating = false;

            RegionPool = new Stack<Region>();

            this.seed = seed;
            this.regionSize = regionSize;
            generator = new RegionGenerator(seed, regionSize);
        }
        public bool CreateRegion(RegionUpdater updater, Vector2i regionCoordiants)
        {
            if(!regionMap.Keys.Contains(regionCoordiants))
            {
                generating = true;
                Vector3i newOrigin = new Vector3i(regionCoordiants.X * regionSize.X, 0, regionCoordiants.Y * regionSize.Z);
                Region newRegion = GetNewRegion(newOrigin, regionSize);
                regionMap.Add(regionCoordiants, newRegion);
                generator.FastGenerateRegion(updater, newOrigin);
                generating = false;

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
        public void UnloadRegion(Vector2i regionCoordiants)
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

            Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.Data.RegionOrigin;

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

            Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.Data.RegionOrigin;
            return currentRegion.Data.GetBlock(currentBlockRelativePosition);

        }
        public void ChangeFacing(Vector3i absolutePosition, BlockFaceField facing, bool active)
        {
            Optional<Region> currentRegion = GetRegionFromAbsolute(absolutePosition);

            Debug.Assert(currentRegion.Valid);

            Vector3i relativePosition = absolutePosition - currentRegion.Data.RegionOrigin;
            if(active)
                currentRegion.Data.AddFace(relativePosition, facing);
            else
                currentRegion.Data.RemoveFace(relativePosition, facing);

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
        public void Save(BinaryWriter outStream)
        {
            outStream.Write(regionMap.Count);
            foreach(Vector2i region in regionMap.Keys)
            {
                outStream.Write(region.X);
                outStream.Write(region.Y);
                regionMap[region].Save(outStream);
            }
        }
        public void Load(BinaryReader inStream)
        {
            Vector2i[] keys = regionMap.Keys.ToArray();
            foreach(Vector2i regionCo in keys)
                UnloadRegion(regionCo);

            RegionPool.Clear();

            int numRegions = inStream.ReadInt32();
            Vector2i position;
            Region region;
            for(int k = 0; k < numRegions; k++)
            {
                position = new Vector2i();
                position.X = inStream.ReadInt32();
                position.Y = inStream.ReadInt32();

                region = new Region(new Vector3i(position.X * 16, 0, position.Y * 16), regionSize);
                region.Load(inStream);
                regionMap.Add(position, region);

                RegionCreatedData creationData = new RegionCreatedData(region);

                GlobalSystems.EventManager.SendEvent(new RegionCreatedEvent(new Optional<object>(this),
                                                                              0,
                                                                              creationData));
            }
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


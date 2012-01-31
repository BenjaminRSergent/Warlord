﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Warlord.Event;
using Microsoft.Xna.Framework;
using System.Threading;
using Warlord.GameTools;

namespace Warlord.Logic.Data.World
{
    class InfiniteWorld : WorldBase
    {
        Vector3i regionSize;
        RegionGenerator generator;

        volatile HashSet<Vector2i> createdWorlds;
        volatile Dictionary<Vector2i, Region> regionMap;

        Thread createThread;
        Thread unloadThread;

        int seed;
        int existenceRadius;

        bool generating;

        public InfiniteWorld(int seed, int existenceRadius, Vector3i regionSize)
        {
            createdWorlds = new HashSet<Vector2i>();
            regionMap = new Dictionary<Vector2i, Region>();

            createdWorlds = new HashSet<Vector2i>(new BasicComparer<Vector2i>());

            createThread = new Thread(() => { });
            createThread.IsBackground = true;
            unloadThread = new Thread(() => { });
            unloadThread.IsBackground = true;

            this.seed = seed;
            this.existenceRadius = existenceRadius;
            this.regionSize = regionSize;
            generator = new RegionGenerator(seed, regionSize);
        }
        public void Initalize()
        {
            Update(new Vector3f(0, 0, 0));
        }
        public void Update(Vector3f playerPosition)
        {
            Vector2i playerRegion = GetRegionCoordiantes(playerPosition.ToIntVector());

            Vector2i existanceBegin = playerRegion - new Vector2i(existenceRadius / 2, existenceRadius / 2);
            Vector2i existanceEnd = playerRegion + new Vector2i(existenceRadius / 2, existenceRadius / 2);

            if(!createThread.IsAlive)
            {
                createThread = new Thread(() => CheckCreateRegions(existanceBegin, existanceEnd));
                createThread.IsBackground = true;
                createThread.Start();
            }
            if(!unloadThread.IsAlive)
            {
                unloadThread = new Thread(() => CheckUnloadRegions(existanceBegin, existanceEnd));
                unloadThread.IsBackground = true;
                unloadThread.Start();
            }
        }
        private void CheckCreateRegions(Vector2i existanceBegin, Vector2i existanceEnd)
        {
            for(int x = existanceBegin.X; x < existanceEnd.X; x++)
            {
                for(int y = existanceBegin.Y; y < existanceEnd.Y; y++)
                {
                    if(!createdWorlds.Contains(new Vector2i(x, y)))
                    {
                        CreateRegion(new Vector2i(x, y));
                    }
                }
            }
        }
        private void CheckUnloadRegions(Vector2i existanceBegin, Vector2i existanceEnd)
        {
            generating = true;
            const int UnloadBufferRadius = 4;

            for(int x = existanceBegin.X - UnloadBufferRadius; x < existanceBegin.X; x++)
            {
                for(int y = existanceBegin.Y - UnloadBufferRadius; y < existanceBegin.Y; y++)
                {
                    if(regionMap.ContainsKey(new Vector2i(x, y)))
                        UnloadRegion(new Vector2i(x, y));
                }
            }

            generating = false;

            for(int x = existanceEnd.X; x < existanceBegin.Y + UnloadBufferRadius; x++)
            {
                for(int y = existanceEnd.Y; y < existanceEnd.Y + UnloadBufferRadius; y++)
                {
                    if(regionMap.ContainsKey(new Vector2i(x, y)))
                        UnloadRegion(new Vector2i(x, y));
                }
            }
        }
        private void CreateRegion(Vector2i regionCoordiantes)
        {
            Vector3i regionOrigin = new Vector3i(regionCoordiantes.X * regionSize.X, 0, regionCoordiantes.Y * regionSize.Z);
            regionMap.Add(regionCoordiantes, new Region(regionOrigin, regionSize));
            createdWorlds.Add(regionCoordiantes);
            generator.GenerateRegion(this, regionOrigin);
            Region newRegion = GetRegionFromCoordiantes(regionCoordiantes).Data;
            WarlordApplication.GameEventManager.SendEvent(new GameEvent(new Optional<object>(this), "region_added", newRegion, 0));
        }
        private void UnloadRegion(Vector2i regionCoordiantes)
        {
            Region deadRegion = GetRegionFromCoordiantes(regionCoordiantes).Data;
            WarlordApplication.GameEventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this), "region_added", deadRegion, 0));
            regionMap.Remove(regionCoordiantes);
        }

        public void AddBlock(Vector3i absolutePosition, BlockType type)
        {
            Region currentRegion = GetRegionFromAbsolute(absolutePosition).Data;
            Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.RegionOrigin;

            currentRegion.AddBlock(currentBlockRelativePosition, type);

            Block theBlock = currentRegion.GetBlock(currentBlockRelativePosition);

            if(theBlock.Type != BlockType.Air)
                AddBlockUpdate(theBlock);
            else
                RemoveBlockUpdate(theBlock);

            WarlordApplication.GameEventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this),
                                                           "block_added",
                                                           new KeyValuePair<Region, Block>(currentRegion, theBlock),
                                                           0));
        }



        public void RemoveBlock(Vector3i absolutePosition)
        {
            Region currentRegion = GetRegionFromAbsolute(absolutePosition).Data;
            Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.RegionOrigin;

            Block theBlock = currentRegion.GetBlock(currentBlockRelativePosition);

            currentRegion.RemoveBlock(currentBlockRelativePosition);

            RemoveBlockUpdate(theBlock);

            WarlordApplication.GameEventManager.SendEvent(new GameEvent(new GameTools.Optional<object>(this),
                                                           "block_removed",
                                                           new KeyValuePair<Region, Block>(currentRegion, theBlock),
                                                           0));
        }
        private void AddBlockUpdate(Block addedBlock)
        {
            Region currentRegion = GetRegionFromAbsolute(addedBlock.UpperLeftTopPosition).Data;
            Region regionOfAdjacentBlock;

            BlockType type = addedBlock.Type;
            Vector3i position = addedBlock.UpperLeftTopPosition;

            Vector3i[] adjacencyOffsets = RegionArrayMaps.AdjacencyOffsets;
            BlockFaceField[] facingList = RegionArrayMaps.FacingList;

            Vector3i currentBlockRelativePosition;
            Vector3i adjacentBlockRelativePosition;

            currentBlockRelativePosition = position - currentRegion.RegionOrigin;

            BlockFaceField oppositeFacing;

            ContainmentType containment;

            for(int k = 0; k < 6; k++)
            {
                Vector3i adjacentVector = (position + adjacencyOffsets[k]).ToVector3;
                oppositeFacing = facingList[(k > 2) ? k - 3 : k + 3];

                containment = currentRegion.RegionBox.Contains(adjacentVector.ToVector3);

                if(containment != ContainmentType.Contains)
                {
                    if(!regionMap.ContainsKey(GetRegionCoordiantes(adjacentVector)) || adjacentVector.Y < 0 || adjacentVector.Y > regionSize.Y)
                    {
                        currentRegion.AddFace(currentBlockRelativePosition, facingList[k]);
                        continue;
                    }
                    else
                    {
                        regionOfAdjacentBlock = GetRegionFromAbsolute(adjacentVector).Data;
                    }
                }
                else
                {
                    regionOfAdjacentBlock = currentRegion;
                }

                adjacentBlockRelativePosition = adjacentVector - regionOfAdjacentBlock.RegionOrigin;

                Block adjacentBlock = regionOfAdjacentBlock.GetBlock(adjacentBlockRelativePosition);

                if(adjacentBlock.Type != BlockType.Air)
                {
                    currentRegion.RemoveFace(currentBlockRelativePosition, facingList[k]);
                    regionOfAdjacentBlock.RemoveFace(adjacentBlockRelativePosition, oppositeFacing);
                }
                else
                {
                    currentRegion.AddFace(currentBlockRelativePosition, facingList[k]);
                }
            }
        }
        private void RemoveBlockUpdate(Block removedBlock)
        {
            Region currentRegion = GetRegionFromAbsolute(removedBlock.UpperLeftTopPosition).Data;
            Region regionOfAdjacentBlock;

            BlockType type = removedBlock.Type;
            Vector3i position = removedBlock.UpperLeftTopPosition;

            Vector3i[] adjacencyOffsets = RegionArrayMaps.AdjacencyOffsets;
            BlockFaceField[] facingList = RegionArrayMaps.FacingList;

            Vector3i currentBlockRelativePosition;
            Vector3i adjacentBlockRelativePosition;

            currentBlockRelativePosition = position - currentRegion.RegionOrigin;

            BlockFaceField oppositeFacing;

            ContainmentType containment;

            for(int k = 0; k < 6; k++)
            {
                Vector3i adjacentVector = (position + adjacencyOffsets[k]).ToVector3;
                oppositeFacing = facingList[(k > 2) ? k - 3 : k + 3];

                containment = currentRegion.RegionBox.Contains(adjacentVector.ToVector3);

                currentRegion.RemoveFace(currentBlockRelativePosition, facingList[k]);
                if(containment != ContainmentType.Contains)
                {
                    if(!regionMap.ContainsKey(GetRegionCoordiantes(adjacentVector)) || adjacentVector.Y < 0 || adjacentVector.Y > regionSize.Y)
                    {
                        continue;
                    }
                    else
                    {
                        regionOfAdjacentBlock = GetRegionFromAbsolute(adjacentVector).Data;
                    }
                }
                else
                {
                    regionOfAdjacentBlock = currentRegion;
                }

                adjacentBlockRelativePosition = adjacentVector - regionOfAdjacentBlock.RegionOrigin;

                Block adjacentBlock = regionOfAdjacentBlock.GetBlock(adjacentBlockRelativePosition);

                if(adjacentBlock.Type != BlockType.Air)
                {
                    regionOfAdjacentBlock.AddFace(adjacentBlockRelativePosition, oppositeFacing);
                }
            }
        }

        private Optional<Region> GetRegionFromAbsolute(Vector3i absolutePosition)
        {
            Vector2i coordinates = GetRegionCoordiantes(absolutePosition);
            return GetRegionFromCoordiantes(coordinates);

        }
        private Optional<Region> GetRegionFromCoordiantes(Vector2i coordiantes)
        {
            if(createdWorlds.Contains(coordiantes))
                return new Optional<Region>(regionMap[coordiantes]);
            else
                return new Optional<Region>();
        }
        private Vector2i GetRegionCoordiantes(Vector3i absolutePosition)
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
        public Block GetBlock(Vector3i absolutePosition)
        {
            Region currentRegion = GetRegionFromAbsolute(absolutePosition).Data;
            Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.RegionOrigin;

            return currentRegion.GetBlock(currentBlockRelativePosition);
        }
        public Block HighestBlockAt(Vector2i location)
        {
            Block highestBlock;
            int y = regionSize.Z - 1;

            do
            {
                highestBlock = GetBlock(new Vector3i(location.X, y, location.Y));
                y--;
            } while(highestBlock.Type == BlockType.Air && y > -1);

            return highestBlock;
        }

        public Vector3i RegionSize
        {
            get { return regionSize; }
        }

        public bool Generating
        {
            get { return generating; }
        }
    }
}

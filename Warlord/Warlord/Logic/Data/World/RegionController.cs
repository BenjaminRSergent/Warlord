using System;
using System.Collections.Generic;
using GameTools;
using GameTools.Graph;
using GameTools.Process;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.GameTools;

namespace Warlord.Logic.Data.World
{
    class RegionController : ThreadWrapper
    {
        private RegionDatabase database;
        private int drawDistance;
        private SpiralProducer spiralProducer;

        public RegionController(int drawDistance, int seed, Vector3i regionSize)
            : base("Region Updater")
        {
            this.drawDistance = drawDistance;
            spiralProducer = new SpiralProducer(Vector3i.Zero, Direction.Up);
            database = new RegionDatabase(seed, regionSize);
        }

        protected override void ThreadBehavior()
        {
            while(true)
            {
                List<Vector3i> mustExist = new List<Vector3i>();

                Optional<Vector3i> region = GetFirstUncreatedWithin(drawDistance);

                if(region.Valid)
                    database.CreateRegion(this, region.Data);

                List<Vector3i> mustUnload = GetCreatedRegionsOutsideArea(drawDistance);
                foreach(Vector3i loadedRegion in mustUnload)
                {
                    database.UnloadRegion(loadedRegion);
                }

                SafePointCheckIn();
            }
        }

        public void ChangeBlock(Vector3i absolutePosition, BlockType blockType)
        {
            database.ChangeBlock(absolutePosition, blockType);
            UpdateFacing(database.GetBlock(absolutePosition));
        }

        private void UpdateFacing(Block block)
        {
            if(block.Type != BlockType.Air)
                AddBlockFaces(block);
            else
                RemoveBlockFace(block);
        }
        private void AddBlockFaces(Block addedBlock)
        {
            const int NUM_ORTHOGONAL_DIRECTIONS = 6;

            Vector3i addedBlockPosition = addedBlock.UpperLeftTopPosition;
            Vector3i adjacentBlockPosition;

            Vector3i adjacentRegion;

            BlockType adjacentBlockType;

            BlockFaceField adjacentBlockFacing;
            BlockFaceField addedBlockFacing;

            for(int directionIndex = 0; directionIndex < NUM_ORTHOGONAL_DIRECTIONS; directionIndex++)
            {
                addedBlockFacing = RegionArrayMaps.FacingList[directionIndex];
                adjacentBlockFacing = RegionArrayMaps.GetOppositeFacing(addedBlockFacing);

                adjacentBlockPosition = addedBlockPosition + RegionArrayMaps.GetDirectionFromFacing(addedBlockFacing);
                adjacentRegion = database.GetRegionCoordiantes(adjacentBlockPosition);

                if(!database.IsRegionLoaded(adjacentRegion))
                {
                    database.AddFace(addedBlockPosition, addedBlockFacing);
                }
                else
                {
                    adjacentBlockType = database.GetBlock(adjacentBlockPosition).Type;

                    if(adjacentBlockType == BlockType.Air)
                    {
                        database.AddFace(addedBlockPosition, addedBlockFacing);
                    }
                    else
                    {
                        database.RemoveFace(addedBlockPosition, addedBlockFacing);
                        database.RemoveFace(adjacentBlockPosition, adjacentBlockFacing);
                    }
                }
            }
        }
        private void RemoveBlockFace(Block removedBlock)
        {
            const int NUM_ORTHOGONAL_DIRECTIONS = 6;

            Vector3i removedBlockPosition = removedBlock.UpperLeftTopPosition;

            Vector3i adjacentBlockPosition;
            Vector3i adjacentRegion;

            BlockFaceField removedBlockFacing;
            BlockFaceField oppositeBlockFacing;

            for(int directionIndex = 0; directionIndex < NUM_ORTHOGONAL_DIRECTIONS; directionIndex++)
            {                
                removedBlockFacing = RegionArrayMaps.FacingList[directionIndex];
                oppositeBlockFacing = RegionArrayMaps.GetOppositeFacing(removedBlockFacing);

                adjacentBlockPosition = removedBlockPosition + RegionArrayMaps.GetDirectionFromFacing(removedBlockFacing);
                adjacentRegion = database.GetRegionCoordiantes(adjacentBlockPosition);

                database.RemoveFace(removedBlockPosition, removedBlockFacing);

                if(database.IsRegionLoaded(adjacentRegion))
                {
                    database.AddFace(adjacentBlockPosition, oppositeBlockFacing);
                }
            }
        }
        public Block GetBlock(Vector3i absolutePosition)
        {
            return database.GetBlock(absolutePosition);
        }
        private Optional<Vector3i> GetFirstUncreatedWithin(int drawDistance)
        {
            Vector3 playerPosition = GlobalSystems.EntityManager.Player.CurrentPosition;
            Vector3i playerRegion = database.GetRegionCoordiantes(playerPosition);

            playerPosition.Y = 0;

            if(!database.IsRegionLoaded(playerRegion))
                return new Optional<Vector3i>(playerRegion);

            Vector3i currentPosition;
            spiralProducer.NewSpiral(playerRegion, Direction.Up);

            for(int k = 0; k < 4 * drawDistance * drawDistance; k++)
            {
                currentPosition = spiralProducer.GetNextPosition();

                if(!database.IsRegionLoaded(currentPosition) &&
                    (currentPosition - playerRegion).LengthSquared < (drawDistance + 1) * (drawDistance + 1))
                    return new Optional<Vector3i>(currentPosition);
            }

            return new Optional<Vector3i>();
        }
        private List<Vector3i> GetCreatedRegionsOutsideArea(int maxDistance)
        {
            Vector3 playerPosition = GlobalSystems.EntityManager.Player.CurrentPosition;
            playerPosition.Y = 0;
            Vector3i playerRegion = database.GetRegionCoordiantes(playerPosition);

            Vector3i playerToRegion;

            List<Vector3i> regionsOutside = new List<Vector3i>();
            foreach(Vector3i region in database.GetCoordiantesOfLoadedRegions())
            {
                playerToRegion = playerRegion - region;
                if(Math.Abs(playerToRegion.X) > maxDistance ||
                   Math.Abs(playerToRegion.Y) > maxDistance)
                {
                    regionsOutside.Add(region);
                }
            }

            return regionsOutside;
        }
    }
}

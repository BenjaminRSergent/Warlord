using System;
using System.Collections.Generic;
using GameTools;
using GameTools.Graph;
using GameTools.Process;
using Microsoft.Xna.Framework;
using Warlord.Application;
using Warlord.GameTools;
using Warlord.Interfaces;

namespace Warlord.Logic.Data.World
{
    class RegionController : ThreadWrapper
    {
        private RegionDatabase database;
        private int drawDistance;
        private SpiralProducer spiralProducer;

        public RegionController(int drawDistance, int seed, Vector3 regionSize)
            : base("Region Controller")
        {
            this.drawDistance = drawDistance;
            spiralProducer = new SpiralProducer(Vector3.Zero, Direction.Up);
            database = new RegionDatabase(seed, regionSize);
        }

        protected override void ThreadBehavior()
        {
            while(true)
            {
                List<Vector3> mustExist = new List<Vector3>();

                Optional<Vector3> region = GetFirstUncreatedWithin(drawDistance);

                if(region.Valid)
                    database.CreateRegion(this, region.Data);

                List<Vector3> mustUnload = GetCreatedRegionsOutsideArea(drawDistance);
                foreach(Vector3 loadedRegion in mustUnload)
                {
                    database.UnloadRegion(loadedRegion);
                }

                SafePointCheckIn();
            }
        }
        public Optional<Block> GetBlock(Vector3 absolutePosition)
        {
            return database.GetBlock(absolutePosition);
        }
        public void ChangeBlock(Vector3 absolutePosition, BlockType blockType)
        {
            database.ChangeBlock(absolutePosition, blockType);
            UpdateFacing(database.GetBlock(absolutePosition).Data);
        }
        private void UpdateFacing(Block block)
        {
            UpdateBlockFaces(block);
        }
        private void UpdateBlockFaces(Block changedBlock)
        {
            const int NUM_ORTHOGONAL_DIRECTIONS = 6;

            Block adjacentBlock;

            Vector3 changedBlockPosition;
            Vector3 adjacentBlockPosition;

            Vector3 adjacentBlockRegion;

            BlockFaceField changedBlockFacing;
            BlockFaceField adjacentBlockFacing;

            changedBlockPosition = changedBlock.BackLeftbottomPosition;
            for(int facingIndex = 0; facingIndex < NUM_ORTHOGONAL_DIRECTIONS; facingIndex++)
            {
                changedBlockFacing = GameArrayMaps.FacingList[facingIndex];
                adjacentBlockFacing = GameArrayMaps.GetOppositeFacing(changedBlockFacing);

                Vector3 directionToAdjacent = GameArrayMaps.GetDirectionFromFacing(changedBlockFacing);

                adjacentBlockPosition = changedBlockPosition + directionToAdjacent;
                adjacentBlockRegion = database.GetRegionCoordiantes(adjacentBlockPosition);

                if(!database.IsRegionLoaded(adjacentBlockRegion))
                {
                    database.AddFace(changedBlockPosition, changedBlockFacing);
                }
                else
                {
                    adjacentBlock = database.GetBlock(adjacentBlockPosition).Data;

                    if(changedBlock.DoesHideFace(directionToAdjacent))
                        database.RemoveFace(adjacentBlockPosition, adjacentBlockFacing);
                    else
                        database.AddFace(adjacentBlockPosition, adjacentBlockFacing);

                    if(adjacentBlock.DoesHideFace(-directionToAdjacent))
                        database.RemoveFace(changedBlockPosition, changedBlockFacing);
                    else
                        database.AddFace(changedBlockPosition, changedBlockFacing);
                }
            }
        }
        private Optional<Vector3> GetFirstUncreatedWithin(int drawDistance)
        {
            Vector3 playerPosition = GlobalSystems.EntityManager.Player.CurrentPosition;
            Vector3 playerRegion = database.GetRegionCoordiantes(playerPosition);

            playerPosition.Y = 0;

            if(!database.IsRegionLoaded(playerRegion))
                return new Optional<Vector3>(playerRegion);

            Vector3 currentPosition;
            spiralProducer.NewSpiral(playerRegion, Direction.Up);

            for(int k = 0; k < 4 * drawDistance * drawDistance; k++)
            {
                currentPosition = spiralProducer.GetNextPosition();

                if(!database.IsRegionLoaded(currentPosition) &&
                    (currentPosition - playerRegion).LengthSquared() < (drawDistance + 1) * (drawDistance + 1))
                    return new Optional<Vector3>(currentPosition);
            }

            return new Optional<Vector3>();
        }
        private List<Vector3> GetCreatedRegionsOutsideArea(int maxDistance)
        {
            Vector3 playerPosition = GlobalSystems.EntityManager.Player.CurrentPosition;
            playerPosition.Y = 0;
            Vector3 playerRegion = database.GetRegionCoordiantes(playerPosition);

            Vector3 playerToRegion;

            int maxDistanceSq = maxDistance * maxDistance;

            List<Vector3> regionsOutside = new List<Vector3>();
            foreach(Vector3 region in database.GetCoordiantesOfLoadedRegions())
            {
                playerToRegion = playerRegion - region;
                if(playerToRegion.LengthSquared() > maxDistanceSq)
                {
                    regionsOutside.Add(region);
                }
            }

            return regionsOutside;
        }

        public List<Block> GetBlocksWithin(BoundingBox currentBox)
        {
            List<Block> blocksWithin = new List<Block>();
            Optional<Block> currentBlock;
            for(int x = (int)currentBox.Min.X; x < currentBox.Max.X; x++)
            {
                for(int y = (int)currentBox.Min.Y; y < currentBox.Max.Y; y++)
                {
                    for(int z = (int)currentBox.Min.Z; z < currentBox.Max.Z; z++)
                    {
                        currentBlock = GetBlock(new Vector3(x,y,z));

                        if(currentBlock.Valid && currentBlock.Data.Type != BlockType.Air)
                            blocksWithin.Add(currentBlock.Data);
                    }
                }
            }

            return blocksWithin;
        }
    }
}

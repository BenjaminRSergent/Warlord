using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using System.Threading;

namespace Warlord.Logic.Data.World
{
    class RegionUpdater : Process
    {
        RegionDatabase database;
        private int drawDistance;

        public RegionUpdater(int drawDistance, int seed, Vector3i regionSize, int timeAllocated) : base( timeAllocated )
        {
            this.drawDistance = drawDistance;

            database = new RegionDatabase(seed, regionSize);
        }

        public void ChangeBlock(Vector3i absolutePosition, BlockType blockType)
        {
            database.ChangeBlock(absolutePosition, blockType);
            UpdateFacing(database.GetBlock(absolutePosition));
        }

        private void UpdateFacing(Block block)
        {
            if( block.Type != BlockType.Air )
                AddBlockFaces(block);
            else
                RemoveBlockFace(block);
        }

        private void RemoveBlockFace(Block block)
        {
            //throw new NotImplementedException();
        }
        private void AddBlockFaces(Block addedBlock)
        {
            Region currentRegion = database.GetRegionFromAbsolute(addedBlock.UpperLeftTopPosition).Data;
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
                    if(!database.RegionMap.ContainsKey(database.GetRegionCoordiantes(adjacentVector)) || adjacentVector.Y < 0 || adjacentVector.Y > database.RegionSize.Y)
                    {
                        currentRegion.AddFace(currentBlockRelativePosition, facingList[k]);
                        continue;
                    }
                    else
                    {
                        regionOfAdjacentBlock = database.GetRegionFromAbsolute(adjacentVector).Data;
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

        public Block GetBlock(Vector3i absolutePosition)
        {
            return database.GetBlock(absolutePosition);
        }

        protected override void UpdateBehavior(GameTime gameTime)
        {
            const int unloadBuffer = 4;
            while( true )
            {

                for(int k = 0; k < drawDistance; k++)
                { 
                    List<Vector2i> mustExist = GetRegionInArea( k ).Where
                        ( vector => !database.RegionMap.ContainsKey(vector) ).ToList( );

                    if( mustExist.Count > 0 )
                    {
                        database.CreateRegion(this, mustExist[0] );                        
                        break;
                    } 
                }

                List<Vector2i> mustUnload = GetRegionBetweenAreas( drawDistance, drawDistance + unloadBuffer ).Where
                    ( vector => database.RegionMap.ContainsKey(vector) ).ToList( );                

                if( mustUnload.Count > 0 )
                    database.UnloadRegion(mustUnload[0]);
                    SafePointCheckIn( );

                SafePointCheckIn( );
            }
        }
        private List<Vector2i> GetRegionInArea( int radius )
        {
            Vector3f playerPosition = GlobalApplication.Application.EntityManager.Player.Position;
            Vector2i playerRegion = database.GetRegionCoordiantes( playerPosition.ToIntVector( ) );

            List<Vector2i> regionCoordiantes = new List<Vector2i>( );

            for(int x = 0; x < radius; x++)
            {
                for(int y = 0; y < radius; y++)
                {
                    regionCoordiantes.Add(playerRegion - new Vector2i( x,y ));

                    regionCoordiantes.Add(playerRegion + new Vector2i( x,y ));
                }
            }

            return regionCoordiantes;
        }
        private List<Vector2i> GetRegionBetweenAreas( int radiusNear, int radiusFar )
        {
            Vector3f playerPosition = GlobalApplication.Application.EntityManager.Player.Position;
            Vector2i playerRegion = database.GetRegionCoordiantes( playerPosition.ToIntVector( ) );

            List<Vector2i> regionCoordiantes = new List<Vector2i>( );

            for(int x = -radiusFar; x < radiusFar; x++)
            {
                if( x == -radiusNear )
                    x = radiusNear;
                for(int y = -radiusFar; y < radiusFar; y++)
                {
                    if( y == -radiusNear )
                        y = radiusNear;
                    regionCoordiantes.Add(playerRegion - new Vector2i( x,y ));
                }
            }

            return regionCoordiantes;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using System.Threading;
using Warlord.GameTools;
using Microsoft.Xna.Framework.Input;
using System.IO;
using GameTools.Process;

namespace Warlord.Logic.Data.World
{
    class RegionUpdater : ThreadProcess
    {
        RegionDatabase database;
        private int drawDistance;

        public RegionUpdater(int drawDistance, int seed, Vector3i regionSize)
        {
            this.drawDistance = drawDistance;

            database = new RegionDatabase(seed, regionSize);
        }

        protected override void ProcessBehavior()
        {
            while(true)
            {
                int radius = 0;

                List<Vector2i> mustExist = new List<Vector2i>();

                do
                {
                    mustExist = GetRegionInArea(radius).Where
                        (vector => !database.RegionMap.ContainsKey(vector)).ToList();
                    radius++;
                } while(radius < drawDistance && mustExist.Count < 1);

                if(mustExist.Count > 0)
                {
                    foreach(Vector2i region in mustExist)
                    {
                        bool regionDidNotPreviouslyExist = database.CreateRegion(this, region);
                        if(regionDidNotPreviouslyExist)
                            break;
                    }
                }              

                List<Vector2i> mustUnload = GetCreatedRegionsOutsideArea(drawDistance);
                foreach( Vector2i region in mustUnload )
                {
                    database.UnloadRegion(region);                    
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
            if( block.Type != BlockType.Air )
                AddBlockFaces(block);
            else
                RemoveBlockFace(block);
        }

        private void RemoveBlockFace(Block block)
        {
            throw new NotImplementedException();
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
                Vector3 adjacentVector = (position + adjacencyOffsets[k]).ToVector3;
                oppositeFacing = facingList[(k > 2) ? k - 3 : k + 3];

                containment = currentRegion.RegionBox.Contains(adjacentVector);

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
        private List<Vector2i> GetCreatedRegionsOutsideArea(int maxDistance)
        {
            Vector3f playerPosition = GlobalApplication.Application.EntityManager.Player.Position;
            Vector2i playerRegion = database.GetRegionCoordiantes(playerPosition.ToIntVector());            

            Vector2i distance;

            List<Vector2i> regionsOutside = new List<Vector2i>();
            foreach(Vector2i region in database.RegionMap.Keys)
            {   
                distance = playerRegion - region;
                if( Math.Abs(distance.X) > maxDistance || 
                    Math.Abs(distance.Y) > maxDistance )
                {
                    regionsOutside.Add(region);
                }
            }

            return regionsOutside;
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
                    regionCoordiantes.Add(playerRegion + new Vector2i( x,y ));
                    regionCoordiantes.Add(playerRegion + new Vector2i( x,-y ));
                    regionCoordiantes.Add(playerRegion + new Vector2i( -x,y ));
                    regionCoordiantes.Add(playerRegion + new Vector2i( -x,-y ));
                }
            }

            return regionCoordiantes;
        }
        public void Save( BinaryWriter outStream )
        {
            database.Save( outStream );
        }
        public void Load( BinaryReader inStream )
        {
            database.Load(inStream);
        }        
    }
}

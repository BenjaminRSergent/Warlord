﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic.Data;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Warlord.Event;

//Flagged for removal

namespace Warlord.Logic
{
    class FiniteWorld
    {
        Region[,] regions;
        
        public FiniteWorld( Vector2i worldSize, Vector3i regionSize )
        {
            regions = new Region[worldSize.X, worldSize.Y];

            for( int x = 0; x < worldSize.X; x++ )
            {
                for( int z = 0; z < worldSize.Y; z++ )
                {
                   regions[x,z] = new Region( new Vector3i(x*regionSize.X,0,z*regionSize.Z ), regionSize );
                   WarlordApplication.GameEventManager.SendEvent( new GameEvent( new GameTools.Optional<object>(this),
                                                                                 "region_added",
                                                                                 regions[x,z],
                                                                                 0));
                }
            }
        }

        public void AddBlock( Vector3i absolutePosition, BlockType type )
        {
            Region currentRegion = GetOwnerRegion(absolutePosition);
            Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.RegionOrigin;
            
            currentRegion.AddBlock( currentBlockRelativePosition, type );

            Block theBlock = currentRegion.GetBlock(currentBlockRelativePosition);                            

            AddBlockUpdate( theBlock );

            WarlordApplication.GameEventManager.SendEvent( new GameEvent( new GameTools.Optional<object>( this ),
                                                                          "block_added",
                                                                          new KeyValuePair<Region,Block>( currentRegion, theBlock),
                                                                          0 )) ;
        }

        public void RemoveBlock( Vector3i absolutePosition )
        {
            Region currentRegion = GetOwnerRegion(absolutePosition);
            Vector3i currentBlockRelativePosition = absolutePosition - currentRegion.RegionOrigin;

            Block theBlock = currentRegion.GetBlock(currentBlockRelativePosition);

            currentRegion.RemoveBlock( currentBlockRelativePosition );

            RemoveBlockUpdate( theBlock );

            WarlordApplication.GameEventManager.SendEvent( new GameEvent( new GameTools.Optional<object>( this ),
                                                                          "block_removed",
                                                                          new KeyValuePair<Region,Block>( currentRegion, theBlock),
                                                                          0 )) ;
        }

        private void AddBlockUpdate( Block currentBlock )
        {
            Region currentRegion = GetOwnerRegion(currentBlock.UpperLeftTopPosition);
            Region regionOfAdjacentBlock;

            BlockType type = currentBlock.Type;
            Vector3i position = currentBlock.UpperLeftTopPosition;

            Vector3i[] adjacencyOffsets = RegionArrayMaps.AdjacencyOffsets;
            BlockFaceField[] facingList  = RegionArrayMaps.FacingList;
  
            Vector3i currentBlockRelativePosition;
            Vector3i adjacentBlockRelativePosition;

            currentBlockRelativePosition = position - currentRegion.RegionOrigin;

            BlockFaceField oppositeFacing;            

            ContainmentType containment;
            for( int k = 0; k < 6; k++ )
            {
                Vector3i adjacentVector = (position + adjacencyOffsets[k]).ToVector3;                
                oppositeFacing = facingList[ (k > 2) ? k-3 : k+3 ];

                containment = currentRegion.RegionBox.Contains( adjacentVector.ToVector3 );
                
                if( containment != ContainmentType.Contains )
                {                    
                    if( adjacentVector.X < 0 ||
                        adjacentVector.Y < 0 ||
                        adjacentVector.Z < 0 )
                    {
                        currentRegion.AddFace( currentBlockRelativePosition, facingList[k] );
                        continue;
                    }                        
                    regionOfAdjacentBlock = GetOwnerRegion( adjacentVector );
                }
                else
                    regionOfAdjacentBlock = currentRegion;
                
                adjacentBlockRelativePosition = adjacentVector - regionOfAdjacentBlock.RegionOrigin;

                Block adjacentBlock = regionOfAdjacentBlock.GetBlock( adjacentBlockRelativePosition );

                if( adjacentBlock.Type != BlockType.Air )
                {
                    currentRegion.RemoveFace( currentBlockRelativePosition, facingList[k] );
                    regionOfAdjacentBlock.RemoveFace( adjacentBlockRelativePosition, oppositeFacing );
                }
                else
                {
                    currentRegion.AddFace( currentBlockRelativePosition, facingList[k] );
                }
            }
        }
        private void RemoveBlockUpdate( Block currentBlock )
        {
            Region currentRegion = GetOwnerRegion(currentBlock.UpperLeftTopPosition);
            Region regionOfAdjacentBlock;

            BlockType type = currentBlock.Type;
            Vector3i position = currentBlock.UpperLeftTopPosition;

            Vector3i[] adjacencyOffsets = RegionArrayMaps.AdjacencyOffsets;
            BlockFaceField[] facingList  = RegionArrayMaps.FacingList;
  
            Vector3i currentBlockRelativePosition;
            Vector3i adjacentBlockRelativePosition;

            currentBlockRelativePosition = position - currentRegion.RegionOrigin;

            BlockFaceField oppositeFacing;

            ContainmentType containment;
            for( int k = 0; k < 6; k++ )
            {
                Vector3i adjacentVector = (position + adjacencyOffsets[k]).ToVector3;                
                oppositeFacing = facingList[ (k > 2) ? k-3 : k+3 ];

                containment = currentRegion.RegionBox.Contains( adjacentVector.ToVector3 );                

                if( containment != ContainmentType.Contains )                
                    regionOfAdjacentBlock = GetOwnerRegion( adjacentVector );
                else
                    regionOfAdjacentBlock = currentRegion;
                
                adjacentBlockRelativePosition = adjacentVector - regionOfAdjacentBlock.RegionOrigin;                

                Block adjacentBlock = regionOfAdjacentBlock.GetBlock( position + adjacencyOffsets[k] );

                currentRegion.RemoveFace( position, facingList[k] );

                if( adjacentBlock.Type != BlockType.Air )
                {
                    regionOfAdjacentBlock.AddFace( adjacentBlockRelativePosition, oppositeFacing );
                }
            }
        }
        private Region GetOwnerRegion( Vector3i absolutePosition )
        {
            Vector3i regionLocation = Vector3i.Zero;

            regionLocation.X = (absolutePosition.X)/(RegionDimensions.X);
            regionLocation.Y = (absolutePosition.Z)/(RegionDimensions.Y);

            return regions[regionLocation.X, regionLocation.Y];
        }
        public Vector2i RegionDimensions
        {
            get
            {
                return new Vector2i( regions[0,0].RegionSize.X, regions[0,0].RegionSize.Z );
            }
        }
        public Vector2i WorldDimensions
        {
            get
            {
                return new Vector2i( regions.GetLength( 0 ), regions.GetLength( 1 ) );
            }
        }
    }
}

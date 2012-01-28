﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using GameTools;
using Microsoft.Xna.Framework.Graphics;
using GameTools.Graph;

namespace Warlord.Logic.Data
{
    class Region
    {
        private Vector3i regionOrigin;        
        private Vector3i regionSize;
        private BoundingBox regionBox;

        private Block[,,] blocks;              

        private Byte[,,] visibleFaceBitField;
        private int visibleFaces;       

        public Region( Vector3i regionOrigin, Vector3i regionSize )
        {
             this.regionOrigin = regionOrigin;
             this.regionSize = regionSize;

            regionBox = new BoundingBox( regionOrigin.ToVector3, (regionOrigin+regionSize-Vector3i.One).ToVector3 );

            blocks = new Block[regionSize.X, regionSize.Y, regionSize.Z];
            visibleFaceBitField = new byte[regionSize.X, regionSize.Y, regionSize.Z];

            Init( );
        }
        public void Init( )
        {
            Vector3i blockLocation;
            for( int x = 0; x < regionSize.X; x++ )
            {
                for( int y = 0; y < regionSize.Y; y++ )
                {
                    for( int z = 0; z < regionSize.Z; z++ )
                    {
                        blockLocation = regionOrigin + new Vector3i(x,y,z);
                        blocks[x,y,z] = new Block(blockLocation, BlockType.Air);
                        Altered = true;
                    }
                }
            }

            visibleFaces = 0;
        }
        public Block GetBlock( Vector3i relativePosition )
        {            
            if( regionBox.Contains( relativePosition.ToVector3 ) == ContainmentType.Contains )
                return blocks[relativePosition.X, relativePosition.Y, relativePosition.Z];
            else
                return new Block( -1*Vector3i.One, BlockType.Air );
        }
        public void AddBlock( Vector3i relativePosition, BlockType type )
        {
            int x = relativePosition.X;
            int y = relativePosition.Y;
            int z = relativePosition.Z;

            if( blocks[x,y,z].Type == BlockType.Air )
            {
                blocks[x,y,z] = new Block( blocks[x,y,z].UpperLeftTopPosition, type);

                Altered = true;
            }
        }
        public void RemoveBlock( Vector3i relativePosition )
        {            
            int x = relativePosition.X;
            int y = relativePosition.Y;
            int z = relativePosition.Z;

            if( blocks[x,y,z].Type != BlockType.Air )
            {
                blocks[x,y,z] = new Block( blocks[x,y,z].UpperLeftTopPosition, BlockType.Air );

                Altered = true;
            }
        }        
        public void AddFace( Vector3i position, BlockFaceField facing )
        {
            if( (visibleFaceBitField[position.X,position.Y,position.Z] & (byte)facing) == 0 )
            {
                visibleFaceBitField[position.X,position.Y,position.Z] |= (byte)facing;
                visibleFaces++;
            }
        }
        public void RemoveFace( Vector3i position, BlockFaceField facing )
        {
            if( (visibleFaceBitField[position.X,position.Y,position.Z] & (byte)facing) > 0 )
            {
                visibleFaceBitField[position.X,position.Y,position.Z] ^= (byte)facing;
                visibleFaces--;
            }
        }
        public Vector3i RegionOrigin
        {
            get { return regionOrigin; }
        }
        public Vector3i RegionSize
        {
            get { return regionSize; }
        } 
        public BoundingBox RegionBox
        {
            get { return regionBox; }
        }
        public Block[, ,] Blocks
        {
            get { return blocks; }
        }
        public Byte[, ,] VisibleFaceBitField
        {
            get { return visibleFaceBitField; }
        }    
        public int VisibleFaces
        {
            get { return visibleFaces; }
        }       

        public bool Altered{ get; set; }
    }
}
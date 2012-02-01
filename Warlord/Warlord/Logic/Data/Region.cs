using System;
using System.IO;
using GameTools.Graph;
using Microsoft.Xna.Framework;

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
                    }
                }
            }

            Altered = true;           
            Active = false;

            visibleFaces = 0;
        }
        public Block GetBlock( Vector3i relativePosition )
        {            
            if( regionBox.Contains( regionOrigin.ToVector3 + relativePosition.ToVector3 ) == ContainmentType.Contains )
                return blocks[relativePosition.X, relativePosition.Y, relativePosition.Z];
            else
                throw new ArgumentException( "The relative position " + relativePosition + " is not within the region" );
        }
        public void AddBlock( Vector3i relativePosition, BlockType type )
        {
            int x = relativePosition.X;
            int y = relativePosition.Y;
            int z = relativePosition.Z;

            blocks[x,y,z] = new Block( blocks[x,y,z].UpperLeftTopPosition, type);

            Altered = true;
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
        public void Save( BinaryWriter outStream )
        {

        }
        public void Load( BinaryReader inStream )
        {
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
        public bool Active{ get; set; }
        public bool Altered{ get; set; }
    }
}
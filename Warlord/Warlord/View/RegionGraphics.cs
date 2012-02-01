using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework;
using Warlord.Logic.Data;
using GameTools.Graph;
using Warlord.Event;
using Warlord.Logic.Data.World;

namespace Warlord.View
{
    class RegionGraphics
    {
        private Region masterRegion;
        private VertexPositionTexture[] regionMesh;
        private VertexBuffer regionBuffer;
        
        private GraphicsDevice graphics;

        private int index;

        static public Dictionary<BlockFaceField, Vector3i[]> faceVertexOffsetMap =
                                                      new Dictionary<BlockFaceField,Vector3i[]>( );

        static public Dictionary<BlockTexture, Dictionary<BlockFaceField, Vector2[]>> UVMap 
                                                                = new Dictionary<BlockTexture,Dictionary<BlockFaceField,Vector2[]>>( );

        public RegionGraphics( GraphicsDevice graphics, Region masterRegion )
        {
            this.masterRegion = masterRegion;            
            this.graphics = graphics;
        }        
        public void Update(  )
        {
            if( masterRegion.Altered )
                RebuildMesh( );
        }
        private void RebuildMesh()
        {   
            const int VERTICES_PER_FACE = 6;
            
            regionMesh = new VertexPositionTexture[VERTICES_PER_FACE*masterRegion.VisibleFaces];

            index = 0;

            BuildCubes( );

            if( regionMesh.Length > 0 )
            {
                regionBuffer = new VertexBuffer( graphics,
                                                 VertexPositionTexture.VertexDeclaration,
                                                 regionMesh.Length,
                                                 BufferUsage.WriteOnly );

                regionBuffer.SetData(regionMesh);
            }            

            masterRegion.Altered = false;
        }

        private void BuildCubes()
        {
            foreach( Block currentBlock in masterRegion.Blocks )
            {
                BuildBlockFaces( currentBlock );
            }
        }

        private void BuildBlockFaces(Block currentBlock)
        {
            int x = currentBlock.UpperLeftTopPosition.X - masterRegion.RegionOrigin.X;
            int y = currentBlock.UpperLeftTopPosition.Y - masterRegion.RegionOrigin.Y;
            int z = currentBlock.UpperLeftTopPosition.Z - masterRegion.RegionOrigin.Z;

            byte FaceInfo = masterRegion.VisibleFaceBitField[x,y,z];

            for( byte faceNum = 1; faceNum < (byte)BlockFaceField.MAXIMUM; faceNum*=2 )
            {
                if( (faceNum  & FaceInfo) > 0 )
                {
                    BuildFace( currentBlock.UpperLeftTopPosition, (BlockFaceField)faceNum, currentBlock.Type );
                }    
            }
        }

        private void BuildFace( Vector3i bottomBackLeft, BlockFaceField faceDir, BlockType type )
        {
            if( type == BlockType.Air )
                throw new ApplicationException( "Air with visible face found" );

            BlockTexture faceTexture = GetTexture( faceDir, type );
            Vector2[] uvMap = UVMap[faceTexture][faceDir];
            Vector3i[] offsetMap = faceVertexOffsetMap[faceDir];

            Vector3i currentFace;
            for( int k = 0; k < 6; k++ )
            {
                currentFace = (bottomBackLeft + offsetMap[k]);
                regionMesh[index+k] = new VertexPositionTexture( currentFace.ToVector3, uvMap[k] );
            }

            index += 6;
        }        

        private BlockTexture GetTexture( BlockFaceField faceDir, BlockType type )
        {
            switch( type )
            {
                case BlockType.Dirt:
                    return BlockTexture.Dirt;
                case BlockType.Grass:
                    {
                        switch( faceDir )
                        {
                            case BlockFaceField.YIncreasing:
                                return BlockTexture.GrassTop;
                            case BlockFaceField.YDecreasing:
                                return BlockTexture.Dirt;
                            default:
                                return BlockTexture.GrassSide;
                        }
                    }
                case BlockType.Stone:
                    return BlockTexture.Stone;                

                default:
                    throw new ArgumentException("Unsupported Block Type");
            }
        }
        
        public VertexPositionTexture[] RegionMesh
        {
            get { return regionMesh; }
        }
        public VertexBuffer RegionBuffer
        {
            get { return regionBuffer; }
        }
    }
}


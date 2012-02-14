using System;
using System.Collections.Generic;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Warlord.Logic.Data;
using Warlord.Logic.Data.World;

namespace Warlord.View.Human.Display
{
    class RegionGraphics : IDisposable
    {
        private Region masterRegion;
        private VertexPositionTexture[] regionMesh;
        private VertexBuffer regionBuffer;

        private GraphicsDevice graphics;

        private int index;
        private int numVertices;

        private bool clean;

        static BlockFaceField[] faces = (BlockFaceField[])System.Enum.GetValues(typeof(BlockFaceField));

        public static Dictionary<BlockFaceField, Vector3[]> faceVertexOffsetMap =
                                                      new Dictionary<BlockFaceField, Vector3[]>();

        public static Dictionary<BlockTexture, Dictionary<BlockFaceField, Vector2[]>> UVMap
                                                                = new Dictionary<BlockTexture, Dictionary<BlockFaceField, Vector2[]>>();

        public RegionGraphics(GraphicsDevice graphics, Region masterRegion)
        {
            this.masterRegion = masterRegion;
            this.graphics = graphics;

            regionMesh = new VertexPositionTexture[0];

            clean = false;
        }
        public void Update()
        {
            if(masterRegion.Altered)
                RebuildMesh();
        }
        private void RebuildMesh()
        {
            clean = false;

            const int VERTICES_PER_FACE = 6;

            numVertices = VERTICES_PER_FACE * masterRegion.VisibleFaces;

            // Resize if it's too small or more that 2 times too big
            if(regionMesh.Length < numVertices || regionMesh.Length > numVertices * 2)
                Array.Resize(ref regionMesh, numVertices);

            index = 0;

            BuildCubes();

            if(regionMesh.Length > 0)
            {
                regionBuffer = new VertexBuffer(graphics,
                                                 VertexPositionTexture.VertexDeclaration,
                                                 regionMesh.Length,
                                                 BufferUsage.WriteOnly);

                regionBuffer.SetData(regionMesh);
            }

            clean = true;
            masterRegion.Altered = false;
        }

        private void BuildCubes()
        {
            foreach(Block currentBlock in masterRegion.Blocks)
            {
                BuildBlockFaces(currentBlock);
            }
        }

        private void BuildBlockFaces(Block currentBlock)
        {
            foreach(BlockFaceField facing in faces)
            {
                if(currentBlock.IsFaceOn(facing))
                    BuildFace(currentBlock.BackLeftbottomPosition, facing, currentBlock.Type);
            }
        }

        private void BuildFace(Vector3 bottomBackLeft, BlockFaceField faceDir, BlockType type)
        {
            if(type == BlockType.Air)
                return;

            BlockTexture faceTexture = GetTexture(faceDir, type);
            Vector2[] uvMap = UVMap[faceTexture][faceDir];
            Vector3[] offsetMap = faceVertexOffsetMap[faceDir];

            Vector3 currentFace;
            for(int k = 0; k < 6; k++)
            {
                if(regionMesh.Length < index + k + 1)
                    break;

                currentFace = (bottomBackLeft + offsetMap[k]);

                if(regionMesh[index + k] == null)
                {
                    regionMesh[index + k] = new VertexPositionTexture(currentFace, uvMap[k]);
                }
                else
                {
                    regionMesh[index + k].Position = currentFace;
                    regionMesh[index + k].TextureCoordinate = uvMap[k];
                }
            }

            index += 6;
        }

        private BlockTexture GetTexture(BlockFaceField faceDir, BlockType type)
        {
            switch(type)
            {
                case BlockType.Dirt:
                    return BlockTexture.Dirt;
                case BlockType.Grass:
                    {
                        switch(faceDir)
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
        public bool Clean
        {
            get { return clean; }
        }
        public VertexPositionTexture[] RegionMesh
        {
            get { return regionMesh; }
        }
        public VertexBuffer RegionBuffer
        {
            get { return regionBuffer; }
        }

        public void Dispose()
        {
            if(regionBuffer != null)
                regionBuffer.Dispose();
        }
        public bool IsInVolume(BoundingFrustum frustum)
        {
            if(frustum.Contains(masterRegion.RegionBox) != ContainmentType.Disjoint)
                return true;
            else
                return false;
        }
        public int NumVertices
        {
            get { return numVertices; }
        }

        public BoundingBox BoundingBox { get { return masterRegion.RegionBox; } }
    }
}


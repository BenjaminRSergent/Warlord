using System;
using System.IO;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.World;

namespace Warlord.Logic.Data
{
    class Region
    {
        private Vector3i regionOrigin;
        private Vector3i regionSize;
        private BoundingBox regionBox;

        private Block[, ,] blocks;

        private int visibleFaces;

        public Region(Vector3i regionOrigin, Vector3i regionSize)
        {
            this.regionOrigin = regionOrigin;
            this.regionSize = regionSize;

            regionBox = new BoundingBox(regionOrigin.ToVector3, (regionOrigin + regionSize - Vector3i.One).ToVector3);

            blocks = new Block[regionSize.X, regionSize.Y, regionSize.Z];

            Init();
        }
        public void Init()
        {
            Vector3i blockLocation;

            for(int x = 0; x < regionSize.X; x++)
            {
                for(int y = 0; y < regionSize.Y; y++)
                {
                    for(int z = 0; z < regionSize.Z; z++)
                    {
                        blockLocation = regionOrigin + new Vector3i(x, y, z);
                        blocks[x, y, z] = new Block(blockLocation, BlockType.Air);                        
                    }
                }
            }

            Altered = true;
            visibleFaces = 0;
            Active = true;
        }
        public void Reinit(Vector3i newOrigin, Vector3i regionSize)
        {
            lock(this)
            {
                Vector3i blockLocation;

                this.regionOrigin = newOrigin;

                if(this.regionSize != regionSize)
                {
                    this.regionSize = regionSize;

                    for(int x = 0; x < regionSize.X; x++)
                    {
                        for(int y = 0; y < regionSize.Y; y++)
                        {
                            for(int z = 0; z < regionSize.Z; z++)
                            {
                                blockLocation = regionOrigin + new Vector3i(x, y, z);
                                blocks[x, y, z].Type = BlockType.Air;              
                                blocks[x, y, z].TurnOffAllFaces( );
                            }
                        }
                    }
                }

                regionBox = new BoundingBox(regionOrigin.ToVector3, (regionOrigin + regionSize - Vector3i.One).ToVector3);

                Init();
            }
        }
        public void Deactivate()
        {
            Active = false;
            Altered = false;
        }
        public Block GetBlock(Vector3i relativePosition)
        {
            if(regionBox.Contains(regionOrigin.ToVector3 + relativePosition.ToVector3) == ContainmentType.Contains)
                return blocks[relativePosition.X, relativePosition.Y, relativePosition.Z];
            else
                throw new ArgumentException("The relative position " + relativePosition + " is not within the region");
        }
        public void AddBlock(Vector3i relativePosition, BlockType type)
        {
            int x = relativePosition.X;
            int y = relativePosition.Y;
            int z = relativePosition.Z;

            blocks[x, y, z].Type = type;

            Altered = true;
        }
        public void RemoveBlock(Vector3i relativePosition)
        {
            int x = relativePosition.X;
            int y = relativePosition.Y;
            int z = relativePosition.Z;

            if(blocks[x, y, z].Type != BlockType.Air)
            {
                blocks[x, y, z].Type = BlockType.Air;

                Altered = true;
            }
        }
        public void AddFace(Vector3i position, BlockFaceField facing)
        {
            Block blockAtPos = GetBlock(position);
            if(blockAtPos.IsFaceOff(facing))
            {
                blockAtPos.TurnFaceOn(facing);
                visibleFaces++;
                Altered = true;
            }
        }
        public void RemoveFace(Vector3i position, BlockFaceField facing)
        {
            Block blockAtPos = GetBlock(position);
            if(blockAtPos.IsFaceOn(facing))
            {
                blockAtPos.TurnFaceOff(facing);
                visibleFaces--;
                Altered = true;
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
        public int VisibleFaces
        {
            get { return visibleFaces; }
        }
        public bool Active { get; private set; }
        public bool Altered { get; set; }
    }
}
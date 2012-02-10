using System;
using System.IO;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.World;

namespace Warlord.Logic.Data
{
    class Region
    {
        private Vector3 regionOrigin;
        private Vector3 regionSize;
        private BoundingBox regionBox;

        private Block[, ,] blocks;

        private int visibleFaces;

        public Region(Vector3 regionOrigin, Vector3 regionSize)
        {
            this.regionOrigin = regionOrigin;
            this.regionSize = regionSize;

            regionBox = new BoundingBox(regionOrigin, (regionOrigin + regionSize - Vector3.One));

            blocks = new Block[(int)regionSize.X, (int)regionSize.Y, (int)regionSize.Z];

            Init();
        }
        public void Init()
        {
            Vector3 blockLocation;

            for(int x = 0; x < regionSize.X; x++)
            {
                for(int y = 0; y < regionSize.Y; y++)
                {
                    for(int z = 0; z < regionSize.Z; z++)
                    {
                        blockLocation = regionOrigin + new Vector3(x, y, z);
                        blocks[x, y, z] = new Block(blockLocation, BlockType.Air);                        
                    }
                }
            }

            Altered = true;
            visibleFaces = 0;
            Active = true;
        }
        public void Reinit(Vector3 newOrigin, Vector3 regionSize)
        {
            lock(this)
            {
                Vector3 blockLocation;

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
                                blockLocation = regionOrigin + new Vector3(x, y, z);
                                blocks[x, y, z].Type = BlockType.Air;              
                                blocks[x, y, z].TurnOffAllFaces( );
                            }
                        }
                    }
                }

                regionBox = new BoundingBox(regionOrigin, (regionOrigin + regionSize - Vector3.One));

                Init();
            }
        }
        public void Deactivate()
        {
            Active = false;
            Altered = false;
        }
        public Block GetBlock(Vector3 relativePosition)
        {
            if(regionBox.Contains(regionOrigin + relativePosition) == ContainmentType.Contains)
                return blocks[(int)relativePosition.X, (int)relativePosition.Y, (int)relativePosition.Z];
            else
                throw new ArgumentException("The relative position " + relativePosition + " is not within the region");
        }
        public void AddBlock(Vector3 relativePosition, BlockType type)
        {
            int x = (int)relativePosition.X;
            int y = (int)relativePosition.Y;
            int z = (int)relativePosition.Z;

            blocks[x, y, z].Type = type;

            Altered = true;
        }
        public void RemoveBlock(Vector3 relativePosition)
        {
            int x = (int)relativePosition.X;
            int y = (int)relativePosition.Y;
            int z = (int)relativePosition.Z;

            if(blocks[x, y, z].Type != BlockType.Air)
            {
                blocks[x, y, z].Type = BlockType.Air;

                Altered = true;
            }
        }
        public void AddFace(Vector3 position, BlockFaceField facing)
        {
            Block blockAtPos = GetBlock(position);
            if(blockAtPos.IsFaceOff(facing))
            {
                blockAtPos.TurnFaceOn(facing);
                visibleFaces++;
                Altered = true;
            }
        }
        public void RemoveFace(Vector3 position, BlockFaceField facing)
        {
            Block blockAtPos = GetBlock(position);
            if(blockAtPos.IsFaceOn(facing))
            {
                blockAtPos.TurnFaceOff(facing);
                visibleFaces--;
                Altered = true;
            }
        }
        public Vector3 RegionOrigin
        {
            get { return regionOrigin; }
        }
        public Vector3 RegionSize
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

using System.IO;
using GameTools.Graph;

namespace Warlord.Logic.Data.World
{
    class Block
    {
        private Vector3i upperLeftTopPosition;
        private byte type;
        public byte facing;

        public Block(Vector3i upperLeftTopPosition, BlockType blockType)
        {
            this.upperLeftTopPosition = upperLeftTopPosition;
            this.type = (byte)blockType;
        }
        public void TurnOffAllFaces( )
        {
            facing = 0;
        }
        public void TurnFaceOn(BlockFaceField face)
        {
            facing |= (byte)face;
        }
        public void TurnFaceOff(BlockFaceField face)
        {
            if( IsFaceOn(face) )
                facing ^= (byte)face;
        }        
        public bool IsFaceOn(BlockFaceField face)
        {
            return (facing & (byte)face) > 0;
        }
        public bool IsFaceOff(BlockFaceField face)
        {
            return (facing & (byte)face) == 0;
        }

        public Vector3i UpperLeftTopPosition
        {
            get { return upperLeftTopPosition; }
        }
        public BlockType Type
        {
            get { return (BlockType)type; }
            set { type = (byte)value; }
        }
    }
}

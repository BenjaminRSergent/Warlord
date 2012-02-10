
using System.IO;
using GameTools.Graph;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.World
{
    class Block
    {
        private Vector3 upperLeftTopPosition;
        private BlockType type;
        public byte facing;

        public Block(Vector3 upperLeftTopPosition, BlockType blockType)
        {
            this.upperLeftTopPosition = upperLeftTopPosition;
            this.type = blockType;
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
        public bool DoesHideFace( Vector3 adjacentLocation )
        {
            if( type == BlockType.Air )
                return false;
            else
                return true;
        }
        public Vector3 UpperLeftTopPosition
        {
            get { return upperLeftTopPosition; }
        }
        public BlockType Type
        {
            get { return (BlockType)type; }
            set { type = value; }
        }
    }
}

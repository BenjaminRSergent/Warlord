
using System.IO;
using GameTools.Graph;

namespace Warlord.Logic.Data.World
{
    class Block
    {
        private Vector3i upperLeftTopPosition;
        private byte type;

        public Block(Vector3i upperLeftTopPosition, BlockType blockType)
        {
            this.upperLeftTopPosition = upperLeftTopPosition;
            this.type = (byte)blockType;
        }
        public void Save(BinaryWriter outStream)
        {
            outStream.Write(UpperLeftTopPosition.X);
            outStream.Write(UpperLeftTopPosition.Y);
            outStream.Write(UpperLeftTopPosition.Z);

            outStream.Write(type);
        }
        public void Load(BinaryReader inStream)
        {
            upperLeftTopPosition.X = inStream.ReadInt32();
            upperLeftTopPosition.Y = inStream.ReadInt32();
            upperLeftTopPosition.Z = inStream.ReadInt32();

            type = inStream.ReadByte();
        }
        public Vector3i UpperLeftTopPosition
        {
            get { return upperLeftTopPosition; }
        }
        public BlockType Type
        {
            get { return (BlockType)type; }
        }
    }
}

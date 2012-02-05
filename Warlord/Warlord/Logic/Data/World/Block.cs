﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using GameTools.Graph;
using System.IO;

namespace Warlord.Logic.Data.World
{
    class Block
    {
        private Vector3i upperLeftTopPosition;
        private byte type;        

        public Block( Vector3i upperLeftTopPosition, BlockType blockType )
        {
            this.upperLeftTopPosition = upperLeftTopPosition;
            this.type = (byte)blockType;
        }
        public void Save( BinaryWriter outStream )
        {
            outStream.Write(UpperLeftTopPosition.X);
            outStream.Write(UpperLeftTopPosition.Y);
            outStream.Write(UpperLeftTopPosition.Z);

            outStream.Write(type);
        }
        public void Load( BinaryReader inStream )
        {
            UpperLeftTopPosition.X = inStream.ReadInt32( );
            UpperLeftTopPosition.Y = inStream.ReadInt32( );
            UpperLeftTopPosition.Z = inStream.ReadInt32( );

            type = inStream.ReadByte( );
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

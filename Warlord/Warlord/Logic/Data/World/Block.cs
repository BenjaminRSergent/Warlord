using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using GameTools.Graph;

namespace Warlord.Logic.Data.World
{
    class Block
    {
        private Vector3i upperLeftTopPosition;
        private BlockType type;        

        public Block( Vector3i upperLeftTopPosition, BlockType blockType )
        {
            this.upperLeftTopPosition = upperLeftTopPosition;
            this.type = blockType;
        }

        public Vector3i UpperLeftTopPosition
        {
            get { return upperLeftTopPosition; }
        }
        public BlockType Type
        {
            get { return type; }            
        }
    }
}

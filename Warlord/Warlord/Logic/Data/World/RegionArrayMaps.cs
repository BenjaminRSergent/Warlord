using GameTools.Graph;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.World
{
    static class GameArrayMaps
    {
        private static Dictionary<BlockFaceField, Vector3> faceOffsetMap;
        private static BlockFaceField[] facingList = new BlockFaceField[6];
        public static Vector3[] directions;

        static GameArrayMaps( )
        {
            faceOffsetMap = new Dictionary<BlockFaceField,Vector3>( );

            faceOffsetMap.Add(BlockFaceField.ZIncreasing,new Vector3(0, 0, 1));
            faceOffsetMap.Add(BlockFaceField.YIncreasing,new Vector3(0, 1, 0));
            faceOffsetMap.Add(BlockFaceField.XIncreasing,new Vector3(1, 0, 0));

            faceOffsetMap.Add( BlockFaceField.ZDecreasing,new Vector3(0, 0, -1));
            faceOffsetMap.Add( BlockFaceField.YDecreasing,new Vector3(0, -1, 0));
            faceOffsetMap.Add( BlockFaceField.XDecreasing,new Vector3(-1, 0, 0));

            facingList[0] = BlockFaceField.ZIncreasing;
            facingList[1] = BlockFaceField.YIncreasing;
            facingList[2] = BlockFaceField.XIncreasing;

            facingList[3] = BlockFaceField.ZDecreasing;
            facingList[4] = BlockFaceField.YDecreasing;
            facingList[5] = BlockFaceField.XDecreasing;

            directions = new Vector3[6];

            directions[0] = faceOffsetMap[BlockFaceField.ZIncreasing];
            directions[1] = faceOffsetMap[BlockFaceField.YIncreasing];
            directions[2] = faceOffsetMap[BlockFaceField.XIncreasing];

            directions[3] = faceOffsetMap[BlockFaceField.ZDecreasing];
            directions[4] = faceOffsetMap[BlockFaceField.YDecreasing];
            directions[5] = faceOffsetMap[BlockFaceField.XDecreasing];
        }
        public static BlockFaceField GetOppositeFacing(BlockFaceField facing)
        {
            int oppositeIndex;
            for(int index = 0; index < facingList.Length; index++)
            {
                if(facingList[index] == facing)
                {
                    oppositeIndex = (index < 3) ? index + 3 : index - 3;
                    return facingList[oppositeIndex];
                }
            }

            throw new ArgumentException("" + facing.ToString() + " was not found in the facingList");
        }
        public static Vector3 GetDirectionFromFacing( BlockFaceField facing)
        {
            return faceOffsetMap[facing];
        }
        public static Vector3[] GetDirections()
        {
            return directions;
        }
        public static BlockFaceField[] FacingList
        {
            get { return GameArrayMaps.facingList; }
        }
    }
}

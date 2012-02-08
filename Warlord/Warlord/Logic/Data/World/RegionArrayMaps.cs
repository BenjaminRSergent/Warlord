using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;

namespace Warlord.Logic.Data.World
{
    static class RegionArrayMaps
    {
        static private Vector3i[] adjacencyOffsets = new Vector3i[6];
        static private BlockFaceField[] facingList = new BlockFaceField[6];

        static public void Init()
        {
            adjacencyOffsets[0] = new Vector3i(0, 0, 1);
            adjacencyOffsets[1] = new Vector3i(0, 1, 0);
            adjacencyOffsets[2] = new Vector3i(1, 0, 0);

            adjacencyOffsets[3] = new Vector3i(0, 0, -1);
            adjacencyOffsets[4] = new Vector3i(0, -1, 0);
            adjacencyOffsets[5] = new Vector3i(-1, 0, 0);

            facingList[0] = BlockFaceField.ZIncreasing;
            facingList[1] = BlockFaceField.YIncreasing;
            facingList[2] = BlockFaceField.XIncreasing;

            facingList[3] = BlockFaceField.ZDecreasing;
            facingList[4] = BlockFaceField.YDecreasing;
            facingList[5] = BlockFaceField.XDecreasing;
        }

        public static Vector3i[] AdjacencyOffsets
        {
            get { return RegionArrayMaps.adjacencyOffsets; }
        }
        public static BlockFaceField[] FacingList
        {
            get { return RegionArrayMaps.facingList; }
        }
    }
}

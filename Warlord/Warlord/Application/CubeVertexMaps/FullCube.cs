using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.World;
using Warlord.View.Human.Display;
using GameTools.Graph;

namespace Warlord.Application.CubeVertexMaps
{   

    static class FullCube
    {
        public static Dictionary<BlockFaceField, Vector3[]> faceVertexOffsetMap =
                                                      new Dictionary<BlockFaceField, Vector3[]>();

        static FullCube()
        {
            InitializeVertecies();
        }

        private static void InitializeVertecies()
        {
            Vector3[] offset = new Vector3[6];

            //X+++
            offset = new Vector3[6];

            offset[0] = new Vector3(1, 0, 1);
            offset[1] = new Vector3(1, 1, 0);
            offset[2] = new Vector3(1, 0, 0);

            offset[3] = new Vector3(1, 1, 1);
            offset[4] = offset[1];
            offset[5] = offset[0];

            faceVertexOffsetMap.Add(BlockFaceField.XIncreasing, offset);
            //Y+++
            offset = new Vector3[6];

            offset[0] = new Vector3(0, 1, 0);
            offset[1] = new Vector3(1, 1, 0);
            offset[2] = new Vector3(0, 1, 1);

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3(1, 1, 1);

            faceVertexOffsetMap.Add(BlockFaceField.YIncreasing, offset);
            //Z+++
            offset = new Vector3[6];

            offset[0] = new Vector3(0, 1, 1);
            offset[1] = new Vector3(1, 0, 1);
            offset[2] = new Vector3(0, 0, 1);

            offset[3] = new Vector3(1, 1, 1);
            offset[4] = offset[1];
            offset[5] = offset[0];

            faceVertexOffsetMap.Add(BlockFaceField.ZIncreasing, offset);
            //X---
            offset = new Vector3[6];

            offset[0] = new Vector3(0, 0, 0);
            offset[1] = new Vector3(0, 1, 0);
            offset[2] = new Vector3(0, 0, 1);

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3(0, 1, 1);

            faceVertexOffsetMap.Add(BlockFaceField.XDecreasing, offset);
            //Y---
            offset = new Vector3[6];

            offset[0] = new Vector3(0, 0, 1);
            offset[1] = new Vector3(1, 0, 0);
            offset[2] = new Vector3(0, 0, 0);

            offset[3] = new Vector3(1, 0, 1);
            offset[4] = offset[1];
            offset[5] = offset[0];

            faceVertexOffsetMap.Add(BlockFaceField.YDecreasing, offset);
            //Z---
            offset = new Vector3[6];

            offset[0] = new Vector3(0, 0, 0);
            offset[1] = new Vector3(1, 0, 0);
            offset[2] = new Vector3(0, 1, 0);

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3(1, 1, 0);

            faceVertexOffsetMap.Add(BlockFaceField.ZDecreasing, offset);

        }       
    }
}

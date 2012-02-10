using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warlord.Logic.Data.World;
using GameTools.Graph;
using Warlord.View.Human.Display;
using Microsoft.Xna.Framework;

namespace Warlord.Application.CubeVertexMaps
{
    class ZIncreasingRamp
    {
        public static Dictionary<BlockFaceField, Vector3i[]> faceVertexOffsetMap =
                                                      new Dictionary<BlockFaceField, Vector3i[]>();

        static ZIncreasingRamp()
        {
            InitializeVertecies();
        }

        private static void InitializeVertecies()
        {
            Vector3i[] offset = new Vector3i[6];

            //X+++
            offset = new Vector3i[6];

            offset[0] = new Vector3i( 1, 0, 1 );
            offset[1] = new Vector3i( 1, 0, 0 );
            offset[2] = new Vector3i( 1, 0, 0 );                        

            offset[3] = new Vector3i( 1, 1, 1 );
            offset[4] = new Vector3i( 1, 0, 0 );
            offset[5] = new Vector3i( 1, 0, 1 );                       

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.XIncreasing, offset);
            //Y+++
            offset = new Vector3i[6];

            offset[0] = new Vector3i( 0, 0, 0 );
            offset[1] = new Vector3i( 1, 0, 0 );
            offset[2] = new Vector3i( 0, 1, 1 );                        

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3i( 1, 1, 1 );

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.YIncreasing, offset);
            //Z+++
            offset = new Vector3i[6];

            offset[0] = new Vector3i( 0, 1, 1 );
            offset[1] = new Vector3i( 1, 0, 1 );
            offset[2] = new Vector3i( 0, 0, 1 );          

            offset[3] = new Vector3i( 1, 1, 1 );
            offset[4] = offset[1];
            offset[5] = offset[0];
            
            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.ZIncreasing, offset);
            //X---
            offset = new Vector3i[3];

            offset[0] = new Vector3i( 0, 0, 1 );
            offset[1] = new Vector3i( 0, 0, 0 );
            offset[2] = new Vector3i( 0, 1, 1 );

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.XDecreasing, offset);
            //Y---
            offset = new Vector3i[6];           
            
            offset[0] = new Vector3i( 0, 0, 1 );
            offset[1] = new Vector3i( 1, 0, 0 );
            offset[2] = new Vector3i( 0, 0, 0 );

            offset[3] = new Vector3i( 1, 0, 1 );
            offset[4] = offset[1];
            offset[5] = offset[0];

            //Z---
            offset = new Vector3i[6];

            offset[0] = new Vector3i( 0, 0, 0 );
            offset[1] = new Vector3i( 1, 0, 0 );
            offset[2] = new Vector3i( 0, 0, 0 );                        

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3i( 1, 0, 0 );

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.ZDecreasing, offset);

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.YDecreasing, offset);
        }
    }
}

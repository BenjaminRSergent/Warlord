using System.Collections.Generic;
using GameTools.Graph;
using Microsoft.Xna.Framework;
using Warlord.Logic.Data.World;
using Warlord.View.Human.Display;

namespace Warlord.Application
{
    class GameStaticInitalizer
    {
        public static void InitalizeStatics()
        {
            InitializeVertecies();
            InitalizeUvMap();
        }

        private static void InitializeVertecies()
        {
            Vector3i[] offset = new Vector3i[6];

            //X+++
            offset = new Vector3i[6];

            offset[0] = new Vector3i(1, 0, 1);
            offset[1] = new Vector3i(1, 1, 0);
            offset[2] = new Vector3i(1, 0, 0);

            offset[3] = new Vector3i(1, 1, 1);
            offset[4] = offset[1];
            offset[5] = offset[0];

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.XIncreasing, offset);
            //Y+++
            offset = new Vector3i[6];

            offset[0] = new Vector3i(0, 1, 0);
            offset[1] = new Vector3i(1, 1, 0);
            offset[2] = new Vector3i(0, 1, 1);

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3i(1, 1, 1);

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.YIncreasing, offset);
            //Z+++
            offset = new Vector3i[6];

            offset[0] = new Vector3i(0, 1, 1);
            offset[1] = new Vector3i(1, 0, 1);
            offset[2] = new Vector3i(0, 0, 1);

            offset[3] = new Vector3i(1, 1, 1);
            offset[4] = offset[1];
            offset[5] = offset[0];

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.ZIncreasing, offset);
            //X---
            offset = new Vector3i[6];

            offset[0] = new Vector3i(0, 0, 0);
            offset[1] = new Vector3i(0, 1, 0);
            offset[2] = new Vector3i(0, 0, 1);

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3i(0, 1, 1);

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.XDecreasing, offset);
            //Y---
            offset = new Vector3i[6];

            offset[0] = new Vector3i(0, 0, 1);
            offset[1] = new Vector3i(1, 0, 0);
            offset[2] = new Vector3i(0, 0, 0);

            offset[3] = new Vector3i(1, 0, 1);
            offset[4] = offset[1];
            offset[5] = offset[0];

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.YDecreasing, offset);
            //Z---
            offset = new Vector3i[6];

            offset[0] = new Vector3i(0, 0, 0);
            offset[1] = new Vector3i(1, 0, 0);
            offset[2] = new Vector3i(0, 1, 0);

            offset[3] = offset[2];
            offset[4] = offset[1];
            offset[5] = new Vector3i(1, 1, 0);

            RegionGraphics.faceVertexOffsetMap.Add(BlockFaceField.ZDecreasing, offset);

        }
        private static void InitalizeUvMap()
        {
            Dictionary<BlockFaceField, Vector2[]> uvMap;

            //Grass Top            
            uvMap = GetUVFromXY(0, 0);
            RegionGraphics.UVMap.Add(BlockTexture.GrassTop, uvMap);

            //Dirt, all Sides
            uvMap = GetUVFromXY(1, 0);
            RegionGraphics.UVMap.Add(BlockTexture.Dirt, uvMap);

            //Grass Side
            uvMap = GetUVFromXY(2, 0);
            RegionGraphics.UVMap.Add(BlockTexture.GrassSide, uvMap);

            // Stone, All sides
            uvMap = GetUVFromXY(3, 0);
            RegionGraphics.UVMap.Add(BlockTexture.Stone, uvMap);
        }
        private static Dictionary<BlockFaceField, Vector2[]> GetUVFromXY(int x, int y)
        {
            Dictionary<BlockFaceField, Vector2[]> uvMap = new Dictionary<BlockFaceField, Vector2[]>();

            Vector2[] uvVectors = new Vector2[6];

            const float textureSize = 1 / 16.0f;

            float Xoffset = x * textureSize;
            float Yoffset = y * textureSize;

            Vector2 upperLeft = new Vector2(Xoffset, Yoffset);
            Vector2 lowerLeft = new Vector2(Xoffset + textureSize, Yoffset);
            Vector2 upperRight = new Vector2(Xoffset, Yoffset + textureSize);
            Vector2 lowerRight = new Vector2(Xoffset + textureSize, Yoffset + textureSize);

            // X-Increasing
            uvVectors = new Vector2[6];

            uvVectors[0] = upperRight;
            uvVectors[1] = lowerLeft;
            uvVectors[2] = lowerRight;

            uvVectors[3] = upperLeft;
            uvVectors[4] = uvVectors[1];
            uvVectors[5] = uvVectors[0];

            uvMap.Add(BlockFaceField.XIncreasing, uvVectors);

            // Y-Increasing
            uvVectors = new Vector2[6];

            uvVectors[0] = upperLeft;
            uvVectors[1] = lowerLeft;
            uvVectors[2] = upperRight;

            uvVectors[3] = uvVectors[2];
            uvVectors[4] = uvVectors[1];
            uvVectors[5] = lowerRight;

            uvMap.Add(BlockFaceField.YIncreasing, uvVectors);

            // Z-Increasing
            uvVectors = new Vector2[6];

            uvVectors[0] = upperLeft;
            uvVectors[1] = lowerRight;
            uvVectors[2] = upperRight;

            uvVectors[3] = lowerLeft;
            uvVectors[4] = uvVectors[1];
            uvVectors[5] = uvVectors[0];

            uvMap.Add(BlockFaceField.ZIncreasing, uvVectors);

            // X-Decreasing
            uvVectors = new Vector2[6];

            uvVectors[0] = upperRight;
            uvVectors[1] = upperLeft;
            uvVectors[2] = lowerRight;

            uvVectors[3] = uvVectors[2];
            uvVectors[4] = uvVectors[1];
            uvVectors[5] = lowerLeft;

            uvMap.Add(BlockFaceField.XDecreasing, uvVectors);

            // Y-Decreasing
            uvVectors = new Vector2[6];

            uvVectors[0] = upperLeft;
            uvVectors[1] = lowerRight;
            uvVectors[2] = upperRight;

            uvVectors[3] = lowerLeft;
            uvVectors[4] = uvVectors[1];
            uvVectors[5] = uvVectors[0];

            uvMap.Add(BlockFaceField.YDecreasing, uvVectors);

            // Z-Decreasing
            uvVectors = new Vector2[6];

            uvVectors[0] = lowerRight;
            uvVectors[1] = upperRight;
            uvVectors[2] = lowerLeft;

            uvVectors[3] = uvVectors[2];
            uvVectors[4] = uvVectors[1];
            uvVectors[5] = upperLeft;

            uvMap.Add(BlockFaceField.ZDecreasing, uvVectors);

            return uvMap;
        }
    }
}

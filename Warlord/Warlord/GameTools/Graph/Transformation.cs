﻿using System;

namespace GameTools.Graph
{
    static class Transformation
    {
        public static Vector3i ChangeVectorScale(Vector3i position, Vector3i scale)
        {
            Vector3i scaledPosition = Vector3i.Zero;

            scaledPosition.X = ScaleSingle(position.X, scale.X);
            scaledPosition.Y = ScaleSingle(position.Y, scale.Y);
            scaledPosition.Z = ScaleSingle(position.Z, scale.Z);

            return scaledPosition;
        }
        public static int ScaleSingle(int original, int scale)
        {
            int scaledSingle;

            if(original > 0)
                scaledSingle = original / scale;
            else
            {
                double doubleX = original / (double)scale;
                scaledSingle = (int)Math.Floor(doubleX);
            }

            return scaledSingle;
        }

        public static Vector3i AbsoluteToRelative(Vector3i position, Vector3i origin)
        {
            return position - origin;
        }
    }
}

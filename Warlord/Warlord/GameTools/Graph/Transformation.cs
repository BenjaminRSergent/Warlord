using System;
using Microsoft.Xna.Framework;

namespace GameTools.Graph
{
    static class Transformation
    {
        public static Vector3 ChangeVectorScaleFloored(Vector3 position, Vector3 scale)
        {
            Vector3 scaledPosition = Vector3.Zero;

            scaledPosition.X = ScaleSingleFloored(position.X, scale.X);
            scaledPosition.Y = ScaleSingleFloored(position.Y, scale.Y);
            scaledPosition.Z = ScaleSingleFloored(position.Z, scale.Z);

            return scaledPosition;
        }
        public static float ScaleSingleFloored(float original, float scale)
        {
            float scaledSingle;

            if( original == 0 )
                return 0;

            if(original > 0)
            {
                scaledSingle = (original / scale);
                scaledSingle -= scaledSingle % 1.0f;
            }
            else
            {
                scaledSingle = (original / scale);  
                scaledSingle -= scaledSingle % 1.0f;

                if( original % scale != 0 ) 
                    scaledSingle -= 1;
                
            }

            return scaledSingle;
        }

        public static Vector3 AbsoluteToRelative(Vector3 position, Vector3 origin)
        {
            return position - origin;
        }
    }
}

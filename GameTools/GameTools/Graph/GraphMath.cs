using System;

using Microsoft.Xna.Framework;

namespace GameTools.Graph
{
    public static class GraphMath
    {
        public static double DistanceBetweenPoints(Point A, Point B)
        {
            int X = B.X - A.X;
            int Y = B.Y - A.Y;

            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }
        public static double DistanceBetweenVector2s(Vector2 A, Vector2 B)
        {
            double X = B.X - A.X;
            double Y = B.Y - A.Y;

            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }
        public static double CosineInterpolate(double a, double b, double amountGreaterThanA)
        {
            double angle = amountGreaterThanA * Math.PI;

            double weightOfB = (1.0 - Math.Cos(angle)) * 0.5;

            return a * (1.0 - weightOfB) + b * weightOfB;
        }

        public static double LinearInterpolate(double a, double b, double amountGreaterThanA)
        {
            return a * (1 - amountGreaterThanA) + b * amountGreaterThanA;
        }
        public static float LinearInterpolateFloat(float a, float b, float amountGreaterThanA)
        {
            return a * (1 - amountGreaterThanA) + b * amountGreaterThanA;
        }
        public static Vector2 AngleBetweenNorms(Vector3 firstVector, Vector3 secondVector)
        {
            Vector2 rotation;
            float angle = (float)Math.Acos(Vector3.Dot(firstVector,secondVector));
            Vector3 axis = Vector3.Cross(firstVector, secondVector);
                        
            Matrix rotationMatrix = Matrix.CreateFromAxisAngle(axis,angle);
            rotation.X = (float)Math.Acos(rotationMatrix.M22);
            rotation.Y = (float)Math.Acos(rotationMatrix.M11);

            if( float.IsNaN(rotation.X) )
                rotation.X = 0;
            if( float.IsNaN(rotation.Y) )
                rotation.Y = 0;

            return rotation;
        }
        public static float AngleBetweenNorms(Vector2 firstVector, Vector2 secondVector)
        {
            float angle;

            angle = (float)(Math.Atan2(secondVector.Y, secondVector.X) - Math.Atan2(firstVector.Y, firstVector.X));

            if(float.IsNaN(angle))
                angle = 0;

            return angle;
        }
    }
}

﻿using System;

using Microsoft.Xna.Framework;

namespace GameTools.Graph
{
    static class GraphMath
    {
        static public double DistanceBetweenPoints(Point A, Point B)
        {
            int X = B.X - A.X;
            int Y = B.Y - A.Y;

            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }
        static public double DistanceBetweenVector2s(Vector2 A, Vector2 B)
        {
            double X = B.X - A.X;
            double Y = B.Y - A.Y;

            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }
        static public double CosineInterpolate(double a, double b, double amountGreaterThanA)
        {
            double angle = amountGreaterThanA * Math.PI;

            double weightOfB = (1.0 - Math.Cos(angle)) * 0.5;

            return a * (1.0 - weightOfB) + b * weightOfB;
        }

        static public double LinearInterpolate(double a, double b, double amountGreaterThanA)
        {
            return a * (1 - amountGreaterThanA) + b * amountGreaterThanA;
        }
        static public float LinearInterpolateFloat(float a, float b, float amountGreaterThanA)
        {
            return a * (1 - amountGreaterThanA) + b * amountGreaterThanA;
        }

        internal static float Dot(Vector2 firstVector, Vector2 secondVector)
        {
            return firstVector.X * secondVector.X + firstVector.Y * secondVector.Y;
        }

        internal static float Dot(Vector3 firstVector, Vector3 secondVector)
        {
            return firstVector.X * secondVector.X + firstVector.Y * secondVector.Y + firstVector.Z + secondVector.Z;
        }
    }
}

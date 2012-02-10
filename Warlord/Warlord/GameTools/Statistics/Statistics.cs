using System;
using Microsoft.Xna.Framework;

namespace GameTools.Statistics
{
    class Statistics
    {
        public static Vector3 Adverage(Vector3[] vectorArray)
        {
            Vector3 sum;

            int numberCount = vectorArray.Length;

            sum = Vector3.Zero;
            for(int index = 0; index < vectorArray.Length; index++)
                sum += vectorArray[index];

            return sum / numberCount;
        }
        public static float Adverage(float[] numberArray)
        {
            float sum;

            int numberCount = numberArray.Length;

            sum = 0;
            for(int index = 0; index < numberArray.Length; index++)
                sum += numberArray[index];

            return sum / numberCount;
        }
        public static double Adverage(double[] numberArray)
        {
            double sum;

            int numberCount = numberArray.Length;

            sum = 0;
            for(int index = 0; index < numberArray.Length; index++)
                sum += numberArray[index];

            return sum / numberCount;
        }

        public static double StdDeviation(double[] numberArray)
        {
            double adverage = Adverage(numberArray);
            int numberCount = numberArray.Length;

            double[] sumOfDifference = new double[numberArray.Length];

            for(int index = 0; index < numberCount; index++)
                sumOfDifference[index] += Math.Pow(numberArray[index] - adverage, 2);

            return Math.Sqrt(Adverage(sumOfDifference));
        }
    }
}

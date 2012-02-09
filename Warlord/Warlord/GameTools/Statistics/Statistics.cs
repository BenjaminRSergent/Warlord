using System;

namespace GameTools.Statistics
{
    class Statistics
    {
        static public double Adverage(double[] numberArray)
        {
            double sum;

            int numberCount = numberArray.Length;

            sum = 0;
            for(int index = 0; index < numberArray.Length; index++)
                sum += numberArray[index];

            return sum / numberCount;
        }

        static public double StdDeviation(double[] numberArray)
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

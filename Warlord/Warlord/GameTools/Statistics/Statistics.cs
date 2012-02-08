using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.GameTools.Statistics
{
    class Statistics
    {
        static public double Adverage(double[] numberArray)
        {
            double sum;

            int numberCount = numberArray.Length;

            sum = 0;
            foreach(double number in numberArray)
                sum += number;

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

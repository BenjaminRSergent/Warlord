using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Statistics;

namespace Warlord.GameTools
{
    public class FloatSmoother
    {
        int smoothIndex;
        float[] mostRecentValues;
        float smoothedValue;        

        public FloatSmoother( int sizeOfSmoothArray )
        {
            smoothIndex = 0;
            mostRecentValues = new float[sizeOfSmoothArray];
        }

        public void AddValue( float value )
        {
            mostRecentValues[smoothIndex] = value;

            smoothedValue = Statistics.Adverage(mostRecentValues);
            IncreaseIndex( );
        }
        public void IncreaseIndex( )
        {
            smoothIndex++;

            if( smoothIndex > mostRecentValues.Length -1)
                smoothIndex = 0;
        }
        public float SmoothedValue
        {
            get { return smoothedValue; }
        }
    }
}

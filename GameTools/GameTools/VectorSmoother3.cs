using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Statistics;
using Microsoft.Xna.Framework;

namespace Warlord.GameTools
{
    public class VectorSmoother3
    {
        int smoothIndex;
        Vector3[] mostRecentValues;
        Vector3 smoothedValue;        

        public VectorSmoother3( int sizeOfSmoothArray )
        {
            smoothIndex = 0;
            mostRecentValues = new Vector3[sizeOfSmoothArray];
        }

        public void AddValue( Vector3 value )
        {
            mostRecentValues[smoothIndex] = value;

            smoothedValue = Statistics.Adverage(mostRecentValues);
            IncreaseIndex( );
        }
        public void IncreaseIndex( )
        {
            smoothIndex++;
            if( smoothIndex > mostRecentValues.Length-1 )
                smoothIndex = 0;
        }
        public Vector3 SmoothedValue
        {
            get { return smoothedValue; }
        }
    }
}

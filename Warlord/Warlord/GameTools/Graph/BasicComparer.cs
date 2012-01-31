using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTools.Graph
{
    class BasicComparer<T> : IEqualityComparer<T>
    {
        public bool Equals( T one, T two )
        {
            return one.Equals(two);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode( );
        }
    }
}

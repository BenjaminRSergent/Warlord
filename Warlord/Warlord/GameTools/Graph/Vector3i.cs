using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameTools.Graph
{
    class Vector3i
    {     
        public Vector3i( )
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
        public Vector3i( int X, int Y, int Z )
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public Vector3i( Vector3i source )
        {
            this.X = source.X;
            this.Y = source.Y;
            this.Z = source.Z;
        }
        public Vector3i( Vector3 source )
        {
            this.X = (int)source.X;
            this.Y = (int)source.Y;
            this.Z = (int)source.Z;
        }
        public static implicit operator Vector3i( Vector3 source )
        {
            Vector3i vector = new Vector3i( );
            vector.X = (int)source.X;
            vector.Y = (int)source.Y;
            vector.Z = (int)source.Z;

            return vector;
        }
        public float AngleBetween( Vector3i otherVector )
        {
            Vector3f thisNorm = GetNormalized( );
            Vector3f otherNorm = otherVector.GetNormalized( );
            float dot = thisNorm.DotProduct( otherNorm );

            return (float)Math.Acos(dot);
        }
        public float DotProduct(  Vector3i otherVector )
        {
            return this.X * otherVector.X + this.Y * otherVector.Y + this.Z * otherVector.Z;
        }  
        public Vector3f GetNormalized( )
        {
            Vector3f normVec = new Vector3f( X, Y, Z );
            normVec.Normalize( );

            return normVec;
        }

        static public Vector3i operator+( Vector3i leftVector, Vector3i rightVector )
        {
            Vector3i sumVector = new Vector3i( leftVector );

            sumVector.X += rightVector.X;
            sumVector.Y += rightVector.Y;
            sumVector.Z += rightVector.Z;

            return sumVector;
        }
        static public Vector3i operator-( Vector3i leftVector, Vector3i rightVector )
        {
            Vector3i differenceVector = new Vector3i( leftVector );

            differenceVector.X -= rightVector.X;
            differenceVector.Y -= rightVector.Y;
            differenceVector.Z -= rightVector.Z;

            return differenceVector;
        }
        static public Vector3i operator*( int number, Vector3i vector )
        {
            Vector3i scaledVector = new Vector3i( vector );

            scaledVector.X *= number;
            scaledVector.Y *= number;
            scaledVector.Z *= number;

            return scaledVector;
        }
        static public float operator*( Vector3i leftVector, Vector3i rightVector )
        {
            return leftVector.DotProduct( rightVector );
        }        
        static public bool operator==( Vector3i leftVector, Vector3i rightVector )
        {
            return leftVector.X == rightVector.X &&
                   leftVector.Y == rightVector.Y &&
                   leftVector.Z == rightVector.Z;
        }
        static public bool operator!=( Vector3i leftVector, Vector3i rightVector )
        {
            return leftVector.X != rightVector.X ||
                   leftVector.Y != rightVector.Y ||
                   leftVector.Z != rightVector.Z;
        }
        public override bool Equals(object obj)
        {
            if( obj is Vector3i )
            {
                return this == (Vector3i)obj;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }      

        public float Length
        { 
            get
            {
                return (float)Math.Sqrt(LengthSquared);
            }
        }
        public int LengthSquared
        { 
            get
            {
                return X*X + Y*Y + Z*Z;
            }
        }
        public Vector3 ToVector3
        {
            get
            {
                return new Vector3( X, Y, Z );
            }
        }
        static public Vector3i Zero
        {
            get
            {
                return new Vector3i( 0, 0, 0 );
            }
        }
        static public Vector3i One
        {
            get
            {
                return new Vector3i( 1, 1, 1 );
            }
        }
        static public Vector3i XIncreasing
        {
            get
            {
                return new Vector3i( 1, 0, 0 );
            }
        }
        static public Vector3i YIncreasing
        {
            get
            {
                return new Vector3i( 0, 1, 0 );
            }
        }
        static public Vector3i ZIncreasing
        {
            get
            {
                return new Vector3i( 0, 0, 1 );
            }
        }
        static public Vector3i XDecreasing
        {
            get
            {
                return new Vector3i( -1, 0, 0 );
            }
        }
        static public Vector3i YDecreasing
        {
            get
            {
                return new Vector3i( 0, -1, 0 );
            }
        }
        static public Vector3i ZDecreasing
        {
            get
            {
                return new Vector3i( 0, 0, -1 );
            }
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
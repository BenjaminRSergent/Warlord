using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameTools.Graph
{
    class Vector3f
    {     
        public Vector3f( )
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
        public Vector3f( float X, float Y, float Z )
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public Vector3f( Vector3f source )
        {
            this.X = source.X;
            this.Y = source.Y;
            this.Z = source.Z;
        }
        public Vector3f( Vector3 source )
        {
            this.X = source.X;
            this.Y = source.Y;
            this.Z = source.Z;
        }
        public static implicit operator Vector3f( Vector3 source )
        {
            Vector3f vector = new Vector3f( );
            vector.X = source.X;
            vector.Y = source.Y;
            vector.Z = source.Z;

            return vector;
        }
        public float AngleBetween( Vector3f otherVector )
        {
            Vector3f thisNorm = GetNormalized( );
            Vector3f otherNorm = otherVector.GetNormalized( );
            float dot = thisNorm.DotProduct( otherNorm );

            return (float)Math.Acos(dot);
        }
        public float DotProduct(  Vector3f otherVector )
        {
            return this.X * otherVector.X + this.Y * otherVector.Y + this.Z * otherVector.Z;
        }  
        public Vector3f GetNormalized( )
        {
            Vector3f normVec = new Vector3f( this );
            normVec.Normalize( );

            return normVec;
        }
        public void Normalize( )
        {
            float length = Length;

            X = X/length;
            Y = Y/length;
            Z = Z/length;
        }

        static public Vector3f operator+( Vector3f leftVector, Vector3f rightVector )
        {
            Vector3f sumVector = new Vector3f( leftVector );

            sumVector.X += rightVector.X;
            sumVector.Y += rightVector.Y;
            sumVector.Z += rightVector.Z;

            return sumVector;
        }
        static public Vector3f operator-( Vector3f leftVector, Vector3f rightVector )
        {
            Vector3f differenceVector = new Vector3f( leftVector );

            differenceVector.X -= rightVector.X;
            differenceVector.Y -= rightVector.Y;
            differenceVector.Z -= rightVector.Z;

            return differenceVector;
        }
        static public Vector3f operator*( float number, Vector3f vector )
        {
            Vector3f scaledVector = new Vector3f( vector );

            scaledVector.X *= number;
            scaledVector.Y *= number;
            scaledVector.Z *= number;

            return scaledVector;
        }
        static public float operator*( Vector3f leftVector, Vector3f rightVector )
        {
            return leftVector.DotProduct( rightVector );
        }        
        static public bool operator==( Vector3f leftVector, Vector3f rightVector )
        {
            return leftVector.X == rightVector.X &&
                   leftVector.Y == rightVector.Y &&
                   leftVector.Z == rightVector.Z;
        }
        static public bool operator!=( Vector3f leftVector, Vector3f rightVector )
        {
            return leftVector.X != rightVector.X ||
                   leftVector.Y != rightVector.Y ||
                   leftVector.Z != rightVector.Z;
        }
        public override bool Equals(object obj)
        {
            if( obj is Vector3f )
            {
                return this == (Vector3f)obj;
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
        public float LengthSquared
        { 
            get
            {
                return X*X + Y*Y + Z*Z;
            }
        }
        public Vector3i ToIntVector( )
        {
            return new Vector3i( (int)X, (int)Y, (int)Z );
        }
        static public Vector3f Zero
        {
            get
            {
                return new Vector3f( 0, 0, 0 );
            }
        }
        static public Vector3f One
        {
            get
            {
                return new Vector3f( 1, 1, 1 );
            }
        }
        static public Vector3f XIncreasing
        {
            get
            {
                return new Vector3f( 1, 0, 0 );
            }
        }
        static public Vector3f YIncreasing
        {
            get
            {
                return new Vector3f( 0, 1, 0 );
            }
        }
        static public Vector3f ZIncreasing
        {
            get
            {
                return new Vector3f( 0, 0, 1 );
            }
        }
        static public Vector3f XDecreasing
        {
            get
            {
                return new Vector3f( -1, 0, 0 );
            }
        }
        static public Vector3f YDecreasing
        {
            get
            {
                return new Vector3f( 0, -1, 0 );
            }
        }
        static public Vector3f ZDecreasing
        {
            get
            {
                return new Vector3f( 0, 0, -1 );
            }
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 ToVector3 
        { 
            get
            { 
                return new Vector3(this.X, this.Y, this.Z);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameTools.Graph
{
    class Vector2f
    {     
        public Vector2f( )
        {
            this.X = 0;
            this.Y = 0;
        }
        public Vector2f( float X, float Y )
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector2f( Vector2f source )
        {
            this.X = source.X;
            this.Y = source.Y;
        }
        public Vector2f( Vector2 source )
        {
            this.X = source.X;
            this.Y = source.Y;
        }
        public static implicit operator Vector2f( Vector2 source )
        {
            Vector2f vector = new Vector2f( );
            vector.X = source.X;
            vector.Y = source.Y;

            return vector;
        }
        public float AngleBetween( Vector2f otherVector )
        {
            Vector2f thisNorm = GetNormalized( );
            Vector2f otherNorm = otherVector.GetNormalized( );
            float dot = thisNorm.DotProduct( otherNorm );

            return (float)Math.Acos(dot);
        }
        public float DotProduct(  Vector2f otherVector )
        {
            return this.X * otherVector.X + this.Y * otherVector.Y;
        }  
        public Vector2f GetNormalized( )
        {
            Vector2f normVec = new Vector2f( this );
            normVec.Normalize( );

            return normVec;
        }
        public void Normalize( )
        {
            float length = Length;

            X = X/length;
            Y = Y/length;
        }

        static public Vector2f operator+( Vector2f leftVector, Vector2f rightVector )
        {
            Vector2f sumVector = new Vector2f( leftVector );

            sumVector.X += rightVector.X;
            sumVector.Y += rightVector.Y;

            return sumVector;
        }
        static public Vector2f operator-( Vector2f leftVector, Vector2f rightVector )
        {
            Vector2f differenceVector = new Vector2f( leftVector );

            differenceVector.X -= rightVector.X;
            differenceVector.Y -= rightVector.Y;

            return differenceVector;
        }
        static public Vector2f operator*( float number, Vector2f vector )
        {
            Vector2f scaledVector = new Vector2f( vector );

            scaledVector.X *= number;
            scaledVector.Y *= number;

            return scaledVector;
        }
        static public float operator*( Vector2f leftVector, Vector2f rightVector )
        {
            return leftVector.DotProduct( rightVector );
        }        
        static public bool operator==( Vector2f leftVector, Vector2f rightVector )
        {
            return leftVector.X == rightVector.X &&
                   leftVector.Y == rightVector.Y;
        }
        static public bool operator!=( Vector2f leftVector, Vector2f rightVector )
        {
            return leftVector.X != rightVector.X ||
                   leftVector.Y != rightVector.Y;
        }
        public override bool Equals(object obj)
        {
            if( obj is Vector2f )
            {
                return this == (Vector2f)obj;
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
            return "(" + X + "," + Y + ")";
        }       

        public float Length
        { 
            get
            {
                return (float)Math.Sqrt(X*X + Y*Y);
            }
        }
        public float LengthSquared
        { 
            get
            {
                return X*X + Y*Y;
            }
        }

        static public Vector2f Zero
        {
            get
            {
                return new Vector2f( 0, 0 );
            }
        }
        static public Vector2f One
        {
            get
            {
                return new Vector2f( 1, 1 );
            }
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}
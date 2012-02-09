using System;

using Microsoft.Xna.Framework;

namespace GameTools.Graph
{
    struct Vector2i
    {
        int x;
        int y;

        public Vector2i(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public Vector2i(Vector2i source)
        {
            x = source.X;
            y = source.Y;
        }
        public Vector2i(Vector2 source)
        {
            x = (int)source.X;
            y = (int)source.Y;
        }
        public static implicit operator Vector2i(Vector2 source)
        {
            Vector2i vector = new Vector2i();
            vector.x = (int)source.X;
            vector.y = (int)source.Y;

            return vector;
        }
        public float AngleBetween(Vector2i otherVector)
        {
            Vector2 thisNorm = GetNormalized();
            Vector2 otherNorm = otherVector.GetNormalized();
            float dot = GraphMath.Dot(thisNorm, otherNorm);

            return (float)Math.Acos(dot);
        }
        public float DotProduct(Vector2i otherVector)
        {
            return this.X * otherVector.X + this.Y * otherVector.Y;
        }
        public Vector2 GetNormalized()
        {
            Vector2 normVec = new Vector2(this.X, this.Y);
            normVec.Normalize();

            return normVec;
        }
        static public Vector2i operator +(Vector2i leftVector, Vector2i rightVector)
        {
            Vector2i sumVector = new Vector2i(leftVector);

            sumVector.X += rightVector.X;
            sumVector.Y += rightVector.Y;

            return sumVector;
        }
        static public Vector2i operator -(Vector2i leftVector, Vector2i rightVector)
        {
            Vector2i differenceVector = new Vector2i(leftVector);

            differenceVector.X -= rightVector.X;
            differenceVector.Y -= rightVector.Y;

            return differenceVector;
        }
        static public Vector2i operator *(int number, Vector2i vector)
        {
            Vector2i scaledVector = new Vector2i(vector);

            scaledVector.X *= number;
            scaledVector.Y *= number;

            return scaledVector;
        }
        static public float operator *(Vector2i leftVector, Vector2i rightVector)
        {
            return leftVector.DotProduct(rightVector);
        }
        static public bool operator ==(Vector2i leftVector, Vector2i rightVector)
        {
            return leftVector.X == rightVector.X &&
                   leftVector.Y == rightVector.Y;
        }
        static public bool operator !=(Vector2i leftVector, Vector2i rightVector)
        {
            return leftVector.X != rightVector.X ||
                   leftVector.Y != rightVector.Y;
        }
        public override bool Equals(object obj)
        {
            if(obj is Vector2i)
            {
                return this == (Vector2i)obj;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return x ^ 73856093 ^ y ^ 19349663 ;
        }
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y);
            }
        }
        public int LengthSquared
        {
            get
            {
                return X * X + Y * Y;
            }
        }
        public Vector2 ToVector2
        {
            get
            {
                return new Vector2(X, Y);
            }
        }
        static public Vector2i Zero
        {
            get
            {
                return new Vector2i(0, 0);
            }
        }
        static public Vector2i One
        {
            get
            {
                return new Vector2i(1, 1);
            }
        }

        public int X 
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
    }
}
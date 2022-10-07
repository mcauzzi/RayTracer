using System;
using GlobalConstants;

namespace MainLib
{
    public class MathTuple : IEquatable<MathTuple>
    {
        public MathTuple(double x, double y, double z, double w)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public double W { get; set; }

        public double this[int row]
        {
            get
            {
                return row switch
                {
                    0 => X,
                    1 => Y,
                    2 => Z,
                    3 => W,
                    _ => -1
                };
            }
            set
            {
                switch (row)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    case 3:
                        W = value;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public static MathTuple GetPoint(double x, double y, double z)
        {
            return new MathTuple(w: 1, x: x, y: y, z: z);
        }

        public static MathTuple GetVector(double x, double y, double z)
        {
            return new MathTuple(w: 0, x: x, y: y, z: z);
        }

        public bool IsPoint()
        {
            return Math.Abs(W - 1) < double.Epsilon;
        }

        public bool IsVector()
        {
            return W == 0;
        }

        public double GetMagnitude()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2) + Math.Pow(W, 2));
        }

        public MathTuple Normalize()
        {
            var mag = GetMagnitude();
            return new MathTuple(X / mag, Y / mag, Z / mag, W / mag);
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}, {nameof(W)}: {W}";
        }

        public MathTuple Reflect(MathTuple normal)
        {
            return this - normal * 2 * DotProduct(this, normal);
        }

        #region Operators

        private static MathTuple Add(MathTuple left, MathTuple right)
        {
            return new MathTuple(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        public static MathTuple operator +(MathTuple left, MathTuple right)
        {
            return MathTuple.Add(left, right);
        }

        private static MathTuple Subtract(MathTuple left, MathTuple right)
        {
            return new MathTuple(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        public static MathTuple operator -(MathTuple left, MathTuple right)
        {
            return MathTuple.Subtract(left, right);
        }

        public static MathTuple operator -(MathTuple right)
        {
            return MathTuple.Subtract(new MathTuple(0, 0, 0, 0), right);
        }

        public static MathTuple operator *(MathTuple tuple, double scalar)
        {
            return new MathTuple(tuple.X * scalar, tuple.Y * scalar, tuple.Z * scalar, tuple.W * scalar);
        }

        public static MathTuple operator /(MathTuple tuple, double scalar)
        {
            return new MathTuple(tuple.X / scalar, tuple.Y / scalar, tuple.Z / scalar, tuple.W / scalar);
        }

        public static double DotProduct(MathTuple left, MathTuple right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }

        public static MathTuple CrossProduct(MathTuple a, MathTuple b)
        {
            return GetVector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        #endregion

        #region Equality

        public bool Equals(MathTuple other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Math.Abs(X - other.X) < Constants.Epsilon && Math.Abs(Y - other.Y) < Constants.Epsilon &&
                   Math.Abs(Z - other.Z) < Constants.Epsilon && Math.Abs(W - other.W) < Constants.Epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MathTuple)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(MathTuple left, MathTuple right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MathTuple left, MathTuple right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
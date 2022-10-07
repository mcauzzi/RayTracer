using System;

namespace MainLib
{
    public static class Transforms
    {
        public static Matrix GetTranslationMatrix(double x, double y, double z)
        {
            return new Matrix(new[,]
            {
                { 1, 0, 0, x },
                { 0, 1, 0, y },
                { 0, 0, 1, z },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix GetScalingMatrix(double x, double y, double z)
        {
            return new Matrix(new double[,]
            {
                { x, 0, 0, 0 },
                { 0, y, 0, 0 },
                { 0, 0, z, 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix RotateX(double radians)
        {
            return new Matrix(new double[,]
            {
                { 1, 0, 0, 0 },
                { 0, Math.Cos(radians), -Math.Sin(radians), 0 },
                { 0, Math.Sin(radians), Math.Cos(radians), 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix RotateY(double radians)
        {
            return new Matrix(new double[,]
            {
                { Math.Cos(radians), 0, Math.Sin(radians), 0 },
                { 0, 1, 0, 0 },
                { -Math.Sin(radians), 0, Math.Cos(radians), 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix RotateZ(double radians)
        {
            return new Matrix(new double[,]
            {
                { Math.Cos(radians), -Math.Sin(radians), 0, 0 },
                { Math.Sin(radians), Math.Cos(radians), 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix Shear(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            return new Matrix(new double[,]
            {
                { 1, xy, xz, 0 },
                { yx, 1, yz, 0 },
                { zx, zy, 1, 0 },
                { 0, 0, 0, 1 }
            });
        }

        public static Matrix ViewTransform(MathTuple from, MathTuple to, MathTuple up)
        {
            var forward    = (to - from).Normalize();
            var upn        = up.Normalize();
            var left       = MathTuple.CrossProduct(forward, upn);
            var trueUp     = MathTuple.CrossProduct(left,    forward);
            var negForward = -forward;
            var orientation = new Matrix(new double[,]
            {
                { left.X, left.Y, left.Z, 0 },
                { trueUp.X, trueUp.Y, trueUp.Z, 0 },
                { negForward.X, negForward.Y, negForward.Z, 0 },
                { 0, 0, 0, 1 }
            });
            var negativeFrom = -from;
            return orientation * GetTranslationMatrix(negativeFrom.X, negativeFrom.Y, negativeFrom.Z);
        }
    }
}
using System;
using MainLib;
using Xunit;

namespace MainLibTests
{
    public class TupleTests
    {
        [Fact]
        public void IsPoint()
        {
            var point = new MathTuple(4.3, -4.2, 3.1, 1);
            Assert.True(point.IsPoint());
            Assert.False(point.IsVector());
            Assert.Equal(4.3,  point.X);
            Assert.Equal(-4.2, point.Y);
            Assert.Equal(3.1,  point.Z);
        }

        [Fact]
        public void IsVector()
        {
            var vector = new MathTuple(4.3, -4.2, 3.1, 0);
            Assert.False(vector.IsPoint());
            Assert.True(vector.IsVector());
            Assert.Equal(4.3,  vector.X);
            Assert.Equal(-4.2, vector.Y);
            Assert.Equal(3.1,  vector.Z);
        }

        [Fact]
        public void FactoryPoint()
        {
            var factoryPoint = MathTuple.GetPoint(4, -4, 3);
            Assert.True(factoryPoint.IsPoint());
            Assert.False(factoryPoint.IsVector());
            Assert.Equal(4,  factoryPoint.X);
            Assert.Equal(-4, factoryPoint.Y);
            Assert.Equal(3,  factoryPoint.Z);
        }

        [Fact]
        public void FactoryVector()
        {
            var factoryVector = MathTuple.GetVector(4, -4, 3);
            Assert.False(factoryVector.IsPoint());
            Assert.True(factoryVector.IsVector());
            Assert.Equal(4,  factoryVector.X);
            Assert.Equal(-4, factoryVector.Y);
            Assert.Equal(3,  factoryVector.Z);
        }

        [Fact]
        public void Add()
        {
            var first  = new MathTuple(3,  -2, 5, 1);
            var second = new MathTuple(-2, 3,  1, 0);
            var res    = first + second;
            Assert.Equal(res, new MathTuple(1, 1, 6, 1));
        }

        [Fact]
        public void SubtractTwoPoints()
        {
            var first  = MathTuple.GetPoint(3, 2, 1);
            var second = MathTuple.GetPoint(5, 6, 7);
            var res    = first - second;
            Assert.Equal(res, MathTuple.GetVector(-2, -4, -6));
        }

        [Fact]
        public void SubtractTwoVectors()
        {
            var first  = MathTuple.GetVector(3, 2, 1);
            var second = MathTuple.GetVector(5, 6, 7);
            var res    = first - second;
            Assert.Equal(res, MathTuple.GetVector(-2, -4, -6));
        }

        [Fact]
        public void SubtractVectorFromPoint()
        {
            var first  = MathTuple.GetPoint(3, 2, 1);
            var second = MathTuple.GetVector(5, 6, 7);
            var res    = first - second;
            Assert.Equal(res, MathTuple.GetPoint(-2, -4, -6));
        }

        [Fact]
        public void SubtractVectorFromZero()
        {
            var first  = MathTuple.GetVector(0, 0,  0);
            var second = MathTuple.GetVector(1, -2, 3);
            var res    = first - second;
            Assert.Equal(res, MathTuple.GetVector(-1, 2, -3));
        }

        [Fact]
        public void Negation()
        {
            var first = new MathTuple(1,           -2, 3,  -4);
            Assert.Equal(-first, new MathTuple(-1, 2,  -3, 4));
        }

        [Fact]
        public void ScalarMultiplication()
        {
            var first = new MathTuple(1,                 -2, 3,    -4);
            Assert.Equal(first * 3.5, new MathTuple(3.5, -7, 10.5, -14));
        }

        [Fact]
        public void FractionMultiplication()
        {
            var first = new MathTuple(1,                 -2, 3,   -4);
            Assert.Equal(first * 0.5, new MathTuple(0.5, -1, 1.5, -2));
        }

        [Fact]
        public void Magnitude()
        {
            Assert.Equal(1,             MathTuple.GetVector(1,  0,  0).GetMagnitude());
            Assert.Equal(1,             MathTuple.GetVector(0,  1,  0).GetMagnitude());
            Assert.Equal(1,             MathTuple.GetVector(0,  0,  1).GetMagnitude());
            Assert.Equal(Math.Sqrt(14), MathTuple.GetVector(1,  2,  3).GetMagnitude());
            Assert.Equal(Math.Sqrt(14), MathTuple.GetVector(-1, -2, -3).GetMagnitude());
        }

        [Fact]
        public void Normalize()
        {
            Assert.Equal(MathTuple.GetVector(4, 0, 0).Normalize(), MathTuple.GetVector(1, 0, 0));
            var normalizedVec = MathTuple.GetVector(1, 2, 3).Normalize();
            Assert.Equal(normalizedVec, MathTuple.GetVector(1 / Math.Sqrt(14), 2 / Math.Sqrt(14), 3 / Math.Sqrt(14)));
            Assert.Equal(1,             normalizedVec.GetMagnitude());
        }

        [Fact]
        public void DotProduct()
        {
            var a = MathTuple.GetVector(1, 2, 3);
            var b = MathTuple.GetVector(2, 3, 4);
            Assert.Equal(20, MathTuple.DotProduct(a, b));
        }

        [Fact]
        public void CrossProduct()
        {
            var a = MathTuple.GetVector(1,       2,  3);
            var b = MathTuple.GetVector(2,       3,  4);
            Assert.Equal(MathTuple.GetVector(-1, 2,  -1), MathTuple.CrossProduct(a, b));
            Assert.Equal(MathTuple.GetVector(1,  -2, 1),  MathTuple.CrossProduct(b, a));
        }

        [Fact]
        public void VectorReflect45Deg()
        {
            var v   = MathTuple.GetVector(1, -1, 0);
            var n   = MathTuple.GetVector(0, 1,  0);
            var res = v.Reflect(n);
            Assert.Equal(MathTuple.GetVector(1, 1, 0), res);
        }

        [Fact]
        public void VectorReflectSlanted()
        {
            var v   = MathTuple.GetVector(0,                -1,               0);
            var n   = MathTuple.GetVector(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0);
            var res = v.Reflect(n);
            Assert.Equal(MathTuple.GetVector(1, 0, 0), res);
        }
    }
}
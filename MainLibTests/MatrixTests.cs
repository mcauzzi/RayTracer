using MainLib;
using Xunit;

namespace MainLibTests;

public class MatrixTests
{
    [Fact]
    public void Test4x4()
    {
        var a = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5.5, 6.5, 7.5, 8.5 }, { 9, 10, 11, 12 }, { 13.5, 14.5, 15.5, 16.5 } });

        Assert.Equal(1, a[0, 0]);
        Assert.Equal(4, a[0, 3]);
        Assert.Equal(5.5, a[1, 0]);
        Assert.Equal(7.5, a[1, 2]);
        Assert.Equal(11, a[2, 2]);
        Assert.Equal(13.5, a[3, 0]);
        Assert.Equal(15.5, a[3, 2]);
    }

    [Fact]
    public void Equality()
    {
        var a = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5.5, 6.5, 7.5, 8.5 }, { 9, 10, 11, 12 }, { 13.5, 14.5, 15.5, 16.5 } });
        var b = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5.5, 6.5, 7.5, 8.5 }, { 9, 10, 11, 12 }, { 13.5, 14.5, 15.5, 16.5 } });
        Assert.Equal(a, b);
    }

    [Fact]
    public void Inequality()
    {
        var a = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5.5, 6.5, 7.5, 8.5 }, { 9, 10, 11, 12 }, { 13.5, 14.5, 15.5, 16.5 } });
        var b = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5.5, 6.5, 7.5, 8.5 }, { 1, 2, 3, 4 }, { 13.5, 14.5, 15.5, 16.5 } });
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Product()
    {
        var a = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5, 6, 7, 8 }, { 9, 8, 7, 6 }, { 5, 4, 3, 2 } });
        var b = new Matrix(new[,]
            { { -2, 1, 2, 3 }, { 3, 2.0, 1, -1 }, { 4, 3, 6, 5 }, { 1, 2, 7, 8 } });
        var res = new Matrix(new[,]
            { { 20, 22, 50, 48 }, { 44, 54, 114, 108 }, { 40, 58, 110, 102 }, { 16, 26, 46, 42.0 } });
        Assert.Equal(res, a * b);
    }

    [Fact]
    public void TupleProduct()
    {
        var a = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 2, 4, 4, 2 }, { 8, 6, 4, 1 }, { 0, 0, 0, 1 } });
        var b = new MathTuple(1, 2, 3, 1);

        Assert.Equal(new MathTuple(18, 24, 33, 1), a * b);
    }

    [Fact]
    public void ProductIdentity()
    {
        var a = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5, 6, 7, 8 }, { 9, 8, 7, 6 }, { 5, 4, 3, 2 } });
        var b = new Matrix(new double[,]
            { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        var res = new Matrix(new[,]
            { { 1.0, 2.0, 3.0, 4 }, { 5, 6, 7, 8 }, { 9, 8, 7, 6 }, { 5, 4, 3, 2 } });
        Assert.Equal(res, a * b);
    }

    [Fact]
    public void TupleProductIdentity()
    {
        var a = new Matrix(new double[,]
            { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        var b = new MathTuple(1, 2, 3, 1);

        Assert.Equal(new MathTuple(1, 2, 3, 1), a * b);
    }

    [Fact]
    public void IdentityTranspose()
    {
        var a = new Matrix(new double[,]
            { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        var b = a.Transpose();

        Assert.Equal(new Matrix(new double[,]
            { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } }), b);
    }

    [Fact]
    public void Transpose()
    {
        var a = new Matrix(new double[,]
            { { 0, 9, 3, 0 }, { 9, 8, 0, 8 }, { 1, 8, 5, 3 }, { 0, 0, 5, 8 } });
        var b = a.Transpose();

        Assert.Equal(new Matrix(new double[,]
            { { 0, 9, 1, 0 }, { 9, 8, 8, 0 }, { 3, 0, 5, 5 }, { 0, 8, 3, 8 } }), b);
    }

    [Fact]
    public void Submatrix()
    {
        var a = new Matrix(new double[,]
            { { 1, 5, 0 }, { -3, 2, 7 }, { 0, 6, -3 } });
        var b = a.GetSubMatrix(0, 2);

        Assert.Equal(new Matrix(new double[,]
            { { -3, 2 }, { 0, 6 } }), b);
    }

    [Fact]
    public void Determinant2x2()
    {
        var a = new Matrix(new double[,]
            { { 1, 5 }, { -3, 2 } });
        Assert.Equal(a.GetDeterminant(), 17);
    }

    [Fact]
    public void Minor()
    {
        var a = new Matrix(new double[,]
            { { 3, 5, 0 }, { 2, -1, -7 }, { 6, -1, 5 } });
        var b = a.GetSubMatrix(1, 0);
        Assert.Equal(25, b.GetDeterminant());
        Assert.Equal(25, a.GetMinor(1, 0));
    }

    [Fact]
    public void Cofactor()
    {
        var a = new Matrix(new double[,]
            { { 3, 5, 0 }, { 2, -1, -7 }, { 6, -1, 5 } });

        Assert.Equal(-12, a.GetMinor(0, 0));
        Assert.Equal(-12, a.GetCofactor(0, 0));
        Assert.Equal(25, a.GetMinor(1, 0));
        Assert.Equal(-25, a.GetCofactor(1, 0));
    }

    [Fact]
    public void Determinant3x3()
    {
        var a = new Matrix(new double[,]
            { { 1, 2, 6 }, { -5, 8, -4 }, { 2, 6, 4 } });

        Assert.Equal(56, a.GetCofactor(0, 0));
        Assert.Equal(12, a.GetCofactor(0, 1));
        Assert.Equal(-46, a.GetCofactor(0, 2));
        Assert.Equal(-196, a.GetDeterminant());
    }

    [Fact]
    public void Determinant4x4()
    {
        var a = new Matrix(new double[,]
            { { -2, -8, 3, 5 }, { -3, 1, 7, 3 }, { 1, 2, -9, 6 }, { -6, 7, 7, -9 } });

        Assert.Equal(690, a.GetCofactor(0, 0));
        Assert.Equal(447, a.GetCofactor(0, 1));
        Assert.Equal(210, a.GetCofactor(0, 2));
        Assert.Equal(51, a.GetCofactor(0, 3));
        Assert.Equal(-4071, a.GetDeterminant());
    }

    [Fact]
    public void Invertible()
    {
        var a = new Matrix(new double[,]
            { { 6, 4, 4, 4 }, { 5, 5, 7, 6 }, { 4, -9, 3, -7 }, { 9, 1, 7, -6 } });

        Assert.True(a.IsInvertible);
    }

    [Fact]
    public void NotInvertible()
    {
        var a = new Matrix(new double[,]
            { { -4, 2, -2, -3 }, { 9, 6, 2, 6 }, { 0, -5, 1, -5 }, { 0, 0, 0, 0 } });
        Assert.False(a.IsInvertible);
    }

    [Fact]
    public void Inverse()
    {
        var a = new Matrix(new double[,]
            { { 8, -5, 9, 2 }, { 7, 5, 6, 1 }, { -6, 0, 9, 6 }, { -3, 0, -9, -4 } });
        var res = new Matrix(new double[,]
        {
            { -0.15385, -0.15385, -0.28205, -0.53846 }, { -0.07692, 0.12308, 0.02564, 0.03077 },
            { 0.35897, 0.35897, 0.43590, 0.92308 }, { -0.69231, -0.69231, -0.76923, -1.92308 }
        });
        Assert.Equal(res, a.GetInverse());
    }

    [Fact]
    public void InverseProduct()
    {
        var a = new Matrix(new double[,]
            { { 3, -0, 7, 3 }, { 3, -8, 2, -9 }, { -4, 4, 4, 1 }, { -6, 5, -1, 1 } });
        var b = new Matrix(new double[,]
            { { 8, 2, 2, 2 }, { 3, -1, 7, 0 }, { 7, 0, 5, 4 }, { 6, -2, 0, 5 } });
        var product = a * b;
        Assert.Equal(product * b.GetInverse(), new Matrix(new double[,]
            { { 3, -0, 7, 3 }, { 3, -8, 2, -9 }, { -4, 4, 4, 1 }, { -6, 5, -1, 1 } }));
    }
}
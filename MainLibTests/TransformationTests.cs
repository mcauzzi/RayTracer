using System;
using MainLib;
using Xunit;

namespace MainLibTests;

public class TransformationTests
{
    [Fact]
    public void Translation()
    {
        var trans = Transforms.GetTranslationMatrix(5, -3, 2);
        Assert.Equal(trans * MathTuple.GetPoint(-3, 4, 5), MathTuple.GetPoint(2, 1, 7));
    }

    [Fact]
    public void InvertedTranslation()
    {
        var trans = Transforms.GetTranslationMatrix(5, -3, 2).GetInverse();
        Assert.Equal(trans * MathTuple.GetPoint(-3, 4, 5), MathTuple.GetPoint(-8, 7, 3));
    }

    [Fact]
    public void Scaling()
    {
        MathTuple tempQualifier = MathTuple.GetPoint(2, 3, 4);
        var       trans         = Transforms.GetScalingMatrix(2, 3, 4);
        Assert.Equal(trans * MathTuple.GetPoint(-4, 6, 8), MathTuple.GetPoint(-8, 18, 32));
    }

    [Fact]
    public void ScalingVector()
    {
        var trans = Transforms.GetScalingMatrix(2, 3, 4);
        Assert.Equal(trans * MathTuple.GetVector(-4, 6, 8), MathTuple.GetVector(-8, 18, 32));
    }

    [Fact]
    public void InvertedScaling()
    {
        var trans = Transforms.GetScalingMatrix(2, 3, 4).GetInverse();
        Assert.Equal(trans * MathTuple.GetPoint(-4, 6, 8), MathTuple.GetPoint(-2, 2, 2));
    }

    [Fact]
    public void Reflection()
    {
        MathTuple tempQualifier = MathTuple.GetPoint(-1, 1, 1);
        var       trans         = Transforms.GetScalingMatrix(-1, 1, 1).GetInverse();
        Assert.Equal(trans * MathTuple.GetPoint(2, 3, 4), MathTuple.GetPoint(-2, 3, 4));
    }

    [Fact]
    public void RotateX()
    {
        MathTuple p           = MathTuple.GetPoint(0, 1, 0);
        var       halfQuarter = Transforms.RotateX(Math.PI / 4);
        var       fullQuarter = Transforms.RotateX(Math.PI / 2);
        Assert.Equal(MathTuple.GetPoint(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2), halfQuarter * p);
        Assert.Equal(MathTuple.GetPoint(0, 0,                1),                fullQuarter * p);
    }

    [Fact]
    public void RotateXInverse()
    {
        MathTuple p           = MathTuple.GetPoint(0, 1, 0);
        var       halfQuarter = Transforms.RotateX(Math.PI / 4).GetInverse();
        Assert.Equal(MathTuple.GetPoint(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2), halfQuarter * p);
    }

    [Fact]
    public void RotateY()
    {
        MathTuple p           = MathTuple.GetPoint(0, 0, 1);
        var       halfQuarter = Transforms.RotateY(Math.PI / 4);
        var       fullQuarter = Transforms.RotateY(Math.PI / 2);
        Assert.Equal(MathTuple.GetPoint(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2), halfQuarter * p);
        Assert.Equal(MathTuple.GetPoint(1,                0, 0),                fullQuarter * p);
    }

    [Fact]
    public void RotateZ()
    {
        MathTuple p           = MathTuple.GetPoint(0, 1, 0);
        var       halfQuarter = Transforms.RotateZ(Math.PI / 4);
        var       fullQuarter = Transforms.RotateZ(Math.PI / 2);
        Assert.Equal(MathTuple.GetPoint(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0), halfQuarter * p);
        Assert.Equal(MathTuple.GetPoint(-1,                0,                0), fullQuarter * p);
    }

    [Fact]
    public void ShearXY()
    {
        var trans = Transforms.Shear(1, 0, 0, 0, 0, 0);
        var p     = MathTuple.GetPoint(2,  3, 4);
        Assert.Equal(MathTuple.GetPoint(5, 3, 4), trans * p);
    }

    [Fact]
    public void ShearXZ()
    {
        var trans = Transforms.Shear(0, 1, 0, 0, 0, 0);
        var p     = MathTuple.GetPoint(2,  3, 4);
        Assert.Equal(MathTuple.GetPoint(6, 3, 4), trans * p);
    }

    [Fact]
    public void ShearYX()
    {
        var trans = Transforms.Shear(0, 0, 1, 0, 0, 0);
        var p     = MathTuple.GetPoint(2,  3, 4);
        Assert.Equal(MathTuple.GetPoint(2, 5, 4), trans * p);
    }

    [Fact]
    public void ShearYZ()
    {
        var trans = Transforms.Shear(0, 0, 0, 1, 0, 0);
        var p     = MathTuple.GetPoint(2,  3, 4);
        Assert.Equal(MathTuple.GetPoint(2, 7, 4), trans * p);
    }

    [Fact]
    public void ShearZX()
    {
        var trans = Transforms.Shear(0, 0, 0, 0, 1, 0);
        var p     = MathTuple.GetPoint(2,  3, 4);
        Assert.Equal(MathTuple.GetPoint(2, 3, 6), trans * p);
    }

    [Fact]
    public void ShearZY()
    {
        var trans = Transforms.Shear(0, 0, 0, 0, 0, 1);
        var p     = MathTuple.GetPoint(2,  3, 4);
        Assert.Equal(MathTuple.GetPoint(2, 3, 7), trans * p);
    }

    [Fact]
    public void MultipleIndividualTransformations()
    {
        var rot         = Transforms.RotateX(Math.PI / 2);
        var scale       = Transforms.GetScalingMatrix(5, 5, 5);
        var translation = Transforms.GetTranslationMatrix(10, 5, 7);
        var p           = MathTuple.GetPoint(1, 0, 1);
        var rotated     = rot * p;
        Assert.Equal(MathTuple.GetPoint(1, -1, 0), rotated);
        var scaled = scale * rotated;
        Assert.Equal(MathTuple.GetPoint(5, -5, 0), scaled);
        var translated = translation * scaled;
        Assert.Equal(MathTuple.GetPoint(15, 0, 7), translated);
    }

    [Fact]
    public void MultipleChainedTransformations()
    {
        var rot         = Transforms.RotateX(Math.PI / 2);
        var scale       = Transforms.GetScalingMatrix(5, 5, 5);
        var translation = Transforms.GetTranslationMatrix(10, 5, 7);
        var p           = MathTuple.GetPoint(1, 0, 1);
        var transformed = (translation * scale * rot) * p;
        Assert.Equal(MathTuple.GetPoint(15, 0, 7), transformed);
    }

    [Fact]
    public void ViewTransformPositiveZ()
    {
        var    from = MathTuple.GetPoint(0, 0, 0);
        var    to   = MathTuple.GetPoint(0, 0, 1);
        var    up   = MathTuple.GetVector(0, 1, 0);
        Matrix t    = Transforms.ViewTransform(from, to, up);
        Assert.Equal(Transforms.GetScalingMatrix(-1, 1, -1), t);
    }

    [Fact]
    public void ViewTransformMovesWorld()
    {
        var    from = MathTuple.GetPoint(0, 0, 8);
        var    to   = MathTuple.GetPoint(0, 0, 1);
        var    up   = MathTuple.GetVector(0, 1, 0);
        Matrix t    = Transforms.ViewTransform(from, to, up);
        Assert.Equal(Transforms.GetTranslationMatrix(0, 0, -8), t);
    }

    [Fact]
    public void ViewTransformArbitrary()
    {
        var    from = MathTuple.GetPoint(1, 3,  2);
        var    to   = MathTuple.GetPoint(4, -2, 8);
        var    up   = MathTuple.GetVector(1, 1, 0);
        Matrix t    = Transforms.ViewTransform(from, to, up);
        Assert.Equal(
            new Matrix(new[,]
            {
                { -0.50709, 0.50709, 0.67612, -2.36643 }, { 0.76772, 0.60609, 0.12122, -2.82843 },
                { -0.35857, 0.59761, -0.71714, 0.00000 }, { 0.0000, 0.0, 0.0, 1 }
            }), t);
    }
}
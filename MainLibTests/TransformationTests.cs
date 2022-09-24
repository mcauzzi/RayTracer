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
        Assert.Equal(trans * new Point(-3, 4, 5), new Point(2, 1, 7));
    }

    [Fact]
    public void InvertedTranslation()
    {
        var trans = Transforms.GetTranslationMatrix(5, -3, 2).GetInverse();
        Assert.Equal(trans * new Point(-3, 4, 5), new Point(-8, 7, 3));
    }

    [Fact]
    public void Scaling()
    {
        Point tempQualifier = new Point(2, 3, 4);
        var trans = Transforms.GetScalingMatrix(2, 3, 4);
        Assert.Equal(trans * new Point(-4, 6, 8), new Point(-8, 18, 32));
    }

    [Fact]
    public void ScalingVector()
    {
        var trans = Transforms.GetScalingMatrix(2, 3, 4);
        Assert.Equal(trans * new Vector(-4, 6, 8), new Vector(-8, 18, 32));
    }

    [Fact]
    public void InvertedScaling()
    {
        var trans = Transforms.GetScalingMatrix(2, 3, 4).GetInverse();
        Assert.Equal(trans * new Point(-4, 6, 8), new Point(-2, 2, 2));
    }

    [Fact]
    public void Reflection()
    {
        Point tempQualifier = new Point(-1, 1, 1);
        var trans = Transforms.GetScalingMatrix(-1, 1, 1).GetInverse();
        Assert.Equal(trans * new Point(2, 3, 4), new Point(-2, 3, 4));
    }

    [Fact]
    public void RotateX()
    {
        Point p = new Point(0, 1, 0);
        var halfQuarter = Transforms.RotateX(Math.PI / 4);
        var fullQuarter = Transforms.RotateX(Math.PI / 2);
        Assert.Equal(new Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2), halfQuarter * p);
        Assert.Equal(new Point(0, 0, 1), fullQuarter * p);
    }

    [Fact]
    public void RotateXInverse()
    {
        Point p = new Point(0, 1, 0);
        var halfQuarter = Transforms.RotateX(Math.PI / 4).GetInverse();
        Assert.Equal(new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2), halfQuarter * p);
    }

    [Fact]
    public void RotateY()
    {
        Point p = new Point(0, 0, 1);
        var halfQuarter = Transforms.RotateY(Math.PI / 4);
        var fullQuarter = Transforms.RotateY(Math.PI / 2);
        Assert.Equal(new Point(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2), halfQuarter * p);
        Assert.Equal(new Point(1, 0, 0), fullQuarter * p);
    }

    [Fact]
    public void RotateZ()
    {
        Point p = new Point(0, 1, 0);
        var halfQuarter = Transforms.RotateZ(Math.PI / 4);
        var fullQuarter = Transforms.RotateZ(Math.PI / 2);
        Assert.Equal(new Point(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0), halfQuarter * p);
        Assert.Equal(new Point(-1, 0, 0), fullQuarter * p);
    }

    [Fact]
    public void ShearXY()
    {
        var trans = Transforms.Shear(1, 0, 0, 0, 0, 0);
        var p = new Point(2, 3, 4);
        Assert.Equal(new Point(5, 3, 4), trans * p);
    }

    [Fact]
    public void ShearXZ()
    {
        var trans = Transforms.Shear(0, 1, 0, 0, 0, 0);
        var p = new Point(2, 3, 4);
        Assert.Equal(new Point(6, 3, 4), trans * p);
    }

    [Fact]
    public void ShearYX()
    {
        var trans = Transforms.Shear(0, 0, 1, 0, 0, 0);
        var p = new Point(2, 3, 4);
        Assert.Equal(new Point(2, 5, 4), trans * p);
    }

    [Fact]
    public void ShearYZ()
    {
        var trans = Transforms.Shear(0, 0, 0, 1, 0, 0);
        var p = new Point(2, 3, 4);
        Assert.Equal(new Point(2, 7, 4), trans * p);
    }

    [Fact]
    public void ShearZX()
    {
        var trans = Transforms.Shear(0, 0, 0, 0, 1, 0);
        var p = new Point(2, 3, 4);
        Assert.Equal(new Point(2, 3, 6), trans * p);
    }

    [Fact]
    public void ShearZY()
    {
        var trans = Transforms.Shear(0, 0, 0, 0, 0, 1);
        var p = new Point(2, 3, 4);
        Assert.Equal(new Point(2, 3, 7), trans * p);
    }

    [Fact]
    public void MultipleIndividualTransformations()
    {
        var rot = Transforms.RotateX(Math.PI / 2);
        var scale = Transforms.GetScalingMatrix(5, 5, 5);
        var translation = Transforms.GetTranslationMatrix(10, 5, 7);
        var p = new Point(1, 0, 1);
        var rotated = rot * p;
        Assert.Equal(new Point(1, -1, 0), rotated);
        var scaled = scale * rotated;
        Assert.Equal(new Point(5, -5, 0), scaled);
        var translated = translation * scaled;
        Assert.Equal(new Point(15, 0, 7), translated);
    }

    [Fact]
    public void MultipleChainedTransformations()
    {
        var rot = Transforms.RotateX(Math.PI / 2);
        var scale = Transforms.GetScalingMatrix(5, 5, 5);
        var translation = Transforms.GetTranslationMatrix(10, 5, 7);
        var p = new Point(1, 0, 1);
        var transformed = (translation * scale * rot) * p;
        Assert.Equal(new Point(15, 0, 7), transformed);
    }
}
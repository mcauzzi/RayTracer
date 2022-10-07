using Drawing;
using MainLib;

namespace DrawingTests;

public class RayTests
{
    [Fact]
    public void RayCreation()
    {
        var r = new Ray(MathTuple.GetPoint(1, 2, 3), MathTuple.GetVector(4, 5, 6));
        Assert.Equal(MathTuple.GetPoint(1, 2, 3),  r.Origin);
        Assert.Equal(MathTuple.GetVector(4, 5, 6), r.Direction);
    }

    [Fact]
    public void Position()
    {
        var r = new Ray(MathTuple.GetPoint(2, 3, 4), MathTuple.GetVector(1, 0, 0));
        Assert.Equal(r.Position(0),   MathTuple.GetPoint(2,   3, 4));
        Assert.Equal(r.Position(1),   MathTuple.GetPoint(3,   3, 4));
        Assert.Equal(r.Position(-1),  MathTuple.GetPoint(1,   3, 4));
        Assert.Equal(r.Position(2.5), MathTuple.GetPoint(4.5, 3, 4));
    }

    [Fact]
    public void TwoPointIntersects()
    {
        var r  = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var s  = new Sphere();
        var xs = r.Intersects(s);
        Assert.Equal(2,   xs.Length);
        Assert.Equal(4.0, xs[0].Distance);
        Assert.Equal(6.0, xs[1].Distance);
    }

    [Fact]
    public void TangentIntersects()
    {
        var r  = new Ray(MathTuple.GetPoint(0, 1, -5), MathTuple.GetVector(0, 0, 1));
        var s  = new Sphere();
        var xs = r.Intersects(s);
        Assert.Equal(2,   xs.Length);
        Assert.Equal(5.0, xs[0].Distance);
        Assert.Equal(5.0, xs[1].Distance);
    }

    [Fact]
    public void RayInsideIntersects()
    {
        var r  = new Ray(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0, 0, 1));
        var s  = new Sphere();
        var xs = r.Intersects(s);
        Assert.Equal(2,    xs.Length);
        Assert.Equal(-1.0, xs[0].Distance);
        Assert.Equal(1.0,  xs[1].Distance);
    }

    [Fact]
    public void RayBehindIntersects()
    {
        var r  = new Ray(MathTuple.GetPoint(0, 0, 5), MathTuple.GetVector(0, 0, 1));
        var s  = new Sphere();
        var xs = r.Intersects(s);
        Assert.Equal(2,    xs.Length);
        Assert.Equal(-6.0, xs[0].Distance);
        Assert.Equal(-4.0, xs[1].Distance);
    }

    [Fact]
    public void NoIntersects()
    {
        var r  = new Ray(MathTuple.GetPoint(0, 2, -5), MathTuple.GetVector(0, 0, 1));
        var s  = new Sphere();
        var xs = r.Intersects(s);
        Assert.Empty(xs);
    }

    [Fact]
    public void RayTranslation()
    {
        var r  = new Ray(MathTuple.GetPoint(1, 2, 3), MathTuple.GetVector(0, 1, 0));
        var m  = Transforms.GetTranslationMatrix(3, 4, 5);
        var r2 = r.Transform(m);
        Assert.Equal(MathTuple.GetPoint(4, 6, 8),  r2.Origin);
        Assert.Equal(MathTuple.GetVector(0, 1, 0), r2.Direction);
    }

    [Fact]
    public void RayScaling()
    {
        var r  = new Ray(MathTuple.GetPoint(1, 2, 3), MathTuple.GetVector(0, 1, 0));
        var m  = Transforms.GetScalingMatrix(2, 3, 4);
        var r2 = r.Transform(m);
        Assert.Equal(MathTuple.GetPoint(2, 6, 12), r2.Origin);
        Assert.Equal(MathTuple.GetVector(0, 3, 0), r2.Direction);
    }
}
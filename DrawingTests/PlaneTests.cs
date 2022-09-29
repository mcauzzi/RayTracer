using Drawing;
using MainLib;

namespace DrawingTests;

public class PlaneTests
{
    [Fact]
    public void NormalIsConstant()
    {
        var p = new Plane();
        var n1 = p.LocalNormal(new Point(0, 0, 0));
        var n2 = p.LocalNormal(new Point(10, 0, -10));
        var n3 = p.LocalNormal(new Point(-5, 0, 150));
        Assert.Equal(new Vector(0, 1, 0), n1);
        Assert.Equal(new Vector(0, 1, 0), n2);
        Assert.Equal(new Vector(0, 1, 0), n3);
    }

    [Fact]
    public void IntersectParallelRay()
    {
        var p = new Plane();
        var r = new Ray(new Point(0, 10, 0), new Vector(0, 0, 1));
        var xs = p.LocalIntersect(r);
        Assert.Empty(xs);
    }

    [Fact]
    public void IntersectCoplanarRay()
    {
        var p = new Plane();
        var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
        var xs = p.LocalIntersect(r);
        Assert.Empty(xs);
    }

    [Fact]
    public void IntersectAboveRay()
    {
        var p = new Plane();
        var r = new Ray(new Point(0, 1, 0), new Vector(0, -1, 0));
        var xs = p.LocalIntersect(r);
        Assert.Single(xs);
        Assert.Equal(1, xs[0].Distance);
        Assert.Equal(p, xs[0].Obj);
    }

    [Fact]
    public void IntersectBelowRay()
    {
        var p = new Plane();
        var r = new Ray(new Point(0, -1, 0), new Vector(0, 1, 0));
        var xs = p.LocalIntersect(r);
        Assert.Single(xs);
        Assert.Equal(1, xs[0].Distance);
        Assert.Equal(p, xs[0].Obj);
    }
}
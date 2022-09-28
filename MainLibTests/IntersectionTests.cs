using System.Collections.Generic;
using Drawing;
using GlobalConstants;
using MainLib;
using Xunit;

namespace MainLibTests;

public class IntersectionTests
{
    [Fact]
    public void CreationTest()
    {
        var s = new Sphere();
        var p = new Intersection(s, 3.5);
        Assert.Equal(3.5, p.Distance);
        Assert.Equal(s, p.Obj);
    }

    [Fact]
    public void ObjectSet()
    {
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        var s = new Sphere();
        var xs = r.Intersects(s);
        Assert.Equal(2, xs.Count);
        Assert.Equal(s, xs[0].Obj);
        Assert.Equal(s, xs[1].Obj);
    }

    [Fact]
    public void HitAllPositive()
    {
        var s = new Sphere();
        var i1 = new Intersection(s, 1);
        var i2 = new Intersection(s, 2);
        var h = Intersection.Hit(new List<Intersection>() { i1, i2 });
        Assert.Equal(i1, h);
    }

    [Fact]
    public void HitSomePositive()
    {
        var s = new Sphere();
        var i1 = new Intersection(s, -1);
        var i2 = new Intersection(s, 1);
        var h = Intersection.Hit(new List<Intersection>() { i1, i2 });
        Assert.Equal(i2, h);
    }

    [Fact]
    public void HitNonePositive()
    {
        var s = new Sphere();
        var i1 = new Intersection(s, -2);
        var i2 = new Intersection(s, -1);
        var h = Intersection.Hit(new List<Intersection>() { i1, i2 });
        Assert.Null(h);
    }

    [Fact]
    public void HitAlwaysLowest()
    {
        var s = new Sphere();
        var i1 = new Intersection(s, 5);
        var i2 = new Intersection(s, 7);
        var i3 = new Intersection(s, -3);
        var i4 = new Intersection(s, 2);
        var h = Intersection.Hit(new List<Intersection>() { i1, i2, i3, i4 });
        Assert.Equal(i4, h);
    }

    [Fact]
    public void PreComputation()
    {
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        var s = new Sphere();
        var i = new Intersection(s, 4);
        var comps = i.PrepareComputation(r);

        Assert.Equal(i.Distance, comps.Distance);
        Assert.Equal(i.Obj, comps.Obj);
        Assert.Equal(new Point(0, 0, -1), comps.Point);
        Assert.Equal(new Vector(0, 0, -1), comps.EyeV);
        Assert.Equal(new Vector(0, 0, -1), comps.NormalV);
    }

    [Fact]
    public void PreComputationOutside()
    {
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        var s = new Sphere();
        var i = new Intersection(s, 4);
        var comps = i.PrepareComputation(r);

        Assert.False(comps.IsInside);
    }

    [Fact]
    public void PreComputationInside()
    {
        var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
        var s = new Sphere();
        var i = new Intersection(s, 1);
        var comps = i.PrepareComputation(r);

        Assert.True(comps.IsInside);
        Assert.Equal(new Point(0, 0, 1), comps.Point);
        Assert.Equal(new Vector(0, 0, -1), comps.EyeV);
        Assert.Equal(new Vector(0, 0, -1), comps.NormalV);
    }

    [Fact]
    public void HitShouldOffsetPoint()
    {
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        var s = new Sphere();
        s.Transformation *= Transforms.GetTranslationMatrix(0, 0, 1);
        var i = new Intersection(s, 5);
        var comps = i.PrepareComputation(r);
        Assert.True(comps.OverPoint.Z < -(Constants.Epsilon / 2), $"{comps.OverPoint.Z}| {-(Constants.Epsilon / 2)}");
        Assert.True(comps.Point.Z > comps.OverPoint.Z, $"{comps.Point.Z} | {comps.OverPoint.Z}");
    }
}
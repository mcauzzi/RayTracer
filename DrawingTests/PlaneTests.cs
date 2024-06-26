﻿using Drawing;
using Drawing.Shapes;
using MainLib;

namespace DrawingTests;

public class PlaneTests
{
    [Fact]
    public void NormalIsConstant()
    {
        var p  = new Plane();
        var n1 = p.LocalNormal(MathTuple.GetPoint(0,  0, 0));
        var n2 = p.LocalNormal(MathTuple.GetPoint(10, 0, -10));
        var n3 = p.LocalNormal(MathTuple.GetPoint(-5, 0, 150));
        Assert.Equal(MathTuple.GetVector(0, 1, 0), n1);
        Assert.Equal(MathTuple.GetVector(0, 1, 0), n2);
        Assert.Equal(MathTuple.GetVector(0, 1, 0), n3);
    }

    [Fact]
    public void IntersectParallelRay()
    {
        var p  = new Plane();
        var r  = new Ray(MathTuple.GetPoint(0, 10, 0), MathTuple.GetVector(0, 0, 1));
        var xs = p.LocalIntersect(r);
        Assert.Empty(xs);
    }

    [Fact]
    public void IntersectCoplanarRay()
    {
        var p  = new Plane();
        var r  = new Ray(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0, 0, 1));
        var xs = p.LocalIntersect(r);
        Assert.Empty(xs);
    }

    [Fact]
    public void IntersectAboveRay()
    {
        var p  = new Plane();
        var r  = new Ray(MathTuple.GetPoint(0, 1, 0), MathTuple.GetVector(0, -1, 0));
        var xs = p.LocalIntersect(r);
        Assert.Single(xs);
        Assert.Equal(1, xs[0].Distance);
        Assert.Equal(p, xs[0].Obj);
    }

    [Fact]
    public void IntersectBelowRay()
    {
        var p  = new Plane();
        var r  = new Ray(MathTuple.GetPoint(0, -1, 0), MathTuple.GetVector(0, 1, 0));
        var xs = p.LocalIntersect(r);
        Assert.Single(xs);
        Assert.Equal(1, xs[0].Distance);
        Assert.Equal(p, xs[0].Obj);
    }
}
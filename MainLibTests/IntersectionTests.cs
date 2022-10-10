using System;
using System.Collections.Generic;
using Drawing;
using Drawing.Shapes;
using Globals;
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
        Assert.Equal(s,   p.Obj);
    }

    [Fact]
    public void ObjectSet()
    {
        var r  = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var s  = new Sphere();
        var xs = r.Intersects(s);
        Assert.Equal(2, xs.Length);
        Assert.Equal(s, xs[0]
            .Obj);
        Assert.Equal(s, xs[1]
            .Obj);
    }

    [Fact]
    public void HitAllPositive()
    {
        var s  = new Sphere();
        var i1 = new Intersection(s, 1);
        var i2 = new Intersection(s, 2);
        var h  = Intersection.Hit(new Intersection[] { i1, i2 });
        Assert.Equal(i1, h);
    }

    [Fact]
    public void HitSomePositive()
    {
        var s  = new Sphere();
        var i1 = new Intersection(s, -1);
        var i2 = new Intersection(s, 1);
        var h  = Intersection.Hit(new Intersection[] { i1, i2 });
        Assert.Equal(i2, h);
    }

    [Fact]
    public void HitNonePositive()
    {
        var s  = new Sphere();
        var i1 = new Intersection(s, -2);
        var i2 = new Intersection(s, -1);
        var h  = Intersection.Hit(new Intersection[] { i1, i2 });
        Assert.Null(h);
    }

    [Fact]
    public void HitAlwaysLowest()
    {
        var s  = new Sphere();
        var i1 = new Intersection(s, 5);
        var i2 = new Intersection(s, 7);
        var i3 = new Intersection(s, -3);
        var i4 = new Intersection(s, 2);
        var h  = Intersection.Hit(new[] { i1, i2, i3, i4 });
        Assert.Equal(i4, h);
    }

    [Fact]
    public void PreComputation()
    {
        var r     = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var s     = new Sphere();
        var i     = new Intersection(s, 4);
        var comps = i.PrepareComputation(r);

        Assert.Equal(i.Distance,                    comps.Distance);
        Assert.Equal(i.Obj,                         comps.Obj);
        Assert.Equal(MathTuple.GetPoint(0, 0, -1),  comps.Point);
        Assert.Equal(MathTuple.GetVector(0, 0, -1), comps.EyeV);
        Assert.Equal(MathTuple.GetVector(0, 0, -1), comps.NormalV);
    }

    [Fact]
    public void PreComputationOutside()
    {
        var r     = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var s     = new Sphere();
        var i     = new Intersection(s, 4);
        var comps = i.PrepareComputation(r);

        Assert.False(comps.IsInside);
    }

    [Fact]
    public void PreComputationInside()
    {
        var r     = new Ray(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0, 0, 1));
        var s     = new Sphere();
        var i     = new Intersection(s, 1);
        var comps = i.PrepareComputation(r);

        Assert.True(comps.IsInside);
        Assert.Equal(MathTuple.GetPoint(0, 0, 1),   comps.Point);
        Assert.Equal(MathTuple.GetVector(0, 0, -1), comps.EyeV);
        Assert.Equal(MathTuple.GetVector(0, 0, -1), comps.NormalV);
    }

    [Fact]
    public void HitShouldOffsetPoint()
    {
        var r = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var s = new Sphere();
        s.Transformation *= Transforms.GetTranslationMatrix(0, 0, 1);
        var i     = new Intersection(s, 5);
        var comps = i.PrepareComputation(r);
        Assert.True(comps.OverPoint.Z < -(Constants.Epsilon / 2), $"{comps.OverPoint.Z}| {-(Constants.Epsilon / 2)}");
        Assert.True(comps.Point.Z > comps.OverPoint.Z,            $"{comps.Point.Z} | {comps.OverPoint.Z}");
    }

    [Fact]
    public void ReflectionVector()
    {
        var p     = new Plane();
        var r     = new Ray(MathTuple.GetPoint(0, 1, -1), MathTuple.GetVector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        var i     = new Intersection(p, Math.Sqrt(2));
        var comps = i.PrepareComputation(r);
        Assert.Equal(MathTuple.GetVector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2), comps.ReflectV);
    }

    [Fact]
    public void N1AndN2AtVariousIntersections()
    {
        var A = Sphere.GlassSphere;
        A.Transformation           = Transforms.GetScalingMatrix(2, 2, 2);
        A.Material.RefractiveIndex = 1.5;
        var B = Sphere.GlassSphere;
        B.Transformation           = Transforms.GetTranslationMatrix(0, 0, -0.25);
        B.Material.RefractiveIndex = 2;
        var C = Sphere.GlassSphere;
        C.Transformation           = Transforms.GetTranslationMatrix(0, 0, 0.25);
        C.Material.RefractiveIndex = 2.5;
        var r = new Ray(MathTuple.GetPoint(0, 0, -4), MathTuple.GetVector(0, 0, 1));
        var xs = new Intersection[]
            { new(A, 2), new(B, 2.75), new(C, 3.25), new(B, 4.75), new(C, 5.25), new(A, 6) };
        var comps = new List<Computation>();
        for (int i = 0; i < xs.Length; i++)
        {
            comps.Add(xs[i]
                .PrepareComputation(r, xs));
        }

        Assert.Equal(1, comps[0]
            .N1);
        Assert.Equal(1.5, comps[0]
            .N2);
        Assert.Equal(1.5, comps[1]
            .N1);
        Assert.Equal(2, comps[1]
            .N2);
        Assert.Equal(2, comps[2]
            .N1);
        Assert.Equal(2.5, comps[2]
            .N2);
        Assert.Equal(2.5, comps[3]
            .N1);
        Assert.Equal(2.5, comps[3]
            .N2);
        Assert.Equal(2.5, comps[4]
            .N1);
        Assert.Equal(1.5, comps[4]
            .N2);
        Assert.Equal(1.5, comps[5]
            .N1);
        Assert.Equal(1, comps[5]
            .N2);
    }

    [Fact]
    public void UnderPointBelowTheSurface()
    {
        var r = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var s = Sphere.GlassSphere;
        s.Transformation =  Transforms.GetTranslationMatrix(0, 0, 1);
        s.Transformation *= Transforms.GetTranslationMatrix(0, 0, 1);
        var i     = new Intersection(s, 5);
        var xs    = new Intersection[] { i };
        var comps = i.PrepareComputation(r, xs);
        Assert.True(comps.UnderPoint.Z > (Constants.Epsilon / 2), $"{comps.UnderPoint.Z}| {-(Constants.Epsilon / 2)}");
        Assert.True(comps.Point.Z < comps.UnderPoint.Z,           $"{comps.Point.Z} | {comps.UnderPoint.Z}");
    }

    [Fact]
    public void SchlickWithTotalInternalRefraction()
    {
        var shape       = Sphere.GlassSphere;
        var ray         = new Ray(MathTuple.GetPoint(0, 0, Math.Sqrt(2) / 2), MathTuple.GetVector(0, 1, 0));
        var xs          = new Intersection[] { new(shape, -Math.Sqrt(2) / 2), new(shape, Math.Sqrt(2) / 2) };
        var comps       = xs[1].PrepareComputation(ray, xs);
        var reflectance = comps.Schlick();
        Assert.Equal(1, reflectance);
    }

    [Fact]
    public void SchlickWithPerpendicularAngle()
    {
        var shape       = Sphere.GlassSphere;
        var ray         = new Ray(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0, 1, 0));
        var xs          = new Intersection[] { new(shape, -1), new(shape, 1) };
        var comps       = xs[1].PrepareComputation(ray, xs);
        var reflectance = comps.Schlick();
        Assert.Equal(0.04, reflectance, 4);
    }

    [Fact]
    public void SchlickWithSmallAngle()
    {
        var shape       = Sphere.GlassSphere;
        var ray         = new Ray(MathTuple.GetPoint(0, 0.99, -2), MathTuple.GetVector(0, 0, 1));
        var xs          = new Intersection[] { new(shape, 1.8589) };
        var comps       = xs[0].PrepareComputation(ray, xs);
        var reflectance = comps.Schlick();
        Assert.Equal(0.48873, reflectance, 5);
    }
}
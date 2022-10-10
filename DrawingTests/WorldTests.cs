using Drawing;
using Drawing.Patterns;
using Drawing.Shapes;
using MainLib;

namespace DrawingTests;

public class WorldTests
{
    [Fact]
    public void DefaultWorld()
    {
        var w = World.GetDefaultWorld();
        var s1 = new Sphere
        {
            Material = new Material() { Color = new Color(0.8, 1.0, 0.6), Diffuse = 0.7, Specular = 0.2 }
        };
        var s2 = new Sphere
        {
            Transformation = Transforms.GetScalingMatrix(0.5, 0.5, 0.5)
        };
        var l = new PointLight(Color.White, MathTuple.GetPoint(-10, 10, -10));
        Assert.Equal(s1, w.Shapes.First());
        Assert.Equal(s2, w.Shapes[1]);
        Assert.Equal(l,  w.Lights.First());
    }

    [Fact]
    public void IntesectWorld()
    {
        var w  = World.GetDefaultWorld();
        var r  = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var wr = w.Intersect(r);
        Assert.Equal(4, wr.Length);
        Assert.Equal(4, wr[0]
            .Distance);
        Assert.Equal(4.5, wr[1]
            .Distance);
        Assert.Equal(5.5, wr[2]
            .Distance);
        Assert.Equal(6, wr[3]
            .Distance);
    }

    [Fact]
    public void IntersectionShade()
    {
        var   w     = World.GetDefaultWorld();
        var   r     = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var   s     = w.Shapes.First();
        var   i     = new Intersection(s, 4);
        var   comps = i.PrepareComputation(r);
        Color c     = w.ShadeHit(comps);
        Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c);
    }

    [Fact]
    public void IntersectionShadeInside()
    {
        var w = World.GetDefaultWorld();
        w.Lights[0] = new PointLight(Color.White, MathTuple.GetPoint(0, 0.25, 0));
        var   r     = new Ray(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0, 0, 1));
        var   s     = w.Shapes[1];
        var   i     = new Intersection(s, 0.5);
        var   comps = i.PrepareComputation(r);
        Color c     = w.ShadeHit(comps);
        Assert.Equal(new Color(0.90498, 0.90498, 0.90498), c);
    }

    [Fact]
    public void RayMiss()
    {
        var w = World.GetDefaultWorld();
        var r = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 1, 0));
        var c = w.ColorAt(r);

        Assert.Equal(Color.Black, c);
    }

    [Fact]
    public void RayHit()
    {
        var w = World.GetDefaultWorld();
        var r = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var c = w.ColorAt(r);

        Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c);
    }

    [Fact]
    public void RayIntersectionBehindRay()
    {
        var w = World.GetDefaultWorld();
        w.Shapes.First()
            .Material.Ambient = 1;
        var innerMat = w.Shapes[1]
            .Material;
        innerMat.Ambient = 1;
        var r = new Ray(MathTuple.GetPoint(0, 0, 0.75), MathTuple.GetVector(0, 0, -1));
        var c = w.ColorAt(r);

        Assert.Equal(innerMat.Color, c);
    }

    [Fact]
    public void NoShadownNothingCollinearPointAndLight()
    {
        var w = World.GetDefaultWorld();
        var p = MathTuple.GetPoint(0, 10, 0);
        Assert.False(w.IsShadowed(p));
    }

    [Fact]
    public void NoShadownObjectBetweenPointAndLight()
    {
        var w = World.GetDefaultWorld();
        var p = MathTuple.GetPoint(10, -10, 10);
        Assert.True(w.IsShadowed(p));
    }

    [Fact]
    public void NoShadownObjectBehindLight()
    {
        var w = World.GetDefaultWorld();
        var p = MathTuple.GetPoint(-20, 20, -20);
        Assert.False(w.IsShadowed(p));
    }

    [Fact]
    public void NoShadownObjectBehindPoint()
    {
        var w = World.GetDefaultWorld();
        var p = MathTuple.GetPoint(-2, 2, -2);
        Assert.False(w.IsShadowed(p));
    }

    [Fact]
    public void ShadeHitWithShadow()
    {
        var l  = new PointLight(Color.White, MathTuple.GetPoint(0, 0, -10));
        var s  = new Sphere();
        var s1 = new Sphere();
        s1.Transformation *= Transforms.GetTranslationMatrix(0, 0, 10);
        var w     = new World(new List<PointLight>() { l }, new List<Shape> { s1, s });
        var r     = new Ray(MathTuple.GetPoint(0, 0, 5), MathTuple.GetVector(0, 0, 1));
        var i     = new Intersection(s1, 4);
        var comps = i.PrepareComputation(r);
        var c     = w.ShadeHit(comps);
        Assert.Equal(new Color(0.1, 0.1, 0.1), c);
    }

    [Fact]
    public void ReflectiveMaterial()
    {
        var w = World.GetDefaultWorld();
        var p = new Plane()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -1, 0), Material = new Material() { Reflective = 0.5 }
        };
        w.Shapes.Add(p);
        var r     = new Ray(MathTuple.GetPoint(0, 0, -3), MathTuple.GetVector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        var i     = new Intersection(p, Math.Sqrt(2));
        var comps = i.PrepareComputation(r);
        var color = w.ReflectedColor(comps);
        Assert.Equal(new Color(0.19033, 0.23791, 0.14274), color);
    }

    [Fact]
    public void NonReflectiveMaterial()
    {
        var w = World.GetDefaultWorld();
        var p = w.Shapes[1];
        var r = new Ray(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0, 0, 1));
        p.Material.Ambient = 1;
        var i     = new Intersection(p, 1);
        var comps = i.PrepareComputation(r);
        var color = w.ReflectedColor(comps);
        Assert.Equal(new Color(0, 0, 0), color);
    }

    [Fact]
    public void ShadeHitReflective()
    {
        var w = World.GetDefaultWorld();
        var p = new Plane()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -1, 0), Material = new Material() { Reflective = 0.5 }
        };
        w.Shapes.Add(p);
        var r     = new Ray(MathTuple.GetPoint(0, 0, -3), MathTuple.GetVector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        var i     = new Intersection(p, Math.Sqrt(2));
        var comps = i.PrepareComputation(r);
        var color = w.ShadeHit(comps);
        Assert.Equal(new Color(0.87675, 0.92434, 0.82917), color);
    }

    [Fact]
    public void ShouldTerminate()
    {
        var light = new PointLight(Color.White, MathTuple.GetPoint(0, 0, 0));
        var p = new Plane()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -1, 0), Material = new Material() { Reflective = 1 }
        };

        var p1 = new Plane()
            { Transformation = Transforms.GetTranslationMatrix(0, 1, 0), Material = new Material() { Reflective = 1 } };

        var r     = new Ray(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0, -1, 0));
        var w     = new World(new List<PointLight>() { light }, new List<Shape>() { p, p1 });
        var color = w.ColorAt(r);
        var res   = Record.Exception(() => w.ColorAt(r));
        Assert.Null(res);
    }

    [Fact]
    public void ReflectedColorMaximumRecursionDepth()
    {
        var w = World.GetDefaultWorld();
        var p = new Plane()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -1, 0), Material = new Material() { Reflective = 0.5 }
        };
        w.Shapes.Add(p);
        var r     = new Ray(MathTuple.GetPoint(0, 0, -3), MathTuple.GetVector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        var i     = new Intersection(p, Math.Sqrt(2));
        var comps = i.PrepareComputation(r);
        var color = w.ReflectedColor(comps, 0);
        Assert.Equal(Color.Black, color);
    }

    [Fact]
    public void RefractedColorOpaqueSurface()
    {
        var w     = World.GetDefaultWorld();
        var shape = w.Shapes[0];
        var r     = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var xs    = new Intersection[] { new(shape, 4), new(shape, 6) };
        var comps = xs[0]
            .PrepareComputation(r, xs);
        var c = w.RefractedColor(comps, 5);
        Assert.Equal(Color.Black, c);
    }

    [Fact]
    public void RefractedColorMaximumDepth()
    {
        var w     = World.GetDefaultWorld();
        var shape = w.Shapes[0];
        shape.Material.Transparency    = 1;
        shape.Material.RefractiveIndex = 1.5;
        var r  = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        var xs = new Intersection[] { new(shape, 4), new(shape, 6) };
        var comps = xs[0]
            .PrepareComputation(r, xs);
        var c = w.RefractedColor(comps, 0);
        Assert.Equal(Color.Black, c);
    }

    [Fact]
    public void RefractedColorTotalInternalReflection()
    {
        var w     = World.GetDefaultWorld();
        var shape = w.Shapes[0];
        shape.Material.Transparency    = 1;
        shape.Material.RefractiveIndex = 1.5;
        var r     = new Ray(MathTuple.GetPoint(0, 0, Math.Sqrt(2) / 2), MathTuple.GetVector(0, 1, 0));
        var xs    = new Intersection[] { new(shape, -Math.Sqrt(2) / 2), new(shape, Math.Sqrt(2) / 2) };
        var comps = xs[1].PrepareComputation(r, xs);
        var c     = w.RefractedColor(comps, 5);
        Assert.Equal(Color.Black, c);
    }

    [Fact]
    public void RefractedColorWithRefractedRay()
    {
        var w = World.GetDefaultWorld();
        var A = w.Shapes[0];
        A.Material.Ambient = 1;
        A.Material.Pattern = new Pattern();
        var B = w.Shapes[1];
        B.Material.Transparency    = 1.0;
        B.Material.RefractiveIndex = 1.5;
        var r     = new Ray(MathTuple.GetPoint(0, 0, 0.1), MathTuple.GetVector(0, 1, 0));
        var xs    = new Intersection[] { new(A, -0.9899), new(B, -0.4899), new(B, 0.4899), new(A, 0.9899) };
        var comps = xs[2].PrepareComputation(r, xs);
        var c     = w.RefractedColor(comps, 5);
        Assert.Equal(new Color(0, 0.99887, 0.04721), c);
    }

    [Fact]
    public void ShadeHitTransparentMaterial()
    {
        var w = World.GetDefaultWorld();
        var floor = new Plane()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -1, 0),
            Material       = new Material() { Transparency = 0.5, RefractiveIndex = 1.5 }
        };
        var ball = new Sphere()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -3.5, -0.5),
            Material = new Material()
            {
                Color   = new Color(1, 0, 0),
                Ambient = 0.5,
            }
        };
        w.Shapes.Add(floor);
        w.Shapes.Add(ball);
        var r     = new Ray(MathTuple.GetPoint(0, 0, -3), MathTuple.GetVector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        var xs    = new Intersection[] { new(floor, Math.Sqrt(2)) };
        var comps = xs[0].PrepareComputation(r, xs);
        var c     = w.ShadeHit(comps, 5);
        Assert.Equal(new Color(0.93642, 0.68642, 0.68642), c);
    }

    [Fact]
    public void ShadeHitReflectiveTransparentMaterial()
    {
        var w = World.GetDefaultWorld();
        var floor = new Plane()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -1, 0),
            Material       = new Material() { Transparency = 0.5, RefractiveIndex = 1.5, Reflective = 0.5 }
        };
        var ball = new Sphere()
        {
            Transformation = Transforms.GetTranslationMatrix(0, -3.5, -0.5),
            Material = new Material()
            {
                Color   = new Color(1, 0, 0),
                Ambient = 0.5,
            }
        };
        w.Shapes.Add(floor);
        w.Shapes.Add(ball);
        var r     = new Ray(MathTuple.GetPoint(0, 0, -3), MathTuple.GetVector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        var xs    = new Intersection[] { new(floor, Math.Sqrt(2)) };
        var comps = xs[0].PrepareComputation(r, xs);
        var c     = w.ShadeHit(comps, 5);
        Assert.Equal(new Color(0.93391, 0.69643, 0.69243), c);
    }
}
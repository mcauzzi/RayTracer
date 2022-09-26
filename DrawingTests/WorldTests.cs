using Drawing;
using MainLib;

namespace DrawingTests;

public class WorldTests
{
    [Fact]
    public void DefaultWorld()
    {
        var w = new World();
        var s1 = new Sphere
        {
            Material = new Material() { Color = new Color(0.8, 1.0, 0.6), Diffuse = 0.7, Specular = 0.2 }
        };
        var s2 = new Sphere
        {
            Transformation = Transforms.GetScalingMatrix(0.5, 0.5, 0.5)
        };
        var l = new PointLight(new Color(1, 1, 1), new Point(-10, 10, -10));
        Assert.Equal(s1, w.Spheres.First());
        Assert.Equal(s2, w.Spheres[1]);
        Assert.Equal(l, w.Lights.First());
    }

    [Fact]
    public void IntesectWorld()
    {
        var w = new World();
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        List<Intersection> wr = w.Intersect(r);
        Assert.Equal(4, wr.Count);
        Assert.Equal(4, wr[0].Distance);
        Assert.Equal(4.5, wr[1].Distance);
        Assert.Equal(5.5, wr[2].Distance);
        Assert.Equal(6, wr[3].Distance);
    }

    [Fact]
    public void IntersectionShade()
    {
        var w = new World();
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        var s = w.Spheres.First();
        var i = new Intersection(s, 4);
        var comps = i.PrepareComputation(r);
        Color c = w.ShadeHit(comps);
        Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c);
    }

    [Fact]
    public void IntersectionShadeInside()
    {
        var w = new World();
        w.Lights[0] = new PointLight(new Color(1, 1, 1), new Point(0, 0.25, 0));
        var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
        var s = w.Spheres[1];
        var i = new Intersection(s, 0.5);
        var comps = i.PrepareComputation(r);
        Color c = w.ShadeHit(comps);
        Assert.Equal(new Color(0.90498, 0.90498, 0.90498), c);
    }

    [Fact]
    public void RayMiss()
    {
        var w = new World();
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));
        var c = w.ColorAt(r);

        Assert.Equal(new Color(0, 0, 0), c);
    }

    [Fact]
    public void RayHit()
    {
        var w = new World();
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        var c = w.ColorAt(r);

        Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c);
    }

    [Fact]
    public void RayIntersectionBehindRay()
    {
        var w = new World();
        w.Spheres.First().Material.Ambient = 1;
        var innerMat = w.Spheres[1].Material;
        innerMat.Ambient = 1;
        var r = new Ray(new Point(0, 0, 0.75), new Vector(0, 0, -1));
        var c = w.ColorAt(r);

        Assert.Equal(innerMat.Color, c);
    }
}
using Drawing;
using Drawing.Shapes;
using MainLib;

namespace DrawingTests;

public class CylinderTests
{
    [Fact]
    public void RayMissesCylinder()
    {
        var cl = new Cylinder();
        var rays = new List<Ray>()
        {
            new(MathTuple.GetPoint(1, 0, 0), MathTuple.GetVector(0,  1, 0).Normalize()),
            new(MathTuple.GetPoint(0, 0, 0), MathTuple.GetVector(0,  1, 0).Normalize()),
            new(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(1, 1, 1).Normalize())
        };
        foreach (var x in rays.Select(ray => cl.LocalIntersect(ray)))
        {
            Assert.Empty(x);
        }
    }

    [Fact]
    public void RayHitsCylinder()
    {
        var cl = new Cylinder();
        var rays = new List<Ray>()
        {
            new(MathTuple.GetPoint(1,   0, -5), MathTuple.GetVector(0,   0, 1).Normalize()),
            new(MathTuple.GetPoint(0,   0, -5), MathTuple.GetVector(0,   0, 1).Normalize()),
            new(MathTuple.GetPoint(0.5, 0, -5), MathTuple.GetVector(0.1, 1, 1).Normalize())
        };
        var cylinderIntersections = rays.Select(x => cl.LocalIntersect(x)).ToList();
        foreach (var intersection in cylinderIntersections)
        {
            Assert.Equal(2, intersection.Length);
        }

        Assert.Equal(5,       cylinderIntersections[0][0].Distance);
        Assert.Equal(5,       cylinderIntersections[0][1].Distance);
        Assert.Equal(4,       cylinderIntersections[1][0].Distance);
        Assert.Equal(6,       cylinderIntersections[1][1].Distance);
        Assert.Equal(6.80798, cylinderIntersections[2][0].Distance, 5);
        Assert.Equal(7.08872, cylinderIntersections[2][1].Distance, 5);
    }

    [Fact]
    public void Normals()
    {
        var cyl = new Cylinder();
        var points = new List<MathTuple>()
        {
            MathTuple.GetPoint(1,  0, 0), MathTuple.GetPoint(0, 5, -1), MathTuple.GetPoint(0, -2, 1),
            MathTuple.GetPoint(-1, 1, 0)
        };
        var normals = points.Select(x => cyl.LocalNormal(x)).ToList();
        Assert.Equal(MathTuple.GetVector(1,  0, 0),  normals[0]);
        Assert.Equal(MathTuple.GetVector(0,  0, -1), normals[1]);
        Assert.Equal(MathTuple.GetVector(0,  0, 1),  normals[2]);
        Assert.Equal(MathTuple.GetVector(-1, 0, 0),  normals[3]);
    }

    [Fact]
    public void DefaultMinimumAndMaximum()
    {
        var cyl = new Cylinder();
        Assert.Equal(double.NegativeInfinity, cyl.Minimum);
        Assert.Equal(double.PositiveInfinity, cyl.Maximum);
    }

    [Fact]
    public void IntersectConstrainedCylinder()
    {
        var cl = new Cylinder() { Minimum = 1, Maximum = 2 };
        var rays = new List<Ray>()
        {
            new(MathTuple.GetPoint(0, 1.5, 0), MathTuple.GetVector(0.1, 1, 0).Normalize()),
            new(MathTuple.GetPoint(0, 3,   -5), MathTuple.GetVector(0,  0, 1).Normalize()),
            new(MathTuple.GetPoint(0, 0,   -5), MathTuple.GetVector(0,  0, 1).Normalize()),
            new(MathTuple.GetPoint(0, 2,   -5), MathTuple.GetVector(0,  0, 1).Normalize()),
            new(MathTuple.GetPoint(0, 1,   -5), MathTuple.GetVector(0,  0, 1).Normalize()),
            new(MathTuple.GetPoint(0, 1.5, -2), MathTuple.GetVector(0,  0, 1).Normalize())
        };
        var cylinderIntersections = rays.Select(x => cl.LocalIntersect(x)).ToList();

        Assert.Empty(cylinderIntersections[0]);
        Assert.Empty(cylinderIntersections[1]);
        Assert.Empty(cylinderIntersections[2]);
        Assert.Empty(cylinderIntersections[3]);
        Assert.Empty(cylinderIntersections[4]);
        Assert.Equal(2, cylinderIntersections[5].Length);
    }

    [Fact]
    public void IntersectClosedCylinder()
    {
        var cl = new Cylinder() { Minimum = 1, Maximum = 2, ClosedBottom = true, ClosedTop = true };
        var rays = new List<Ray>()
        {
            new(MathTuple.GetPoint(0, 3,  0), MathTuple.GetVector(0,  -1, 0).Normalize()),
            new(MathTuple.GetPoint(0, 3,  -2), MathTuple.GetVector(0, -1, 2).Normalize()),
            new(MathTuple.GetPoint(0, 4,  -2), MathTuple.GetVector(0, -1, 1).Normalize()),
            new(MathTuple.GetPoint(0, 0,  -2), MathTuple.GetVector(0, 1,  2).Normalize()),
            new(MathTuple.GetPoint(0, -1, -2), MathTuple.GetVector(0, 1,  1).Normalize()),
        };
        var cylinderIntersections = rays.Select(x => cl.LocalIntersect(x)).ToList();

        Assert.Equal(2, cylinderIntersections[0].Length);
        Assert.Equal(2, cylinderIntersections[1].Length);
        Assert.Equal(2, cylinderIntersections[2].Length);
        Assert.Equal(2, cylinderIntersections[3].Length);
        Assert.Equal(2, cylinderIntersections[4].Length);
    }

    [Fact]
    public void NormalsCaps()
    {
        var cyl = new Cylinder() { Maximum = 2, Minimum = 1, ClosedBottom = true, ClosedTop = true };
        var points = new List<MathTuple>()
        {
            MathTuple.GetPoint(0, 1, 0), MathTuple.GetPoint(0.5, 1, 0), MathTuple.GetPoint(0, 1, 0.5),
            MathTuple.GetPoint(0, 2, 0), MathTuple.GetPoint(0.5, 2, 0), MathTuple.GetPoint(0, 2, 0.5)
        };
        var normals = points.Select(x => cyl.LocalNormal(x)).ToList();
        Assert.Equal(MathTuple.GetVector(0, -1, 0), normals[0]);
        Assert.Equal(MathTuple.GetVector(0, -1, 0), normals[1]);
        Assert.Equal(MathTuple.GetVector(0, -1, 0), normals[2]);
        Assert.Equal(MathTuple.GetVector(0, 1,  0), normals[3]);
        Assert.Equal(MathTuple.GetVector(0, 1,  0), normals[4]);
        Assert.Equal(MathTuple.GetVector(0, 1,  0), normals[5]);
    }
}
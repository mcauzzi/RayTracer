using Drawing;
using Drawing.Shapes;
using MainLib;

namespace DrawingTests;

public class CubeTests
{
    [Fact]
    public void RayIntersections()
    {
        var c = new Cube();
        var r = new List<Ray>()
        {
            new(MathTuple.GetPoint(5,   0.5, 0), MathTuple.GetVector(-1, 0,  0)),
            new(MathTuple.GetPoint(-5,  0.5, 0), MathTuple.GetVector(1,  0,  0)),
            new(MathTuple.GetPoint(0.5, 5,   0), MathTuple.GetVector(0,  -1, 0)),
            new(MathTuple.GetPoint(0.5, -5,  0), MathTuple.GetVector(0,  1,  0)),
            new(MathTuple.GetPoint(0.5, 0,   5), MathTuple.GetVector(0,  0,  -1)),
            new(MathTuple.GetPoint(0.5, 0,   -5), MathTuple.GetVector(0, 0,  1)),
            new(MathTuple.GetPoint(0,   0.5, 0), MathTuple.GetVector(-0, 0,  1)),
        };
        var raysIntersections = new List<Intersection[]>()
        {
            c.LocalIntersect(r[0]), c.LocalIntersect(r[1]), c.LocalIntersect(r[2]), c.LocalIntersect(r[3]),
            c.LocalIntersect(r[4]), c.LocalIntersect(r[5]), c.LocalIntersect(r[6])
        };
        foreach (var intersections in raysIntersections)
        {
            Assert.Equal(2, intersections.Length);
        }

        Assert.Equal(4,  raysIntersections[0][0].Distance);
        Assert.Equal(6,  raysIntersections[0][1].Distance);
        Assert.Equal(4,  raysIntersections[1][0].Distance);
        Assert.Equal(6,  raysIntersections[1][1].Distance);
        Assert.Equal(4,  raysIntersections[2][0].Distance);
        Assert.Equal(6,  raysIntersections[2][1].Distance);
        Assert.Equal(4,  raysIntersections[3][0].Distance);
        Assert.Equal(6,  raysIntersections[3][1].Distance);
        Assert.Equal(4,  raysIntersections[4][0].Distance);
        Assert.Equal(6,  raysIntersections[4][1].Distance);
        Assert.Equal(4,  raysIntersections[5][0].Distance);
        Assert.Equal(6,  raysIntersections[5][1].Distance);
        Assert.Equal(-1, raysIntersections[6][0].Distance);
        Assert.Equal(1,  raysIntersections[6][1].Distance);
    }

    [Fact]
    public void RayMisses()
    {
        var c = new Cube();
        var r = new List<Ray>()
        {
            new(MathTuple.GetPoint(-2, 0,  0), MathTuple.GetVector(0.2673,  0.5345, 0.8018)),
            new(MathTuple.GetPoint(0,  -2, 0), MathTuple.GetVector(0.8018,  0.2673, 0.5345)),
            new(MathTuple.GetPoint(0,  0,  -2), MathTuple.GetVector(0.5345, 0.8018, 0.2673)),
            new(MathTuple.GetPoint(2,  0,  2), MathTuple.GetVector(0,       0,      -1)),
            new(MathTuple.GetPoint(0,  2,  2), MathTuple.GetVector(0,       -1,     0)),
            new(MathTuple.GetPoint(2,  2,  0), MathTuple.GetVector(-1,      0,      0)),
        };
        var raysIntersections = new List<Intersection[]>()
        {
            c.LocalIntersect(r[0]), c.LocalIntersect(r[1]), c.LocalIntersect(r[2]), c.LocalIntersect(r[3]),
            c.LocalIntersect(r[4]), c.LocalIntersect(r[5])
        };
        foreach (var intersections in raysIntersections)
        {
            Assert.Empty(intersections);
        }
    }

    [Fact]
    public void Normals()
    {
        var c = new Cube();
        var r = new List<MathTuple>()
        {
            MathTuple.GetPoint(1,   0.5, -0.8), MathTuple.GetPoint(-1,   -0.2, 0.9), MathTuple.GetPoint(-0.4, 1, -0.1),
            MathTuple.GetPoint(0.3, -1,  -0.7), MathTuple.GetPoint(-0.6, 0.3,  1), MathTuple.GetPoint(0.4,    0.4, -1),
            MathTuple.GetPoint(1,   1,   1), MathTuple.GetPoint(-1,      -1,   -1),
        };
        var normals = new List<MathTuple>()
        {
            c.LocalNormal(r[0]), c.LocalNormal(r[1]), c.LocalNormal(r[2]), c.LocalNormal(r[3]),
            c.LocalNormal(r[4]), c.LocalNormal(r[5]), c.LocalNormal(r[6]), c.LocalNormal(r[7])
        };
        Assert.Equal(MathTuple.GetVector(1,  0,  0),  normals[0]);
        Assert.Equal(MathTuple.GetVector(-1, 0,  0),  normals[1]);
        Assert.Equal(MathTuple.GetVector(0,  1,  0),  normals[2]);
        Assert.Equal(MathTuple.GetVector(0,  -1, 0),  normals[3]);
        Assert.Equal(MathTuple.GetVector(0,  0,  1),  normals[4]);
        Assert.Equal(MathTuple.GetVector(0,  0,  -1), normals[5]);
        Assert.Equal(MathTuple.GetVector(1,  0,  0),  normals[6]);
        Assert.Equal(MathTuple.GetVector(-1, 0,  0),  normals[7]);
    }
}
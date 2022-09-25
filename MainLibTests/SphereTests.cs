using MainLib;
using Xunit;

namespace MainLibTests;

public class SphereTests
{
    [Fact]
    public void DefaultTransformation()
    {
        var s = new Sphere();
        var identity = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        Assert.Equal(identity, s.Transformation);
    }

    [Fact]
    public void SetTransformation()
    {
        var s = new Sphere();
        var translationMatrix = Transforms.GetTranslationMatrix(2, 3, 4);
        s.Transformation = translationMatrix;
        Assert.Equal(translationMatrix, s.Transformation);
    }

    [Fact]
    public void IntersectScaledSphere()
    {
        var s = new Sphere();
        var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
        s.Transformation = Transforms.GetScalingMatrix(2, 2, 2);
        var rs = r.Intersects(s);
        Assert.Equal(2, rs.Count);
        Assert.Equal(3, rs[0].Distance);
        Assert.Equal(7, rs[1].Distance);
    }
}
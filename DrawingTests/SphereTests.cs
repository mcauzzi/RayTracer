using Drawing;
using MainLib;

namespace DrawingTests;

public class SphereTests
{
    [Fact]
    public void DefaultTransformation()
    {
        var s        = new Sphere();
        var identity = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        Assert.Equal(identity, s.Transformation);
    }

    [Fact]
    public void SetTransformation()
    {
        var s                 = new Sphere();
        var translationMatrix = Transforms.GetTranslationMatrix(2, 3, 4);
        s.Transformation = translationMatrix;
        Assert.Equal(translationMatrix, s.Transformation);
    }

    [Fact]
    public void IntersectScaledSphere()
    {
        var s = new Sphere();
        var r = new Ray(MathTuple.GetPoint(0, 0, -5), MathTuple.GetVector(0, 0, 1));
        s.Transformation = Transforms.GetScalingMatrix(2, 2, 2);
        var rs = r.Intersects(s);
        Assert.Equal(2, rs.Length);
        Assert.Equal(3, rs[0]
            .Distance);
        Assert.Equal(7, rs[1]
            .Distance);
    }

    [Fact]
    public void NormalXAxis()
    {
        var s = new Sphere();
        var v = s.Normal(MathTuple.GetPoint(1, 0, 0));
        Assert.Equal(MathTuple.GetVector(1, 0, 0), v);
    }

    [Fact]
    public void NormalYAxis()
    {
        var s = new Sphere();
        var v = s.Normal(MathTuple.GetPoint(0, 1, 0));
        Assert.Equal(MathTuple.GetVector(0, 1, 0), v);
    }

    [Fact]
    public void NormalZAxis()
    {
        var s = new Sphere();
        var v = s.Normal(MathTuple.GetPoint(0, 0, 1));
        Assert.Equal(MathTuple.GetVector(0, 0, 1), v);
    }

    [Fact]
    public void NormalNonAxial()
    {
        var s = new Sphere();
        var v = s.Normal(MathTuple.GetPoint(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
        Assert.Equal(MathTuple.GetVector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3), v);
    }

    [Fact]
    public void NormalIsNormalized()
    {
        var s = new Sphere();
        var v = s.Normal(MathTuple.GetPoint(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
        Assert.Equal(v.Normalize(), v);
    }

    [Fact]
    public void NormalTranslated()
    {
        var s = new Sphere();
        s.Transformation = Transforms.GetScalingMatrix(1, 0.5, 1) * Transforms.RotateZ(Math.PI / 5);
        var v = s.Normal(MathTuple.GetPoint(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));
        Assert.Equal(MathTuple.GetVector(0, 0.97014, -0.24254), v);
    }

    [Fact]
    public void NormalTransformed()
    {
        var s = new Sphere();
        s.Transformation = Transforms.GetTranslationMatrix(0, 1, 0);
        var v = s.Normal(MathTuple.GetPoint(0, 1.70711, -0.70711));
        Assert.Equal(MathTuple.GetVector(0, 0.70711, -0.70711), v);
    }

    [Fact]
    public void SphereMaterial()
    {
        var s = new Sphere();
        var m = new Material() { Ambient = 1 };
        s.Material = m;
        Assert.Equal(m, s.Material);
    }

    [Fact]
    public void GlassSphere()
    {
        var s = Sphere.GlassSphere;
        Assert.Equal(1.0, s.Material.Transparency);
        Assert.Equal(1.5, s.Material.RefractiveIndex);
    }
}
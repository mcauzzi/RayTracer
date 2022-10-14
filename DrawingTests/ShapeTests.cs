using Drawing;
using Drawing.Shapes;
using MainLib;

namespace DrawingTests;

public class ShapeTests
{
    [Fact]
    public void DefaultTransformation()
    {
        var s = new Sphere();
        Assert.Equal(new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } }),
            s.Transformation);
    }

    [Fact]
    public void TransformAssignment()
    {
        var s = new Sphere();
        s.Transformation = Transforms.GetTranslationMatrix(2, 3, 4);
        Assert.Equal(Transforms.GetTranslationMatrix(2, 3, 4), s.Transformation);
    }

    [Fact]
    public void DefaultMaterial()
    {
        var s = new Sphere();
        Assert.Equal(new Material(), s.Material);
    }

    [Fact]
    public void MaterialAssignment()
    {
        var s = new Sphere();
        var m = new Material() { Ambient = 1 };
        s.Material = m;
        Assert.Equal(m, s.Material);
    }
}
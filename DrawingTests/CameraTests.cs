using Drawing;
using GlobalConstants;
using MainLib;

namespace DrawingTests;

public class CameraTests
{
    [Fact]
    public void Creation()
    {
        var c = new Camera(160, 120, Math.PI / 2);
        Assert.Equal(160, c.HSize);
        Assert.Equal(120, c.VSize);
        Assert.Equal(Math.PI / 2, c.FOV);
        Assert.Equal(new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } }),
            c.Transform);
    }

    [Fact]
    public void PixelSizeHorizontalCanvas()
    {
        var c = new Camera(200, 125, Math.PI / 2);
        Assert.True(Math.Abs(0.01 - c.PixelSize) < Constants.Epsilon);
    }

    [Fact]
    public void PixelSizeVerticalCanvas()
    {
        var c = new Camera(125, 200, Math.PI / 2);
        Assert.True(Math.Abs(0.01 - c.PixelSize) < Constants.Epsilon);
    }

    [Fact]
    public void RayThroughCenterCanvas()
    {
        var c = new Camera(201, 101, Math.PI / 2);
        Ray r = c.RayForPixel(100, 50);
        Assert.Equal(new Point(0, 0, 0), r.Origin);
        Assert.Equal(new Vector(0, 0, -1), r.Direction);
    }

    [Fact]
    public void RayThroughCornerCanvas()
    {
        var c = new Camera(201, 101, Math.PI / 2);
        Ray r = c.RayForPixel(0, 0);
        Assert.Equal(new Point(0, 0, 0), r.Origin);
        Assert.Equal(new Vector(0.66519, 0.33259, -0.66851), r.Direction);
    }

    [Fact]
    public void RayCameraTransformed()
    {
        var c = new Camera(201, 101, Math.PI / 2);
        c.Transform = Transforms.RotateY(Math.PI / 4) * Transforms.GetTranslationMatrix(0, -2, 5);

        Ray r = c.RayForPixel(100, 50);
        Assert.Equal(new Point(0, 2, -5), r.Origin);
        Assert.Equal(new Vector(Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2), r.Direction);
    }

    [Fact]
    public void RenderWorldWithCamera()
    {
        var w = new World();
        var c = new Camera(11, 11, Math.PI / 2);
        var from = new Point(0, 0, -5);
        var to = new Point(0, 0, 0);
        var up = new Vector(0, 1, 0);
        c.Transform = Transforms.ViewTransform(from, to, up);
        Canvas canv = c.Render(w);
        Assert.Equal(new Color(0.38066, 0.47583, 0.2855), canv.PixelAt(5, 5));
    }
}
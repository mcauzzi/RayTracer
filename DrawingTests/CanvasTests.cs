using Drawing;

namespace DrawingTests;

public class CanvasTests
{
    [Fact]
    public void Creation()
    {
        var a = new Canvas(10, 20);
        Assert.Equal(20, a.Height);
        Assert.Equal(10, a.Width);
    }

    [Fact]
    public void PixelWriting()
    {
        var a = new Canvas(10, 20);
        var red = new Color(1, 0, 0);
        a.WritePixel(red, 2, 3);
        Assert.Equal(red, a.PixelAt(2, 3));
    }
}
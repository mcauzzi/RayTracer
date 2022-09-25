using MainLib;

namespace DrawingTests;

public class ColorTests
{
    [Fact]
    public void Add()
    {
        var a = new Color(0.9, 0.6, 0.75);
        var b = new Color(0.7, 0.1, 0.25);
        Assert.Equal(a + b, new Color(1.6, 0.7, 1.0));
    }

    [Fact]
    public void Subtract()
    {
        var a = new Color(0.9, 0.6, 0.75);
        var b = new Color(0.7, 0.1, 0.25);
        Assert.Equal(a - b, new Color(0.2, 0.5, 0.5));
    }

    [Fact]
    public void MultiplyByScalar()
    {
        var a = new Color(0.2, 0.3, 0.4);
        var b = 2;
        Assert.Equal(a * b, new Color(0.4, 0.6, 0.8));
    }

    [Fact]
    public void MultiplyTwoColors()
    {
        var a = new Color(1, 0.2, 0.4);
        var b = new Color(0.9, 1, 0.1);
        Assert.Equal(a * b, new Color(0.9, 0.2, 0.04));
    }
}
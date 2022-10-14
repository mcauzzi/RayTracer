using Drawing;
using FileManagement;

namespace FileTests;

public class BmpTests
{
    [Fact]
    public void ClampMax()
    {
        var col = new Color(1, 1, 1);
        var res = col.ClampColor();
        Assert.Equal(255, res[0]);
        Assert.Equal(255, res[1]);
        Assert.Equal(255, res[2]);
    }

    [Fact]
    public void ClampMin()
    {
        var col = new Color(0, 0, 0);
        var res = col.ClampColor();
        Assert.Equal(0, res[0]);
        Assert.Equal(0, res[1]);
        Assert.Equal(0, res[2]);
    }

    [Fact]
    public void ClampMid()
    {
        var col = new Color(0.5, 0.5, 0.5);
        var res = col.ClampColor();
        Assert.Equal(128, res[0]);
        Assert.Equal(128, res[1]);
        Assert.Equal(128, res[2]);
    }
}
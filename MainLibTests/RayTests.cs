using MainLib;
using Xunit;

namespace MainLibTests;

public class RayTests
{
    [Fact]
    public void RayCreation()
    {
        var r = new Ray(new Point(1, 2, 3), new Vector(4, 5, 6));
        Assert.Equal(new Point(1, 2, 3), r.Origin);
        Assert.Equal(new Vector(4, 5, 6), r.Direction);
    }

    [Fact]
    public void Position()
    {
        var r = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));
        Assert.Equal(r.Position(0), new Point(2, 3, 4));
        Assert.Equal(r.Position(1), new Point(3, 3, 4));
        Assert.Equal(r.Position(-1), new Point(1, 3, 4));
        Assert.Equal(r.Position(2.5), new Point(4.5, 3, 4));
    }
}
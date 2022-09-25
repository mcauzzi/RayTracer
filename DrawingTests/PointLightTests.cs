using MainLib;

namespace DrawingTests;

public class PointLightTests
{
    [Fact]
    public void Creation()
    {
        var intensity = new Color(1, 1, 1);
        var position = new Point(0, 0, 0);
        var l = new PointLight(intensity, position);

        Assert.Equal(intensity, l.Intensity);
        Assert.Equal(position, l.Position);
    }
}
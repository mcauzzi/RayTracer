using MainLib;

namespace DrawingTests;

public class PointLightTests
{
    [Fact]
    public void Creation()
    {
        var intensity = Color.White;
        var position  = new Point(0, 0, 0);
        var l         = new PointLight(intensity, position);

        Assert.Equal(intensity, l.Intensity);
        Assert.Equal(position,  l.Position);
    }
}
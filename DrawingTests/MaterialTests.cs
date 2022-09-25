using MainLib;

namespace DrawingTests;

public class MaterialTests
{
    [Fact]
    public void EyeBetweenLightAndSurface()
    {
        var m = new Material();
        var pos = new Point(0, 0, 0);
        var eyeV = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light = new PointLight(new Color(1, 1, 1), new Point(0, 0, -10));
        var res = m.GetLighting(light, pos, eyeV, normalV);
        Assert.Equal(new Color(1.9, 1.9, 1.9), res);
    }

    [Fact]
    public void EyeOffset45DegBetweenLightAndSurface()
    {
        var m = new Material();
        var pos = new Point(0, 0, 0);
        var eyeV = new Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);
        var normalV = new Vector(0, 0, -1);
        var light = new PointLight(new Color(1, 1, 1), new Point(0, 0, -10));
        var res = m.GetLighting(light, pos, eyeV, normalV);
        Assert.Equal(new Color(1, 1, 1), res);
    }

    [Fact]
    public void EyeOppositeSurfaceLightOffset45Deg()
    {
        var m = new Material();
        var pos = new Point(0, 0, 0);
        var eyeV = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light = new PointLight(new Color(1, 1, 1), new Point(0, 10, -10));
        var res = m.GetLighting(light, pos, eyeV, normalV);
        Assert.Equal(new Color(0.7364, 0.7364, 0.7364), res);
    }

    [Fact]
    public void EyeInThePathReflectionVector()
    {
        var m = new Material();
        var pos = new Point(0, 0, 0);
        var eyeV = new Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
        var normalV = new Vector(0, 0, -1);
        var light = new PointLight(new Color(1, 1, 1), new Point(0, 10, -10));
        var res = m.GetLighting(light, pos, eyeV, normalV);
        Assert.Equal(new Color(1.6364, 1.6364, 1.6364), res);
    }

    [Fact]
    public void LightBehindSurface()
    {
        var m = new Material();
        var pos = new Point(0, 0, 0);
        var eyeV = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light = new PointLight(new Color(1, 1, 1), new Point(0, 0, 10));
        var res = m.GetLighting(light, pos, eyeV, normalV);
        Assert.Equal(new Color(0.1, 0.1, 0.1), res);
    }
}
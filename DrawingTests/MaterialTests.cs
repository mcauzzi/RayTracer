using Drawing;
using Drawing.Patterns;
using MainLib;

namespace DrawingTests;

public class MaterialTests
{
    [Fact]
    public void EyeBetweenLightAndSurface()
    {
        var m       = new Material();
        var pos     = new Point(0, 0, 0);
        var eyeV    = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light   = new PointLight(Color.White, new Point(0, 0, -10));
        var res     = m.GetLighting(light, new Sphere(), pos, eyeV, normalV);
        Assert.Equal(new Color(1.9, 1.9, 1.9), res);
    }

    [Fact]
    public void EyeOffset45DegBetweenLightAndSurface()
    {
        var m       = new Material();
        var pos     = new Point(0, 0, 0);
        var eyeV    = new Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);
        var normalV = new Vector(0, 0,                -1);
        var light   = new PointLight(Color.White, new Point(0, 0, -10));
        var res     = m.GetLighting(light, new Sphere(), pos, eyeV, normalV);
        Assert.Equal(Color.White, res);
    }

    [Fact]
    public void EyeOppositeSurfaceLightOffset45Deg()
    {
        var m       = new Material();
        var pos     = new Point(0, 0, 0);
        var eyeV    = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light   = new PointLight(Color.White, new Point(0, 10, -10));
        var res     = m.GetLighting(light, new Sphere(), pos, eyeV, normalV);
        Assert.Equal(new Color(0.7364, 0.7364, 0.7364), res);
    }

    [Fact]
    public void EyeInThePathReflectionVector()
    {
        var m       = new Material();
        var pos     = new Point(0, 0, 0);
        var eyeV    = new Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
        var normalV = new Vector(0, 0,                 -1);
        var light   = new PointLight(Color.White, new Point(0, 10, -10));
        var res     = m.GetLighting(light, new Sphere(), pos, eyeV, normalV);
        Assert.Equal(new Color(1.6364, 1.6364, 1.6364), res);
    }

    [Fact]
    public void LightBehindSurface()
    {
        var m       = new Material();
        var pos     = new Point(0, 0, 0);
        var eyeV    = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light   = new PointLight(Color.White, new Point(0, 0, 10));
        var res     = m.GetLighting(light, new Sphere(), pos, eyeV, normalV);
        Assert.Equal(new Color(0.1, 0.1, 0.1), res);
    }

    [Fact]
    public void LightingShadowedSurface()
    {
        var m       = new Material();
        var pos     = new Point(0, 0, 0);
        var eyev    = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light   = new PointLight(Color.White, new Point(0, 0, -10));
        var res     = m.GetLighting(light, new Sphere(), pos, eyev, normalV, true);
        Assert.Equal(new Color(0.1, 0.1, 0.1), res);
    }

    [Fact]
    public void LightingWithPattern()
    {
        var m = new Material()
            { Ambient = 1, Diffuse = 0, Specular = 0, Pattern = new StripePattern(Color.White, Color.Black) };
        var eyev    = new Vector(0, 0, -1);
        var normalV = new Vector(0, 0, -1);
        var light   = new PointLight(Color.White, new Point(0,          0, -10));
        var c1      = m.GetLighting(light, new Sphere(), new Point(0.9, 0, 0), eyev, normalV);
        var c2      = m.GetLighting(light, new Sphere(), new Point(1.1, 0, 0), eyev, normalV);
        Assert.Equal(Color.White, c1);
        Assert.Equal(Color.Black, c2);
    }
}
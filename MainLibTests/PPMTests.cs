using MainLib;
using Xunit;

namespace MainLibTests;

public class PPMTests
{
    [Fact]
    public void TestHeader()
    {
        var c = new Canvas(5, 3);
        var ppm = new PPMCreator(c);
        var splittedString = ppm.File.Split("\n");
        Assert.Equal("P3\r", splittedString[0]);
        Assert.Equal("5 3\r", splittedString[1]);
        Assert.Equal("255\r", splittedString[2]);
    }

    [Fact]
    public void PPMData()
    {
        var c = new Canvas(5, 3);
        c.WritePixel(new Color(1.5, 0, 0), 0, 0);
        c.WritePixel(new Color(0, 0.5, 0), 2, 1);
        c.WritePixel(new Color(-0.5, 0, 1), 4, 2);
        var ppm = new PPMCreator(c);
        var splittedString = ppm.File.Split("\n");
        Assert.Equal("255 0 0 0 0 0 0 0 0 0 0 0 0 0 0\r", splittedString[3]);
        Assert.Equal("0 0 0 0 0 0 0 128 0 0 0 0 0 0 0\r", splittedString[4]);
        Assert.Equal("0 0 0 0 0 0 0 0 0 0 0 0 0 0 255\r", splittedString[5]);
    }

    [Fact]
    public void LineLength()
    {
        var c = new Canvas(10, 2);
        for (int i = 0; i < c.Height; i++)
        {
            for (int j = 0; j < c.Width; j++)
            {
                c.WritePixel(new Color(1, 0.8, 0.6), j, i);
            }
        }

        var ppm = new PPMCreator(c);
        var splittedString = ppm.File.Split("\n");
        Assert.All(splittedString, x => Assert.True(x.Length < 70));
    }
}
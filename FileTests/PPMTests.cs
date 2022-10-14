using Drawing;
using FileManagement;

namespace FileTests;

public class PPMTests
{
    [Fact]
    public void TestHeader()
    {
        var c = new Canvas(5, 3);
        var ppm = new PPMCreator(c);
        var splittedString = ppm.FileContent.Split("\n");
        Assert.Equal((string)"P3\r", (string)splittedString[0]);
        Assert.Equal((string)"5 3\r", (string)splittedString[1]);
        Assert.Equal((string)"255\r", (string)splittedString[2]);
    }

    [Fact]
    public void PPMData()
    {
        var c = new Canvas(5, 3);
        c.WritePixel(new Color(1.5, 0, 0), 0, 0);
        c.WritePixel(new Color(0, 0.5, 0), 2, 1);
        c.WritePixel(new Color(-0.5, 0, 1), 4, 2);
        var ppm = new PPMCreator(c);
        var splittedString = ppm.FileContent.Split("\n");
        Assert.Equal((string)"255 0 0 0 0 0 0 0 0 0 0 0 0 0 0\r", (string)splittedString[3]);
        Assert.Equal((string)"0 0 0 0 0 0 0 128 0 0 0 0 0 0 0\r", (string)splittedString[4]);
        Assert.Equal((string)"0 0 0 0 0 0 0 0 0 0 0 0 0 0 255\r", (string)splittedString[5]);
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
        var splittedString = ppm.FileContent.Split("\n");
        Assert.All<string>(splittedString, x => Assert.True(x.Length < 70));
    }
}
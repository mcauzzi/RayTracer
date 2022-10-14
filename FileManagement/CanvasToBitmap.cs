using Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = Drawing.Color;

namespace FileManagement;

public static class CanvasToBitmap
{
    private const int MAX_COLOR_VALUE = 255;

    public static void CreateBitmap(Canvas c, string fileName)
    {
        using Image<Rgba32> image = new(c.Width, c.Height);
        for (var i = 0; i < image.Height; i++)
        {
            for (var j = 0; j < image.Width; j++)
            {
                var color = c.PixelAt(j, i).ClampColor();
                image[j, i] = new Rgba32(color[0], color[1], color[2]);
            }
        }

        image.SaveAsBmp(fileName);
    }

    public static byte[] ClampColor(this Color c)
    {
        var clampedRed = (byte)Math.Min(Math.Round(c.R * MAX_COLOR_VALUE), MAX_COLOR_VALUE);
        var clampedGreen = (byte)Math.Min(Math.Round(c.G * MAX_COLOR_VALUE), MAX_COLOR_VALUE);
        var clampedBlue = (byte)Math.Min(Math.Round(c.B * MAX_COLOR_VALUE), MAX_COLOR_VALUE);
        return new[] { clampedRed, clampedGreen, clampedBlue };
    }
}
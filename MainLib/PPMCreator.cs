using System;
using System.IO;
using System.Text;

namespace MainLib;

public class PPMCreator
{
    public const int MAX_COLOR_VALUE = 255;

    public PPMCreator(Canvas c)
    {
        var sb = new StringBuilder();
        sb.AppendLine("P3");
        sb.AppendLine($"{c.Width} {c.Height}");
        sb.AppendLine(MAX_COLOR_VALUE.ToString());
        var temp = "";
        var colorCounter = 1;
        for (var i = 0; i < c.Height; i++)
        {
            for (var j = 0; j < c.Width; j++)
            {
                var pixelAt = c.PixelAt(j, i);
                var colorString = ClampColor(pixelAt);
                if ((temp + " " + colorString).Length > 70 || colorCounter > c.Width)
                {
                    sb.AppendLine(temp);
                    temp = colorString;
                    colorCounter = 1;
                }
                else if (i == 0 && j == 0)
                {
                    temp += colorString;
                }
                else
                {
                    temp += " " + colorString;
                }

                colorCounter++;
            }
        }

        sb.AppendLine(temp);
        sb.AppendLine();
        FileContent = sb.ToString();
    }

    public string FileContent { get; }

    public void WriteToFile()
    {
        File.WriteAllText("Test.ppm", FileContent);
    }

    private static string ClampColor(Color c)
    {
        var clampedRed = Math.Min(Math.Round(c.R * MAX_COLOR_VALUE), MAX_COLOR_VALUE);
        var clampedGreen = Math.Min(Math.Round(c.G * MAX_COLOR_VALUE), MAX_COLOR_VALUE);
        var clampedBlue = Math.Min(Math.Round(c.B * MAX_COLOR_VALUE), MAX_COLOR_VALUE);
        return $"{Math.Max(clampedRed, 0)} {Math.Max(clampedGreen, 0)} {Math.Max(clampedBlue, 0)}";
    }
}
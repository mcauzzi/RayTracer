using MainLib;

namespace Drawing.Patterns;

public class GradientPattern : Pattern
{
    public GradientPattern(Color initialColor, Color endColor)
    {
        InitialColor = initialColor;
        EndColor     = endColor;
        Distance     = EndColor - InitialColor;
    }

    private Color Distance { get; set; }

    public Color InitialColor { get; }
    public Color EndColor     { get; }

    public override Color ColorAt(MathTuple point)
    {
        var frac = point.X - Math.Floor(point.X);

        return InitialColor + Distance * frac;
    }
}
using MainLib;

namespace Drawing.Patterns;

public class RingPattern : Pattern
{
    public RingPattern(Color firstColor, Color secondColor)
    {
        FirstColor  = firstColor;
        SecondColor = secondColor;
    }

    public Color FirstColor  { get; }
    public Color SecondColor { get; }

    public override Color ColorAt(MathTuple point)
    {
        var res = Math.Floor(Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Z, 2)));
        return res % 2 == 0 ? FirstColor : SecondColor;
    }
}
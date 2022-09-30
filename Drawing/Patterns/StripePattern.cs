using MainLib;

namespace Drawing.Patterns;

public class StripePattern : Pattern
{
    public StripePattern(Color firstColor, Color secondColor)
    {
        FirstColor  = firstColor;
        SecondColor = secondColor;
    }

    public Color FirstColor  { get; }
    public Color SecondColor { get; }

    public override Color ColorAt(MathTuple point)
    {
        return Math.Floor(point.X) % 2 == 0 ? FirstColor : SecondColor;
    }
}
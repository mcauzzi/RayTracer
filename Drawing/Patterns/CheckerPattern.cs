using MainLib;

namespace Drawing.Patterns;

public class CheckerPattern : Pattern
{
    public CheckerPattern(Color firstColor, Color secondColor)
    {
        FirstColor  = firstColor;
        SecondColor = secondColor;
    }

    public Color FirstColor  { get; }
    public Color SecondColor { get; }

    public override Color ColorAt(MathTuple point)
    {
        return (Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z)) % 2 == 0 ? FirstColor : SecondColor;
    }
}

public class RandomPattern : Pattern
{
    public RandomPattern()
    {
        Generator = new Random();
    }

    public Random Generator { get; }

    public override Color ColorAt(MathTuple point)
    {
        return new Color(Generator.NextDouble(), Generator.NextDouble(), Generator.NextDouble());
    }
}
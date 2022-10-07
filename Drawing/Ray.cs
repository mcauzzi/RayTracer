using MainLib;

namespace Drawing;

public struct Ray
{
    public Ray(MathTuple origin, MathTuple direction)
    {
        Direction = direction;
        Origin    = origin;
    }

    public MathTuple Origin    { get; }
    public MathTuple Direction { get; }

    public MathTuple Position(double distance)
    {
        var res = (Origin + Direction * distance);
        return MathTuple.GetPoint(res.X, res.Y, res.Z);
    }

    /// <summary>
    /// Controlla le intersezioni con una sfera
    /// </summary>
    /// <param name="s">La sfera con cui verficare le intersezioni</param>
    /// <returns>Le distanze in cui il raggio interseca la sfera</returns>
    public Intersection[] Intersects(Sphere s)
    {
        return s.Intersect(this);
    }

    public Ray Transform(Matrix m)
    {
        var transformedOrigin    = m * Origin;
        var transformedDirection = m * Direction;
        return new Ray(MathTuple.GetPoint(transformedOrigin.X, transformedOrigin.Y, transformedOrigin.Z),
            MathTuple.GetVector(transformedDirection.X, transformedDirection.Y, transformedDirection.Z));
    }
}
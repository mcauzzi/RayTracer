namespace MainLib;

public class Ray
{
    public Ray(Point origin, Vector direction)
    {
        Direction = direction;
        Origin = origin;
    }

    public Point Origin { get; }
    public Vector Direction { get; }

    public Point Position(double distance)
    {
        var res = (Origin + Direction * distance);
        return new Point(res.X, res.Y, res.Z);
    }
}
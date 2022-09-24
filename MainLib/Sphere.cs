namespace MainLib;

public class Sphere
{
    public Sphere(Point origin, double radius)
    {
        Radius = radius;
        Origin = origin;
    }

    public Point Origin { get; }
    public double Radius { get; }
}
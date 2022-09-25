using MainLib;

namespace Drawing;

public class Ray
{
    public Ray(Point origin, MathTuple direction)
    {
        Direction = direction;
        Origin = origin;
    }

    public Point Origin { get; }
    public MathTuple Direction { get; }

    public Point Position(double distance)
    {
        var res = (Origin + Direction * distance);
        return new Point(res.X, res.Y, res.Z);
    }

    /// <summary>
    /// Controlla le intersezioni con una sfera
    /// </summary>
    /// <param name="s">La sfera con cui verficare le intersezioni</param>
    /// <returns>Le distanze in cui il raggio interseca la sfera</returns>
    public List<Intersection>
        Intersects(Sphere s)
    {
        var result = new List<Intersection>();
        var transformedRay = Transform(s.Transformation.GetInverse());
        var sphereToRay = transformedRay.Origin - s.Origin;
        var a = MathTuple.DotProduct(transformedRay.Direction, transformedRay.Direction);
        var b = 2 * MathTuple.DotProduct(transformedRay.Direction, sphereToRay);
        var c = MathTuple.DotProduct(sphereToRay, sphereToRay) - 1;
        var discriminant = Math.Pow(b, 2) - 4 * a * c;
        if (discriminant < 0)
        {
            return result;
        }

        result.Add(new Intersection(s, (-b - Math.Sqrt(discriminant)) / (2 * a)));
        result.Add(new Intersection(s, (-b + Math.Sqrt(discriminant)) / (2 * a)));
        return result;
    }

    public Ray Transform(Matrix m)
    {
        var transformedOrigin = m * Origin;
        var transformedDirection = m * Direction;
        return new Ray(new Point(transformedOrigin.X, transformedOrigin.Y, transformedOrigin.Z),
            new Vector(transformedDirection.X, transformedDirection.Y, transformedDirection.Z));
    }
}
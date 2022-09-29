using GlobalConstants;
using MainLib;

namespace Drawing;

public class Intersection
{
    public Intersection(Shape obj, double distance)
    {
        Obj = obj;
        Distance = distance;
    }

    public double Distance { get; }
    public Shape Obj { get; }

    public Computation PrepareComputation(Ray r)
    {
        var point = r.Position(Distance);
        var normalV = Obj.Normal(point);
        var eyeV = -r.Direction;
        var inside = false;
        if (MathTuple.DotProduct(normalV, eyeV) < 0)
        {
            normalV = -normalV;
            inside = true;
        }


        var res = new Computation(Distance, Obj, point, eyeV, normalV, inside);
        res.OverPoint = res.Point + (res.NormalV * Constants.Epsilon);
        return res;
    }

    /// <summary>
    /// Ritorna la distanza del primo HIT su un'oggetto
    /// </summary>
    /// <param name="xs">Una lista d'intersezioni con un oggetto</param>
    /// <returns>Le distanze in cui il raggio interseca la sfera</returns>
    public static Intersection? Hit(List<Intersection> xs)
    {
        return xs.OrderBy(x => x.Distance).FirstOrDefault(x => x.Distance > 0);
    }
}

public class Computation
{
    public Computation(double distance, Shape obj, Point point, MathTuple eyeV, MathTuple normalV, bool inside)
    {
        Distance = distance;
        Obj = obj;
        Point = point;
        EyeV = eyeV;
        NormalV = normalV;
        IsInside = inside;
    }

    public double Distance { get; }
    public Shape Obj { get; }
    public Point Point { get; }
    public MathTuple EyeV { get; }
    public MathTuple NormalV { get; }
    public bool IsInside { get; }
    public MathTuple OverPoint { get; set; }
}
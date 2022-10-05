using GlobalConstants;
using MainLib;

namespace Drawing;

public class Intersection
{
    public Intersection(Shape obj, double distance)
    {
        Obj      = obj;
        Distance = distance;
    }

    public double Distance { get; }
    public Shape  Obj      { get; }

    public Computation PrepareComputation(Ray r, List<Intersection>? intersections = null)
    {
        var point   = r.Position(Distance);
        var normalV = Obj.Normal(point);
        var eyeV    = -r.Direction;
        var inside  = false;
        if (MathTuple.DotProduct(normalV, eyeV) < 0)
        {
            normalV = -normalV;
            inside  = true;
        }

        var res = new Computation(Distance, Obj, point, eyeV, normalV, inside);
        if (intersections != null)
        {
            CalculateN1N2(intersections, res);
        }

        res.ReflectV   = r.Direction.Reflect(res.NormalV);
        res.OverPoint  = res.Point + (res.NormalV * Constants.Epsilon);
        res.UnderPoint = res.Point - (res.NormalV * Constants.Epsilon);
        return res;
    }

    private void CalculateN1N2(List<Intersection>? intersections, Computation res)
    {
        var containers = new List<Shape>();
        foreach (var i in intersections)
        {
            ComputeN1(res, i, containers);

            if (containers.Contains(i.Obj))
            {
                containers.Remove(i.Obj);
            }
            else
            {
                containers.Add(i.Obj);
            }

            if (i != this) continue;
            ComputeN2(res, containers);

            return;
        }
    }

    private void ComputeN2(Computation res, List<Shape> containers)
    {
        if (!containers.Any())
        {
            res.N2 = 1.0;
        }
        else
        {
            res.N2 = containers.TakeLast(1)
                .First()
                .Material.RefractiveIndex;
        }
    }

    private void ComputeN1(Computation res, Intersection i, List<Shape> containers)
    {
        if (i == this)
        {
            if (!containers.Any())
            {
                res.N1 = 1.0;
            }
            else
            {
                res.N1 = containers.TakeLast(1)
                    .First()
                    .Material.RefractiveIndex;
            }
        }
    }

    /// <summary>
    /// Ritorna la distanza del primo HIT su un'oggetto
    /// </summary>
    /// <param name="xs">Una lista d'intersezioni con un oggetto</param>
    /// <returns>Le distanze in cui il raggio interseca la sfera</returns>
    public static Intersection? Hit(List<Intersection> xs)
    {
        return xs.OrderBy(x => x.Distance)
            .FirstOrDefault(x => x.Distance > 0);
    }
}

public class Computation
{
    public Computation(double distance, Shape obj, Point point, MathTuple eyeV, MathTuple normalV, bool inside)
    {
        Distance = distance;
        Obj      = obj;
        Point    = point;
        EyeV     = eyeV;
        NormalV  = normalV;
        IsInside = inside;
    }

    public double    Distance   { get; }
    public Shape     Obj        { get; }
    public Point     Point      { get; }
    public MathTuple EyeV       { get; }
    public MathTuple NormalV    { get; }
    public bool      IsInside   { get; }
    public MathTuple OverPoint  { get; set; }
    public MathTuple ReflectV   { get; set; }
    public double    N1         { get; set; }
    public double    N2         { get; set; }
    public MathTuple UnderPoint { get; set; }

    public double Schlick()
    {
        var cos = MathTuple.DotProduct(EyeV, NormalV);
        if (N1 > N2)
        {
            var n     = N1 / N2;
            var sin2t = Math.Pow(n, 2) * (1 - Math.Pow(cos, 2));
            if (sin2t > 1)
            {
                return 1.0;
            }

            cos = Math.Sqrt(1 - sin2t);
        }

        var r0 = Math.Pow((N1 - N2) / (N1 + N2), 2);
        return r0 + (1 - r0) * Math.Pow(1 - cos, 5);
    }
}
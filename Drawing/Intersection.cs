using Globals;
using MainLib;

namespace Drawing;

public class Intersection
{
    public Intersection(Shape obj, double distance)
    {
        Obj      = obj;
        Distance = distance;
    }

    public static IComparer<Intersection> DistanceComparer { get; } = new DistanceRelationalComparer();

    public double Distance { get; }
    public Shape  Obj      { get; }

    public Computation PrepareComputation(Ray r, Intersection[]? intersections = null)
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

        var    ReflectV   = r.Direction.Reflect(normalV);
        var    OverPoint  = point + (normalV * Constants.Epsilon);
        var    UnderPoint = point - (normalV * Constants.Epsilon);
        double n1         = 0, n2 = 0;
        if (intersections?.Length > 0)
        {
            CalculateN1N2(intersections, out n1, out n2);
        }

        var res = new Computation(Distance, Obj, point, eyeV, normalV, inside, ReflectV, OverPoint, UnderPoint, n1, n2);


        return res;
    }

    private void CalculateN1N2(Intersection[] intersections, out double n1, out double n2)
    {
        var containers = new List<Shape>();
        n1 = n2        = 0;
        for (var index = 0; index < intersections.Length; index++)
        {
            var i = intersections[index];
            if (i == this)
            {
                if (!containers.Any())
                {
                    n1 = 1.0;
                }
                else
                {
                    n1 = containers.TakeLast(1)
                        .First()
                        .Material.RefractiveIndex;
                }
            }

            if (containers.Contains(i.Obj))
            {
                containers.Remove(i.Obj);
            }
            else
            {
                containers.Add(i.Obj);
            }

            if (i != this) continue;
            if (i == this)
            {
                if (!containers.Any())
                {
                    n2 = 1.0;
                }
                else
                {
                    n2 = containers.TakeLast(1)
                        .First()
                        .Material.RefractiveIndex;
                }
            }
        }
    }

    /// <summary>
    /// Ritorna la distanza del primo HIT su un'oggetto
    /// </summary>
    /// <param name="xs">Una lista d'intersezioni con un oggetto</param>
    /// <returns>Le distanze in cui il raggio interseca la sfera</returns>
    public static Intersection? Hit(Intersection[] xs)
    {
        return xs.FirstOrDefault(x => x.Distance > 0);
    }

    private sealed class DistanceRelationalComparer : IComparer<Intersection>
    {
        public int Compare(Intersection x, Intersection y)
        {
            if (ReferenceEquals(x,    y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.Distance.CompareTo(y.Distance);
        }
    }
}

public struct Computation
{
    public Computation(double distance, Shape obj, MathTuple point, MathTuple eyeV, MathTuple normalV, bool inside,
        MathTuple reflectV, MathTuple overPoint, MathTuple underPoint, double n1, double n2)
    {
        Distance   = distance;
        Obj        = obj;
        Point      = point;
        EyeV       = eyeV;
        NormalV    = normalV;
        IsInside   = inside;
        OverPoint  = overPoint;
        ReflectV   = reflectV;
        UnderPoint = underPoint;
        N1         = n1;
        N2         = n2;
    }


    public double    Distance   { get; }
    public Shape     Obj        { get; }
    public MathTuple Point      { get; }
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
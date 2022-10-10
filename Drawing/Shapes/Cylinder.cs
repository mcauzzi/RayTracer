using Globals;
using MainLib;

namespace Drawing.Shapes;

public class Cylinder : Shape
{
    public Cylinder()
    {
        Maximum = double.PositiveInfinity;
        Minimum = double.NegativeInfinity;
    }

    public double Minimum      { get; init; }
    public double Maximum      { get; init; }
    public bool   ClosedTop    { get; init; }
    public bool   ClosedBottom { get; init; }

    public override MathTuple LocalNormal(MathTuple point)
    {
        var dist = point.X * point.X + point.Z * point.Z;
        if (dist < 1 && point.Y >= Maximum - Constants.Epsilon)
        {
            return MathTuple.GetVector(0, 1, 0);
        }

        if (dist < 1 && point.Y <= Minimum + Constants.Epsilon)
        {
            return MathTuple.GetVector(0, -1, 0);
        }

        return MathTuple.GetVector(point.X, 0, point.Z);
    }

    public override Intersection[] LocalIntersect(Ray r)
    {
        var res = new List<Intersection>(2);
        var a   = r.Direction.X * r.Direction.X + r.Direction.Z * r.Direction.Z;
        if (Math.Abs(a) < Constants.Epsilon)
        {
            IntersectCaps(r, res);
            return res.ToArray();
        }

        var b    = 2 * r.Origin.X * r.Direction.X + 2 * r.Origin.Z * r.Direction.Z;
        var c    = r.Origin.X * r.Origin.X + r.Origin.Z * r.Origin.Z - 1;
        var disc = b * b - 4 * a * c;
        if (disc < 0)
        {
            return res.ToArray();
        }

        var int0 = (-b - Math.Sqrt(disc)) / (2 * a);
        var int1 = (-b + Math.Sqrt(disc)) / (2 * a);
        if (int0 > int1)
        {
            (int0, int1) = (int1, int0); //Cool feature, doest not create a tuple, uses a hidden temp value
        }

        var y0 = r.Origin.Y + int0 * r.Direction.Y;
        var y1 = r.Origin.Y + int1 * r.Direction.Y;
        if (y0 > Minimum && y0 < Maximum)
        {
            res.Add(new(this, int0));
        }

        if (y1 > Minimum && y1 < Maximum)
        {
            res.Add(new(this, int1));
        }

        IntersectCaps(r, res);
        return res.ToArray();
    }

    private void IntersectCaps(Ray r, List<Intersection> res)
    {
        if (Math.Abs(r.Direction.Y) < Constants.Epsilon)
        {
            return;
        }

        var tTop    = (Maximum - r.Origin.Y) / r.Direction.Y;
        var tBottom = (Minimum - r.Origin.Y) / r.Direction.Y;
        if (ClosedTop && CheckCap(r, tTop))
        {
            res.Add(new(this, tTop));
        }

        if (ClosedBottom && CheckCap(r, tBottom))
        {
            res.Add(new(this, tBottom));
        }
    }

    private static bool CheckCap(Ray r, double t)
    {
        var x = r.Origin.X + t * r.Direction.X;
        var z = r.Origin.Z + t * r.Direction.Z;
        return (x * x + z * z) <= 1;
    }
}
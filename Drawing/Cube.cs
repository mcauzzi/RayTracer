using Globals;
using MainLib;

namespace Drawing;

public class Cube : Shape
{
    public override MathTuple LocalNormal(MathTuple point)
    {
        var maxc = HelperMethods.Max(Math.Abs(point.X), Math.Abs(point.Y), Math.Abs(point.Z));

        if (Math.Abs(maxc - Math.Abs(point.X)) < Constants.Epsilon)
        {
            return MathTuple.GetVector(point.X, 0, 0);
        }

        if (Math.Abs(maxc - Math.Abs(point.Y)) < Constants.Epsilon)
        {
            return MathTuple.GetVector(0, point.Y, 0);
        }

        return MathTuple.GetVector(0, 0, point.Z);
    }

    public override Intersection[] LocalIntersect(Ray r)
    {
        double[] xt = CheckAxis(r.Origin.X, r.Direction.X);
        double[] yt = CheckAxis(r.Origin.Y, r.Direction.Y);
        double[] zt = CheckAxis(r.Origin.Z, r.Direction.Z);

        var tmin = HelperMethods.Max(xt[0], yt[0], zt[0]); //Is it really better than a double call to Math.Max
        var tmax = HelperMethods.Min(xt[1], yt[1], zt[1]); //Same as above but with Math.Min

        return tmin > tmax
            ? Array.Empty<Intersection>()
            : new[] { new Intersection(this, tmin), new Intersection(this, tmax) };
    }

    private double[] CheckAxis(double origin, double direction)
    {
        var minNumerator = -1 - origin;
        var maxNumerator = 1 - origin;
        var res          = new double[2];
        if (Math.Abs(direction) >= Constants.Epsilon)
        {
            res[0] = minNumerator / direction;
            res[1] = maxNumerator / direction;
        }
        else
        {
            res[0] = minNumerator * double.PositiveInfinity;
            res[1] = maxNumerator * double.PositiveInfinity;
        }

        return res[0] > res[1] ? new[] { res[1], res[0] } : res;
    }
}
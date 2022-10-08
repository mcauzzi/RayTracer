using Globals;
using MainLib;

namespace Drawing;

public class Plane : Shape
{
    public override MathTuple LocalNormal(MathTuple point)
    {
        return MathTuple.GetVector(0, 1, 0);
    }

    public override Intersection[] LocalIntersect(Ray r)
    {
        if (!(Math.Abs(r.Direction.Y) < Constants.Epsilon))
        {
            return new[] { new Intersection(this, (-r.Origin.Y) / r.Direction.Y) };
        }

        return Array.Empty<Intersection>();
    }
}
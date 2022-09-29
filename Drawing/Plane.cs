using GlobalConstants;
using MainLib;

namespace Drawing;

public class Plane : Shape
{
    public override MathTuple LocalNormal(MathTuple objPoint)
    {
        return new Vector(0, 1, 0);
    }

    public override List<Intersection> LocalIntersect(Ray r)
    {
        var res = new List<Intersection>();
        if (!(Math.Abs(r.Direction.Y) < Constants.Epsilon))
            return new List<Intersection>() { new(this, (-r.Origin.Y) / r.Direction.Y) };
        return new List<Intersection>();
    }
}
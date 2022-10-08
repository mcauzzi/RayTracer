using MainLib;

namespace Drawing;

public class Sphere : Shape, IEquatable<Sphere>
{
    public Sphere(MathTuple origin, double radius)
    {
        Radius = radius;
        Origin = origin;
    }

    public Sphere()
    {
        Radius = 1;
        Origin = MathTuple.GetPoint(0, 0, 0);
    }

    public static Sphere GlassSphere => new()
        { Material = new Material() { Transparency = 1, RefractiveIndex = 1.5 } };

    public MathTuple Origin { get; }
    public double    Radius { get; }

    public override Intersection[] LocalIntersect(Ray r)
    {
        var sphereToRay  = r.Origin - Origin;
        var a            = MathTuple.DotProduct(r.Direction, r.Direction);
        var b            = 2 * MathTuple.DotProduct(r.Direction, sphereToRay);
        var c            = MathTuple.DotProduct(sphereToRay,     sphereToRay) - 1;
        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return Array.Empty<Intersection>();
        }

        return new[]
        {
            new Intersection(this, (-b - Math.Sqrt(discriminant)) / (2 * a)),
            new Intersection(this, (-b + Math.Sqrt(discriminant)) / (2 * a))
        };
    }

    public override MathTuple LocalNormal(MathTuple p)
    {
        return p - MathTuple.GetPoint(0, 0, 0);
    }

    #region Equality

    public bool Equals(Sphere? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Origin.Equals(other.Origin) && Radius.Equals(other.Radius) &&
               Transformation.Equals(other.Transformation) && Material.Equals(other.Material);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Sphere)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Origin.GetHashCode();
            hashCode = (hashCode * 397) ^ Radius.GetHashCode();
            hashCode = (hashCode * 397) ^ Transformation.GetHashCode();
            hashCode = (hashCode * 397) ^ Material.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(Sphere? left, Sphere? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Sphere? left, Sphere? right)
    {
        return !Equals(left, right);
    }

    #endregion
}
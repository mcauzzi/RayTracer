using MainLib;

namespace Drawing;

public class Sphere : IEquatable<Sphere>
{
    public Sphere(Point origin, double radius)
    {
        Radius = radius;
        Origin = origin;
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        Material = new Material();
    }

    public Sphere()
    {
        Radius = 1;
        Origin = new Point(0, 0, 0);
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        Material = new Material();
    }

    public Point Origin { get; }
    public double Radius { get; }
    public Matrix Transformation { get; set; }
    public Material Material { get; set; }

    public MathTuple Normal(Point p)
    {
        var inverseTrans = Transformation.GetInverse();
        var objPoint = inverseTrans * p;
        var objNormal = objPoint - new Point(0, 0, 0);
        var worldNormal = inverseTrans.Transpose() * objNormal;
        worldNormal.W = 0;
        return worldNormal.Normalize();
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
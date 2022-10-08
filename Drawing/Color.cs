using Globals;

namespace MainLib;

public class Color : IEquatable<Color>
{
    public Color(double r, double g, double b)
    {
        R = r;
        G = g;
        B = b;
    }

    public        double R     { get; }
    public        double G     { get; }
    public        double B     { get; }
    public static Color  Black => new Color(0, 0, 0);
    public static Color  White => new Color(1, 1, 1);
    public static Color  Red   => new Color(1, 0, 0);
    public static Color  Green => new Color(0, 1, 0);
    public static Color  Blue  => new Color(0, 0, 1);

    #region Operators

    public static Color operator +(Color left, Color right)
    {
        return new Color(left.R + right.R, left.G + right.G, left.B + right.B);
    }

    public static Color operator -(Color left, Color right)
    {
        return new Color(left.R - right.R, left.G - right.G, left.B - right.B);
    }

    public static Color operator *(Color left, Color right)
    {
        return new Color(left.R * right.R, left.G * right.G, left.B * right.B);
    }

    public static Color operator *(Color left, double right)
    {
        return new Color(left.R * right, left.G * right, left.B * right);
    }

    #endregion

    #region Equality

    public bool Equals(Color other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Math.Abs(R - other.R) < Constants.Epsilon && Math.Abs(G - other.G) < Constants.Epsilon &&
               Math.Abs(B - other.B) < Constants.Epsilon;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Color)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = R.GetHashCode();
            hashCode = (hashCode * 397) ^ G.GetHashCode();
            hashCode = (hashCode * 397) ^ B.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(Color left, Color right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Color left, Color right)
    {
        return !Equals(left, right);
    }

    #endregion
}
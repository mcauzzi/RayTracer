namespace MainLib;

public class PointLight : IEquatable<PointLight>
{
    public PointLight(Color intensity, MathTuple position)
    {
        Intensity = intensity;
        Position  = position;
    }

    public Color     Intensity { get; }
    public MathTuple Position  { get; }

    #region Equality

    public bool Equals(PointLight? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Intensity.Equals(other.Intensity) && Position.Equals(other.Position);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PointLight)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Intensity.GetHashCode() * 397) ^ Position.GetHashCode();
        }
    }

    public static bool operator ==(PointLight? left, PointLight? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PointLight? left, PointLight? right)
    {
        return !Equals(left, right);
    }

    #endregion
}
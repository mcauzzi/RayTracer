using Drawing.Patterns;
using Drawing.Shapes;
using Globals;
using MainLib;

namespace Drawing;

public class Material : IEquatable<Material>
{
    public Material(double shininess, double specular, double diffuse, double ambient, Color color)
    {
        Shininess = shininess;
        Specular = specular;
        Diffuse = diffuse;
        Ambient = ambient;
        Color = color;
    }

    public Material()
    {
        Color = Color.White;
        Ambient = 0.1;
        Diffuse = 0.9;
        Specular = 0.9;
        Shininess = 200;
        Reflective = 0;
        Transparency = 0;
        RefractiveIndex = 1;
    }

    public Color Color { get; set; }
    public Pattern? Pattern { get; set; }
    public double Ambient { get; set; }
    public double Diffuse { get; set; }
    public double Specular { get; set; }
    public double Shininess { get; set; }
    public double Reflective { get; set; }
    public double Transparency { get; set; }
    public double RefractiveIndex { get; set; }


    public Color GetLighting(PointLight pl, Shape shape, MathTuple position, MathTuple eyeDirection,
        MathTuple normalVector,
        bool inShadow = false)
    {
        var color = Pattern != null ? Pattern.ColorAtObject(shape, position) : Color;
        var effColor = color * pl.Intensity;
        var lightV = (pl.Position - position).Normalize();
        var ambient = effColor * Ambient;
        var lightDotNormal = MathTuple.DotProduct(lightV, normalVector);
        var diffuse = Color.Black;
        var specular = Color.Black;
        if (lightDotNormal > 0 && !inShadow)
        {
            diffuse = effColor * Diffuse * lightDotNormal;
            var reflectV = -lightV.Reflect(normalVector);
            var reflectDotEye = MathTuple.DotProduct(reflectV, eyeDirection);
            if (reflectDotEye > 0)
            {
                var factor = Math.Pow(reflectDotEye, Shininess);
                specular = pl.Intensity * Specular * factor;
            }
        }

        return ambient + diffuse + specular;
    }

    #region Equality

    public bool Equals(Material? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Color.Equals(other.Color) && Math.Abs(Ambient - other.Ambient) < Constants.Epsilon &&
               Math.Abs(Diffuse - other.Diffuse) < Constants.Epsilon &&
               Math.Abs(Specular - other.Specular) < Constants.Epsilon &&
               Math.Abs(Shininess - other.Shininess) < Constants.Epsilon;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Material)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Color.GetHashCode();
            hashCode = (hashCode * 397) ^ Ambient.GetHashCode();
            hashCode = (hashCode * 397) ^ Diffuse.GetHashCode();
            hashCode = (hashCode * 397) ^ Specular.GetHashCode();
            hashCode = (hashCode * 397) ^ Shininess.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(Material? left, Material? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Material? left, Material? right)
    {
        return !Equals(left, right);
    }

    #endregion
}
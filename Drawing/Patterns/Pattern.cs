using MainLib;

namespace Drawing.Patterns;

public class Pattern
{
    public Pattern()
    {
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
    }

    public Matrix Transformation { get; set; }

    public virtual Color ColorAt(MathTuple point)
    {
        return new Color(point.X, point.Y, point.Z);
    }

    public Color ColorAtObject(Shape shape, MathTuple point)
    {
        var objectPoint  = shape.Transformation.GetInverse() * point;
        var patternPoint = Transformation.GetInverse() * objectPoint;
        return ColorAt(patternPoint);
    }
}
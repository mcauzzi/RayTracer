using MainLib;

namespace Drawing.Patterns;

public abstract class Pattern
{
    public Pattern()
    {
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
    }

    public Matrix Transformation { get; set; }

    public abstract Color ColorAt(MathTuple point);

    public Color ColorAtObject(Shape shape, MathTuple point)
    {
        var objectPoint  = shape.Transformation.GetInverse() * point;
        var patternPoint = Transformation.GetInverse() * objectPoint;
        return ColorAt(patternPoint);
    }
}
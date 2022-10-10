using Drawing.Shapes;
using MainLib;

namespace Drawing.Patterns;

public class Pattern
{
    private Matrix _transformation;

    public Pattern()
    {
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
    }

    public Matrix Transformation
    {
        get => _transformation;
        set
        {
            _transformation       = value;
            TransformationInverse = value.GetInverse();
        }
    }

    public Matrix TransformationInverse { get; private set; }

    public virtual Color ColorAt(MathTuple point)
    {
        return new Color(point.X, point.Y, point.Z);
    }

    public Color ColorAtObject(Shape shape, MathTuple point)
    {
        var objectPoint  = shape.TransformationInverse * point;
        var patternPoint = TransformationInverse * objectPoint;
        return ColorAt(patternPoint);
    }
}
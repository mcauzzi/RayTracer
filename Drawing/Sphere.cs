using MainLib;

namespace Drawing;

public class Sphere
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
}
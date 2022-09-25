namespace MainLib;

public class Sphere
{
    public Sphere(Point origin, double radius)
    {
        Radius = radius;
        Origin = origin;
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
    }

    public Sphere()
    {
        Radius = 1;
        Origin = new Point(0, 0, 0);
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
    }

    public Point Origin { get; }
    public double Radius { get; }
    public Matrix Transformation { get; set; }
}
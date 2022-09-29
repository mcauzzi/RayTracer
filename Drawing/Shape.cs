using MainLib;

namespace Drawing;

public abstract class Shape
{
    public Shape()
    {
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        Material = new Material();
    }

    public Matrix Transformation { get; set; }
    public Material Material { get; set; }

    public MathTuple Normal(Point p)
    {
        var inverseTrans = Transformation.GetInverse();
        var objPoint = inverseTrans * p;
        var objNormal = LocalNormal(objPoint);
        var worldNormal = inverseTrans.Transpose() * objNormal;
        worldNormal.W = 0;
        return worldNormal.Normalize();
    }

    public abstract MathTuple LocalNormal(MathTuple objPoint);

    public abstract List<Intersection> LocalIntersect(Ray r);

    public List<Intersection> Intersect(Ray r)
    {
        var localRay = r.Transform(Transformation.GetInverse());
        return LocalIntersect(localRay);
    }
}
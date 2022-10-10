using MainLib;

namespace Drawing.Shapes;

public abstract class Shape
{
    private Matrix _transformation;

    public Shape()
    {
        Transformation = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        Material       = new Material();
    }

    public Matrix Transformation
    {
        get => _transformation;
        set
        {
            _transformation       = value;
            TransformationInverse = _transformation.GetInverse();
        }
    }

    public Matrix TransformationInverse { get; private set; }

    public Material Material { get; set; }

    public MathTuple Normal(MathTuple p)
    {
        var inverseTrans = TransformationInverse;
        var objPoint     = inverseTrans * p;
        var objNormal    = LocalNormal(objPoint);
        var worldNormal  = inverseTrans.Transpose() * objNormal;
        worldNormal.W = 0;
        return worldNormal.Normalize();
    }

    public abstract MathTuple LocalNormal(MathTuple point);

    public abstract Intersection[] LocalIntersect(Ray r);

    public Intersection[] Intersect(Ray r)
    {
        var localRay = r.Transform(TransformationInverse);
        return LocalIntersect(localRay);
    }
}
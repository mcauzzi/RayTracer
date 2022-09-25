namespace MainLib;

public class Vector : MathTuple
{
    public Vector(double x, double y, double z) : base(x, y, z, 0)
    {
    }

    public MathTuple Reflect(Vector normal)
    {
        return this - normal * 2 * DotProduct(this, normal);
    }
}
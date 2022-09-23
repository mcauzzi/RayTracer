namespace MainLib
{
    public static class Transforms
    {
        public static Matrix GetTranslationMatrix(double x, double y, double z)
        {
            return new Matrix(new[,] { { 1, 0, 0, x }, { 0, 1, 0, y }, { 0, 0, 1, z }, { 0, 0, 0, 1 } });
        }

        public static Matrix GetScalingMatrix(double x, double y, double z)
        {
            return new Matrix(new double[,] { { x, 0, 0, 0 }, { 0, y, 0, 0 }, { 0, 0, z, 0 }, { 0, 0, 0, 1 } });
        }
    }
}
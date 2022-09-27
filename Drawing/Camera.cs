using GlobalConstants;
using MainLib;

namespace Drawing;

public class Camera
{
    public Camera(int hSize, int vSize, double fov)
    {
        HSize = hSize;
        VSize = vSize;
        FOV = fov;

        Transform = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        SetPixelSize();
    }

    public int HSize { get; }
    public int VSize { get; }
    public double FOV { get; }
    public double PixelSize { get; private set; }
    public Matrix Transform { get; set; }

    public double HalfHeight { get; private set; }

    public double HalfWidth { get; private set; }

    private void SetPixelSize()
    {
        var halfView = Math.Tan(FOV / 2);
        var aspect = HSize / (double)VSize;
        if (aspect >= 1)
        {
            HalfWidth = halfView;
            HalfHeight = halfView / aspect;
        }
        else
        {
            HalfWidth = halfView * aspect;
            HalfHeight = halfView;
        }

        PixelSize = (HalfWidth * 2) / HSize;
    }

    public Ray RayForPixel(int px, int py)
    {
        var offsetX = (px + 0.5) * PixelSize;
        var offsetY = (py + 0.5) * PixelSize;
        var worldX = HalfWidth - offsetX;
        var worldY = HalfHeight - offsetY;
        var inverseTransform = Transform.GetInverse();
        var pixel = inverseTransform * new Point(worldX, worldY, -1);
        var origin = inverseTransform * new Point(0, 0, 0);
        var direction = (pixel - origin).Normalize();
        return new Ray(origin, direction);
    }

    public Canvas Render(World world)
    {
        var res = new Canvas(HSize, VSize);
        var tasks = new List<Thread>();
        var rowsPerThread = VSize / Constants.NUMBER_OF_THREADS;
        for (int thr = 0; thr < Constants.NUMBER_OF_THREADS; thr++)
        {
            var thrEff = thr;
            var thread = new Thread(() =>
            {
                for (int i = rowsPerThread * thrEff; i < rowsPerThread * (thrEff + 1); i++)
                {
                    for (int j = 0; j < HSize; j++)
                    {
                        GetRenderedPixel(world, j, i, res);
                    }
                }
            });
            thread.Start();
            tasks.Add(thread);
        }

        while (tasks.All(x => x.IsAlive))
        {
            Thread.Sleep(100);
        }

        return res;
    }

    private void GetRenderedPixel(World world, int j, int i, Canvas res)
    {
        var ray = RayForPixel(j, i);
        var c = world.ColorAt(ray);
        res.WritePixel(c, j, i);
    }
}
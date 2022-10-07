using GlobalConstants;
using MainLib;

namespace Drawing;

public class Camera
{
    private Matrix _transform;

    public Camera(int hSize, int vSize, double fov)
    {
        HSize = hSize;
        VSize = vSize;
        FOV   = fov;

        Transform = new Matrix(new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });
        SetPixelSize();
    }

    public int    HSize     { get; }
    public int    VSize     { get; }
    public double FOV       { get; }
    public double PixelSize { get; private set; }

    public Matrix Transform
    {
        get => _transform;
        set
        {
            _transform       = value;
            InverseTransform = value.GetInverse();
            OriginPoint      = InverseTransform * MathTuple.GetPoint(0, 0, 0);
        }
    }

    private MathTuple OriginPoint { get; set; }

    private Matrix InverseTransform { get; set; }

    private double HalfHeight { get; set; }

    private double HalfWidth { get; set; }

    private void SetPixelSize()
    {
        var halfView = Math.Tan(FOV / 2);
        var aspect   = HSize / (double)VSize;
        if (aspect >= 1)
        {
            HalfWidth  = halfView;
            HalfHeight = halfView / aspect;
        }
        else
        {
            HalfWidth  = halfView * aspect;
            HalfHeight = halfView;
        }

        PixelSize = (HalfWidth * 2) / HSize;
    }

    public Ray RayForPixel(int px, int py)
    {
        var offsetX   = (px + 0.5) * PixelSize;
        var offsetY   = (py + 0.5) * PixelSize;
        var worldX    = HalfWidth - offsetX;
        var worldY    = HalfHeight - offsetY;
        var pixel     = InverseTransform * MathTuple.GetPoint(worldX, worldY, -1);
        var direction = (pixel - OriginPoint).Normalize();
        return new Ray(OriginPoint, direction);
    }

    public Canvas RenderMultiThreaded(World world)
    {
        var res   = new Canvas(HSize, VSize);
        var tasks = new List<Thread>();
        var rowsPerThread = VSize / Constants.NUMBER_OF_THREADS == 0
            ? VSize
            : (int)Math.Ceiling(VSize / (double)Constants.NUMBER_OF_THREADS);
        var threads = rowsPerThread == VSize ? 1 : Constants.NUMBER_OF_THREADS;
        Console.WriteLine($"rowsPerThread={rowsPerThread}, threads={threads}");
        for (int thr = 0; thr < threads; thr++)
        {
            var thrEff = thr;
            var thread = new Thread(() =>
            {
                for (int i = rowsPerThread * thrEff; i < rowsPerThread * (thrEff + 1) && i < VSize; i++)
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

        foreach (var thread in tasks)
        {
            thread.Join();
        }

        return res;
    }

    public Canvas RenderWithTasks(World world)
    {
        var res      = new Canvas(HSize, VSize);
        var taskList = new List<Task>();
        for (int i = 0; i < VSize; i++)
        {
            var i1 = i;
            taskList.Add(Task.Run(() => RenderRow(world, i1, res)));
            if (taskList.Count > Constants.NUMBER_OF_THREADS)
            {
                Task.WaitAll(taskList.ToArray());
                taskList.Clear();
            }
        }

        return res;
    }

    private void RenderRow(World world, int i1, Canvas res)
    {
        for (int j = 0; j < HSize; j++)
        {
            GetRenderedPixel(world, j, i1, res);
        }
    }

    public Canvas RenderSingleThread(World world)
    {
        var res = new Canvas(HSize, VSize);

        for (int i = 0; i < VSize; i++)
        {
            for (int j = 0; j < HSize; j++)
            {
                GetRenderedPixel(world, j, i, res);
            }
        }

        return res;
    }

    private void GetRenderedPixel(World world, int j, int i, Canvas res)
    {
        var ray = RayForPixel(j, i);
        var c   = world.ColorAt(ray, 5);
        res.WritePixel(c, j, i);
    }
}
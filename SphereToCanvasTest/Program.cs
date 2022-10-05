// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using Drawing;
using MainLib;

var floor = new Plane
{
    Material = new Material
    {
        Color      = new Color(0, 0, 1), Specular = 0.2, Diffuse       = 0.2, Ambient = 0.1,
        Reflective = 0.3, Transparency            = 0, RefractiveIndex = 1.5
    }
};
var backGround = new Plane
{
    Material = new Material
        { Color = new Color(0.8, 0.8, 0.8), Specular = 0.4, Diffuse = 0.2, Reflective = 0, RefractiveIndex = 0 },
    Transformation = Transforms.GetTranslationMatrix(0, 0, 10) * Transforms.RotateX(Math.PI / 2)
};
var sphereList = new List<Sphere>();
var rnd        = new Random();
for (int i = 0; i < 4; i++)
{
    for (int j = 0; j < 4; j++)
    {
        for (int k = 0; k < 4; k++)
        {
            sphereList.Add(new Sphere
            {
                Transformation = Transforms.GetTranslationMatrix(-13 + i * 3, -9 + j * 3, 8 - k * 4),
                Material = new Material
                {
                    Color   = new Color(i / (double)3, j / (double)3, k / (double)3),
                    Ambient = 0.4, Diffuse = 0.1, Specular = 0.6, Reflective = 0.6,
                    // Pattern = new RingPattern(new Color(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()),
                    //         new Color(rnd.NextDouble(),                   rnd.NextDouble(), rnd.NextDouble()))
                    //     { Transformation = Transforms.GetScalingMatrix(2,1,1) }
                }
            });
        }
    }
}

var camera = new Camera(1920, 1080, Math.PI / 3)
{
    Transform = Transforms.ViewTransform(new Point(9, -5, -11), new Point(-18, -5, 13), new Vector(0, 1, 0))
};
var pl2    = new PointLight(Color.White, new Point(1, 2, -6));
var shapes = new List<Shape> { backGround };
shapes.AddRange(sphereList);
var world = new World(new List<PointLight> { pl2 },
    shapes);

var st = new Stopwatch();

st.Start();

var canvas = camera.RenderMultiThreaded(world);
st.Stop();
Console.WriteLine($"Total Render Time {st.Elapsed}");
new PPMCreator(canvas).WriteToFile("Sphere");
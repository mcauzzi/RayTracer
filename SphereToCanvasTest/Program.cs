// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using Drawing;
using Drawing.Shapes;
using MainLib;

var backGroundMaterial = new Material
{
    Color = new Color(0.3, 0.3, 0.3), Specular = 0.2, Ambient = 0.1, Diffuse = 0.4, Reflective = 0.1, Transparency = 0,
    RefractiveIndex = 1.52, Shininess = 300
};
var floor = new Plane
{
    Material = backGroundMaterial
};
var background = new Plane
{
    Material = backGroundMaterial,
    Transformation = Transforms.GetTranslationMatrix(10, 0, 10) * Transforms.RotateX(Math.PI / 2) *
                     Transforms.RotateZ(-Math.PI / 4)
};

var cylinder = new Cylinder()
{
    ClosedBottom   = true, ClosedTop = true, Maximum = 4, Minimum = 0,
    Transformation = Transforms.GetTranslationMatrix(5, 0, 5),
    Material = new Material()
    {
        Color = new Color(0.1, 0.1, 0.1), Specular = 1, Ambient = 0.1, Diffuse = 0.1, Reflective = 0, Transparency = 1,
        RefractiveIndex = 1, Shininess = 300
    }
};
var cube = new Cube()
{
    Transformation = Transforms.GetTranslationMatrix(4.5, 1, 12),
    Material = new Material()
    {
        Color           = Color.Red, Specular = 0.8, Ambient = 0.1, Diffuse = 0.5, Reflective = 1, Transparency = 0,
        RefractiveIndex = 1
    }
};
var sphereInside = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(5, 2, 5) * Transforms.GetScalingMatrix(0.5, 0.5, 0.5),
    Material = new Material()
    {
        Color           = Color.Blue, Specular = 1, Ambient = 0.1, Diffuse = 0.1, Reflective = 1, Transparency = 0,
        Shininess       = 300,
        RefractiveIndex = 1.52
    }
};
var sphere = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(2.5, 1, 5),
    Material = new Material()
    {
        Color           = Color.Green, Specular = 0.8, Ambient = 0.2, Diffuse = 0.4, Reflective = 0, Transparency = 0,
        RefractiveIndex = 1.52
    }
};
var sphere2 = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(5, 1, 2),
    Material = new Material()
    {
        Color        = new Color(0.7, 0.7, 0.3), Specular = 0.8, Ambient = 0.2, Diffuse = 0.4, Reflective = 0,
        Transparency = 0, RefractiveIndex                 = 1.52
    }
};
var camera = new Camera(1280, 720, Math.PI / 2)
{
    Transform = Transforms.ViewTransform(
        MathTuple.GetPoint(0, 5, 0),
        MathTuple.GetPoint(5, 0, 5),
        MathTuple.GetPoint(0, 1, 0)
    ),
};
var pl     = new PointLight(Color.White, MathTuple.GetPoint(7,  2, -4));
var pl2    = new PointLight(Color.White, MathTuple.GetPoint(-7, 2, 4));
var shapes = new List<Shape> { floor, cylinder, sphere, cube, sphere2, sphereInside, background };
var world = new World(new List<PointLight> { pl, pl2 },
    shapes);

var st = new Stopwatch();

st.Start();
var canvas = camera.RenderWithTasks(world);
st.Stop();
Console.WriteLine(
    $"Total Render Time {st.Elapsed}, Pixel/s:{canvas.Height * canvas.Width / (double)st.ElapsedMilliseconds * 1000}");
new PPMCreator(canvas).WriteToFile("Sphere");

void CreateCubeOfSpheres(int cubeLength1, List<Shape> spheres, int startCubex1, int startCubeZ1)
{
    for (int i = 0; i < cubeLength1; i++)
    {
        for (int j = 0; j < cubeLength1; j++)
        {
            for (int k = 0; k < cubeLength1; k++)
            {
                var redColor   = i / ((double)cubeLength1 - 1);
                var greenColor = j / ((double)cubeLength1 - 1);
                var blueColor  = k / ((double)cubeLength1 - 1);

                spheres.Add(new Sphere
                {
                    Transformation =
                        Transforms.GetTranslationMatrix(startCubex1 - i * 2.5, 0 + j * 2.5, startCubeZ1 - k * 2.5),
                    Material = new Material
                    {
                        Color           = new Color(redColor, greenColor, blueColor),
                        Ambient         = 0.1, Diffuse   = 0.1, Specular = 0.8, Reflective = 0, Transparency = 0,
                        RefractiveIndex = 1.2, Shininess = 300,
                    }
                });
            }
        }
    }
}

void CreateCubeOfCubes(int cubeLength1, List<Shape> cubes, int startCubex1, int startCubeZ1)
{
    for (int i = 0; i < cubeLength1; i++)
    {
        for (int j = 0; j < cubeLength1; j++)
        {
            for (int k = 0; k < cubeLength1; k++)
            {
                var redColor   = i / ((double)cubeLength1 - 1);
                var greenColor = j / ((double)cubeLength1 - 1);
                var blueColor  = k / ((double)cubeLength1 - 1);

                cubes.Add(new Cube
                {
                    Transformation =
                        Transforms.GetTranslationMatrix(startCubex1 - i * 2.5, 0 + j * 2.5, startCubeZ1 - k * 2.5),
                    Material = new Material
                    {
                        Color           = new Color(redColor, greenColor, blueColor),
                        Ambient         = 0.1, Diffuse = 0.5, Specular = 0.8, Reflective = 0.4, Transparency = 1,
                        RefractiveIndex = 1, Shininess = 300,
                    }
                });
            }
        }
    }
}

void CreateCubeOfCylinders(int cubeLength, List<Shape> cubes, int startCubex1, int startCubeZ1)
{
    for (int i = 0; i < cubeLength; i++)
    {
        for (int j = 0; j < cubeLength; j++)
        {
            for (int k = 0; k < cubeLength; k++)
            {
                var redColor   = i / ((double)cubeLength - 1);
                var greenColor = j / ((double)cubeLength - 1);
                var blueColor  = k / ((double)cubeLength - 1);

                cubes.Add(new Cylinder()
                {
                    ClosedBottom = true, ClosedTop = true, Maximum = 1, Minimum = -1,
                    Transformation =
                        Transforms.GetTranslationMatrix(startCubex1 - i * 2.5, 0 + j * 2.5, startCubeZ1 - k * 2.5),
                    Material = new Material
                    {
                        Color           = new Color(redColor, greenColor, blueColor),
                        Ambient         = 0.1, Diffuse = 0.5, Specular = 0.8, Reflective = 0.4, Transparency = 0.1,
                        RefractiveIndex = 1, Shininess = 300,
                    }
                });
            }
        }
    }
}
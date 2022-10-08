// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using Drawing;
using MainLib;

var backGroundMaterial = new Material
{
    Color           = Color.Red, Specular = 0.2, Ambient = 0.1, Diffuse = 0.1, Reflective = 0, Transparency = 1,
    RefractiveIndex = 1.52, Shininess     = 300
};
var backGround2 = new Plane
{
    Material       = backGroundMaterial,
    Transformation = Transforms.GetTranslationMatrix(0, 11, 0)
};
var backGround3 = new Plane
{
    Material = new Material
    {
        Color           = new Color(0.1, 0.1, 0.1), Specular = 1, Ambient = 0.4, Diffuse = 0.1, Reflective = 1,
        RefractiveIndex = 1.5, Shininess                     = 200
    },
    Transformation = Transforms.GetTranslationMatrix(0, 0, 20) * Transforms.RotateZ(Math.PI / 2) *
                     Transforms.RotateX(Math.PI / 2)
};
var       shapesList = new List<Shape>();
const int cubeLength = 4;
const int startCubex = 10;
const int startCubeZ = 10;
CreateCubeOfSpheres(cubeLength, shapesList, startCubex, startCubeZ);
CreateCubeOfCubes(cubeLength, shapesList, startCubex, startCubeZ);
var camera = new Camera(1280, 720, Math.PI / 2)
{
    Transform = Transforms.ViewTransform(
        MathTuple.GetPoint(-7, 5, -7),
        MathTuple.GetPoint(10, 5, 10),
        MathTuple.GetPoint(0,  1, 0)
    ),
};
var pl2    = new PointLight(Color.White, MathTuple.GetPoint(5, 12, 5));
var pl     = new PointLight(Color.White, MathTuple.GetPoint(5, 5,  0));
var shapes = new List<Shape> { };
shapes.AddRange(shapesList);
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
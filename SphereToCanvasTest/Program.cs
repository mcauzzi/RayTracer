// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using Drawing;
using MainLib;

var floor = new Plane
{
    Material = new Material() { Color = new Color(1, 0.9, 0.9), Specular = 0 }
};
var ceiling = new Plane
{
    Material = new Material() { Color = new Color(0.8, 0.3, 0.4), Specular = 0 },
    Transformation = Transforms.GetTranslationMatrix(0, 11, 0)
};
var rightWall = new Plane
{
    Material = new Material() { Color = new Color(0.8, 0.3, 0.4), Specular = 0 },
    Transformation = Transforms.GetTranslationMatrix(5, 0, 0) * Transforms.RotateZ(Math.PI / 4) *
                     Transforms.RotateY(-Math.PI / 4)
};
var leftWall = new Plane
{
    Material = new Material() { Color = new Color(0.8, 0.3, 0.4), Specular = 0 },
    Transformation = Transforms.GetTranslationMatrix(-5, 0, 0) * Transforms.RotateZ(-Math.PI / 4) *
                     Transforms.RotateY(Math.PI / 4)
};
var middle = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(-0.5, 2, 0.5),
    Material = new Material()
    {
        Color = new Color(0.1, 1, 0.5),
        Diffuse = 0.7,
        Specular = 0.6
    }
};
var right = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(0.75, 2, -0.5) * Transforms.GetScalingMatrix(0.5, 0.5, 0.5),
    Material = new Material()
    {
        Color = new Color(0.5, 1, 0.1),
        Diffuse = 0.7,
        Specular = 0.6
    }
};
var left = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(1.5, 2, -2) * Transforms.GetScalingMatrix(0.33, 0.33, 0.33),
    Material = new Material()
    {
        Color = new Color(1, 0.8, 0.1),
        Diffuse = 0.7,
        Specular = 0.6
    }
};
var camera = new Camera(1920, 1080, Math.PI / 3)
{
    Transform = Transforms.ViewTransform(new Point(0, 8, -10), new Point(0, 1, 4), new Vector(0, 1, 0))
};
var pl = new PointLight(new Color(0.3, 0.3, 0.3), new Point(3, 6, -8));
var pl2 = new PointLight(new Color(1, 1, 1), new Point(0, 6, 4));
var world = new World(new List<PointLight>() { pl, pl2 },
    new List<Shape>() { floor, ceiling, rightWall, leftWall, right, middle, left });

var st = new Stopwatch();
st.Start();
var canvas = camera.Render(world);
st.Stop();
Console.WriteLine($"Total Render Time {st.Elapsed}");
new PPMCreator(canvas).WriteToFile("Sphere");
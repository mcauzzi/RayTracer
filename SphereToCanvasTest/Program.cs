// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using Drawing;
using MainLib;

var floor = new Sphere
{
    Transformation = Transforms.GetScalingMatrix(10, 0.01, 10),
    Material = new Material() { Color = new Color(1, 0.9, 0.9), Specular = 0 }
};
var leftWall = new Sphere
{
    Transformation = Transforms.GetTranslationMatrix(0, 0, 5) * Transforms.RotateY(-Math.PI / 4) *
                     Transforms.RotateX(Math.PI / 2) * Transforms.GetScalingMatrix(10, 0.01, 10),
    Material = floor.Material
};
var rightWall = new Sphere
{
    Transformation = Transforms.GetTranslationMatrix(0, 0, 5) * Transforms.RotateY(Math.PI / 4) *
                     Transforms.RotateX(Math.PI / 2) * Transforms.GetScalingMatrix(10, 0.01, 10),
    Material = floor.Material
};
var middle = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(-0.5, 1, 0.5),
    Material = new Material()
    {
        Color = new Color(0.1, 1, 0.5),
        Diffuse = 0.7,
        Specular = 0.3
    }
};
var right = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(1.5, 0.5, -0.5) * Transforms.GetScalingMatrix(0.5, 0.5, 0.5),
    Material = new Material()
    {
        Color = new Color(0.5, 1, 0.1),
        Diffuse = 0.7,
        Specular = 0.3
    }
};
var left = new Sphere()
{
    Transformation = Transforms.GetTranslationMatrix(-1.5, 0.33, -0.75) * Transforms.GetScalingMatrix(0.33, 0.33, 0.33),
    Material = new Material()
    {
        Color = new Color(1, 0.8, 0.1),
        Diffuse = 0.7,
        Specular = 0.3
    }
};
var camera = new Camera(1280, 720, Math.PI / 3);
camera.Transform = Transforms.ViewTransform(new Point(0, 1.5, -5), new Point(0, 1, 0), new Vector(0, 1, 0));
var pl = new PointLight(new Color(1, 1, 1), new Point(-10, 10, -10));
var pl2 = new PointLight(new Color(1, 1, 1), new Point(8, 10, 8));
var world = new World(new List<PointLight>() { pl },
    new List<Sphere>() { floor, leftWall, rightWall, right, middle, left });

var st = new Stopwatch();
st.Start();
var canvas = camera.Render(world);
st.Stop();
Console.WriteLine($"Total Render Time{st.Elapsed}");
new PPMCreator(canvas).WriteToFile("Sphere");
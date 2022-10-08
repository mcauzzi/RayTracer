// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using Drawing;
using Drawing.Patterns;
using MainLib;

var backGroundMaterial = new Material
{
    Color = new Color(0.5, 0.5, 0.5), Specular = 0.3, Ambient = 0.1, Diffuse = 0.3, Reflective = 0.3,
    RefractiveIndex = 1, Shininess = 200,
    Pattern = new CheckerPattern(Color.Red, Color.White) { Transformation = Transforms.GetScalingMatrix(4, 4, 4) }
};
var backGround = new Plane
{
    Material       = backGroundMaterial,
    Transformation = Transforms.GetTranslationMatrix(0, -10, 0)
};
var ceiling = new Plane
{
    Material = new Material()
        { Color = new Color(0.8, 0.8, 0.8), Specular = 0.3, Ambient = 0.1, Diffuse = 0.3, Reflective = 0.3 },
    Transformation = Transforms.GetTranslationMatrix(0, 0, 0)
};
var backGround2 = new Plane
{
    Material       = backGroundMaterial,
    Transformation = Transforms.GetTranslationMatrix(-9.1, 0, 0) * Transforms.RotateZ(Math.PI / 2)
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
var sphereList = new List<Sphere>();
var rnd        = new Random();
var cubeLength = 5;
var startCubex = -18;
var startCubeZ = 13;
for (int i = 0; i < cubeLength; i++)
{
    for (int j = 0; j < cubeLength; j++)
    {
        for (int k = 0; k < cubeLength; k++)
        {
            var redColor   = i / ((double)cubeLength - 1);
            var greenColor = j / ((double)cubeLength - 1);
            var blueColor  = k / ((double)cubeLength - 1);


            sphereList.Add(new Sphere
            {
                Transformation =
                    Transforms.GetTranslationMatrix(startCubex + i * 2.5, -9 + j * 2.5, startCubeZ - k * 2.5),
                Material = new Material
                {
                    Color           = new Color(redColor, greenColor, blueColor),
                    Ambient         = 0.1, Diffuse   = 0.8, Specular = 0.5, Reflective = 0, Transparency = 1,
                    RefractiveIndex = 1.2, Shininess = 300,
                    // Pattern = new CheckerPattern(new Color(redColor, greenColor, blueColor),new Color(1-redColor, 1-greenColor, 1-blueColor))
                    // {
                    //     Transformation = Transforms.GetScalingMatrix(2,2,2)
                    // }
                }
            });
            sphereList.Add(new Sphere
            {
                Transformation =
                    Transforms.GetTranslationMatrix(startCubex + i * 2.5, -9 + j * 2.5, startCubeZ - k * 2.5) *
                    Transforms.GetScalingMatrix(0.33, 0.33, 0.33),
                Material = new Material
                {
                    Color           = Color.Red,
                    Ambient         = 0.1, Diffuse = 0.8, Specular = 0.3, Reflective = 0, Transparency = 0.0,
                    RefractiveIndex = 0, Shininess = 100
                }
            });
            // sphereList.Add(new Sphere
            // {
            //     Transformation = Transforms.GetTranslationMatrix(-13 + i * 3, -9 + j * 3, 8 - k * 4)*Transforms.GetScalingMatrix(0.5,0.5,0.5),
            //     Material = new Material
            //     {
            //         Color   = new Color(1-(i / (double)3), 1-(j / (double)3),1- (k / (double)3)),
            //         Ambient = 0.3, Diffuse = 0.1, Specular = 0.8, Reflective = 0,
            //         // Pattern = new RingPattern(new Color(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()),
            //         //         new Color(rnd.NextDouble(),                   rnd.NextDouble(), rnd.NextDouble()))
            //         //     { Transformation = Transforms.GetScalingMatrix(2,1,1) }
            //     }
            // });
        }
    }
}

// sphereList.Add(new Sphere
// {
//     Transformation = Transforms.GetTranslationMatrix(-6, -6, 2)*Transforms.GetScalingMatrix(8,8,8),
//     Material = new Material
//     {
//         Color   = new Color(0.9,0,0),
//         Ambient = 0.03 , Diffuse = 0.2, Specular = 1, Reflective = 1,Transparency = 0.9,RefractiveIndex = 1.1,Shininess = 300
//     }
// });
var camera = new Camera(1280, 720, Math.PI / 2)
{
    Transform = Transforms.ViewTransform(
        MathTuple.GetPoint(startCubex - 2,                4,   startCubeZ + 2),
        MathTuple.GetPoint(startCubex + cubeLength * 2.5, -10, startCubeZ - cubeLength * 2.5),
        MathTuple.GetPoint(0,                             1,   0)
    ),
};
var pl2    = new PointLight(Color.White, MathTuple.GetPoint(7,   -3, 8));
var pl     = new PointLight(Color.White, MathTuple.GetPoint(4.5, 0,  -9.5));
var shapes = new List<Shape> { };
shapes.AddRange(sphereList);
var world = new World(new List<PointLight> { pl },
    shapes);

var st = new Stopwatch();

st.Start();

var canvas = camera.RenderWithTasks(world);
st.Stop();
Console.WriteLine(
    $"Total Render Time {st.Elapsed}, Pixel/s:{canvas.Height * canvas.Width / (double)st.ElapsedMilliseconds * 1000}");
new PPMCreator(canvas).WriteToFile("Sphere");
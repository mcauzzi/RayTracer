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
    Transformation = Transforms.GetTranslationMatrix(0, 0, 10) * Transforms.RotateX(Math.PI / 2)
};
var backGround2 = new Plane
{
    Material       = backGroundMaterial,
    Transformation = Transforms.GetTranslationMatrix(-9.1, 0, 0) * Transforms.RotateZ(Math.PI / 2)
};
var backGround3 = new Plane
{
    Material       = backGroundMaterial,
    Transformation = Transforms.GetTranslationMatrix(9.1, 0, 0) * Transforms.RotateZ(Math.PI / 2)
};
var sphereList = new List<Sphere>();
var rnd        = new Random();
var cubeLength = 3;
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
                Transformation = Transforms.GetTranslationMatrix(-9 + i * 2.5, -9 + j * 2.5, 4 - k * 2.5),
                Material = new Material
                {
                    Color           = new Color(redColor, greenColor, blueColor),
                    Ambient         = 0.2, Diffuse = 0.5, Specular = 0.2, Reflective = 0.1, Transparency = 0.6,
                    RefractiveIndex = 1, Shininess = 28,
                    // Pattern = new CheckerPattern(new Color(redColor, greenColor, blueColor),new Color(1-redColor, 1-greenColor, 1-blueColor))
                    // {
                    //     Transformation = Transforms.GetScalingMatrix(2,2,2)
                    // }
                }
            });
            // sphereList.Add(new Sphere
            // {
            //     Transformation = Transforms.GetTranslationMatrix(-9 + i * 3, -9 + j * 3, 4 - k * 3)*Transforms.GetScalingMatrix(0.33,0.33,0.33),
            //     Material = new Material
            //     {
            //         Color   = new Color(1-redColor, 1-greenColor, 1-blueColor),
            //         Ambient = 0.2, Diffuse = 0.1, Specular = 0.3, Reflective = 0.0,Transparency = 0.0,RefractiveIndex = 0,Shininess = 28
            //     }
            // });
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
var camera = new Camera(800, 600, Math.PI / 2)
{
    Transform = Transforms.ViewTransform(MathTuple.GetPoint(0.5, -6.5, -5.5), MathTuple.GetPoint(-10, -6.5, +5),
        MathTuple.GetVector(0, 1, 0))
};
var pl2    = new PointLight(Color.White, MathTuple.GetPoint(7,   -3, 8));
var pl     = new PointLight(Color.White, MathTuple.GetPoint(4.5, 0,  -9.5));
var shapes = new List<Shape> { backGround };
shapes.AddRange(sphereList);
var world = new World(new List<PointLight> { pl },
    shapes);

var st = new Stopwatch();

st.Start();

var canvas = camera.RenderWithTasks(world);
st.Stop();
Console.WriteLine(
    $"Total Render Time {st.Elapsed}, Pixel/s:{((canvas.Height * canvas.Width) / (double)st.ElapsedMilliseconds) * 1000}");
new PPMCreator(canvas).WriteToFile("Sphere");
// See https://aka.ms/new-console-template for more information


using Drawing;
using MainLib;

var c = new Canvas(900, 900);
var startingPoint = new Point(0, 0, -10);
var wallZ = 10;
var wallSize = 7;
var pixelSize = wallSize / (double)c.Height;
var half = wallSize / (double)2;

var s = new Sphere
{
    Material =
    {
        Color = new Color(1, 0.2, 1)
    }
};
var pl = new PointLight(new Color(1, 1, 1), new Point(-10, 10, -10));
for (int i = 0; i < c.Height; i++)
{
    var worldY = half - pixelSize * i;
    for (int j = 0; j < c.Width; j++)
    {
        var worldX = -half + pixelSize * j;
        var wallTarget = new Point(worldX, worldY, wallZ);
        var direction = (wallTarget - startingPoint).Normalize();
        var ray = new Ray(startingPoint, new Vector(direction.X, direction.Y, direction.Z).Normalize());
        //s.Transformation = Transforms.GetScalingMatrix(1.5, 1.5, 1.5);
        var raySphere = ray.Intersects(s);
        var hit = Intersection.Hit(raySphere);
        if (hit != null)
        {
            var point = ray.Position(hit.Distance);
            var normal = (hit.Obj as Sphere).Normal(point);
            var eye = ray.Direction;
            var color = s.Material.GetLighting(pl, point, eye, normal);
            c.WritePixel(color, j, i);
        }
    }
}

new PPMCreator(c).WriteToFile("Sphere");
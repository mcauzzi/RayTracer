﻿// See https://aka.ms/new-console-template for more information


using MainLib;

var c = new Canvas(900, 900);
var startingPoint = new Point(0, 0, -10);
var wallZ = 10;
var wallSize = 7;
var pixelSize = wallSize / (double)c.Height;
var half = wallSize / (double)2;

var color = new Color(1, 0, 0);
var s = new Sphere();
for (int i = 0; i < c.Height; i++)
{
    var worldY = half - pixelSize * i;
    for (int j = 0; j < c.Width; j++)
    {
        var worldX = -half + pixelSize * j;
        var wallTarget = new Point(worldX, worldY, wallZ);
        var direction = (wallTarget - startingPoint).Normalize();
        var ray = new Ray(startingPoint, new Vector(direction.X, direction.Y, direction.Z));
        s.Transformation = Transforms.GetScalingMatrix(1.5, 1.5, 1.5);
        var raySphere = ray.Intersects(s);
        if (Intersection.Hit(raySphere) != null)
        {
            c.WritePixel(color, j, i);
        }
    }
}

new PPMCreator(c).WriteToFile("Sphere");
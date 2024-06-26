﻿using Drawing.Shapes;
using MainLib;

namespace Drawing;

public class World
{
    private World()
    {
        Lights = new List<PointLight>();
        Shapes = new List<Shape>();
        Lights.Add(new PointLight(Color.White, MathTuple.GetPoint(-10, 10, -10)));
        var s1 = new Sphere
        {
            Material = new Material() { Color = new Color(0.8, 1.0, 0.6), Diffuse = 0.7, Specular = 0.2 }
        };
        Shapes.Add(s1);
        var s2 = new Sphere
        {
            Transformation = Transforms.GetScalingMatrix(0.5, 0.5, 0.5)
        };
        Shapes.Add(s2);
    }

    public World(List<PointLight> lights, List<Shape> shapes)
    {
        Lights = lights;
        Shapes = shapes;
    }

    public List<PointLight> Lights { get; }
    public List<Shape>      Shapes { get; set; }

    public static World GetDefaultWorld() => new();

    public Intersection[] Intersect(Ray ray)
    {
        var intersectionsPershape = new Intersection[Shapes.Count][];
        var totalIntersections    = 0;
        for (var index = 0; index < Shapes.Count; index++)
        {
            var shapesIntersections = Shapes[index].Intersect(ray);
            intersectionsPershape[index] =  shapesIntersections;
            totalIntersections           += shapesIntersections.Length;
        }

        var res = new Intersection[totalIntersections];
        for (int i = 0; i < Shapes.Count; i++)
        {
            for (int j = 0; j < intersectionsPershape[i].Length; j++)
            {
                res[totalIntersections - 1] = intersectionsPershape[i][j];
                totalIntersections--;
            }
        }

        Array.Sort(res, Intersection.DistanceComparer);
        return res;
    }

    public Color ShadeHit(Computation comps, int remaining = 5)
    {
        var color   = Color.Black;
        var surface = color;
        for (var i = 0; i < Lights.Count; i++)
        {
            surface += comps.Obj.Material.GetLighting(Lights[i], comps.Obj, comps.OverPoint, comps.EyeV,
                comps.NormalV, IsShadowed(comps.OverPoint, Lights[i]));
        }

        var reflected = ReflectedColor(comps, remaining);
        var refracted = RefractedColor(comps, remaining);
        var mat       = comps.Obj.Material;
        if (mat.Reflective > 0 && mat.Transparency > 0)
        {
            var reflectance = comps.Schlick();
            return surface + reflected * reflectance + refracted * (1 - reflectance);
        }

        var res = surface + reflected + refracted;
        return res;
    }

    public Color ColorAt(Ray r, int remaining = 5)
    {
        var inter = Intersect(r);
        var hits  = Intersection.Hit(inter);
        if (inter.Length == 0 || hits == null)
        {
            return Color.Black;
        }
        else
        {
            var comps = hits.PrepareComputation(r, inter);
            return ShadeHit(comps, remaining);
        }
    }

    public bool IsShadowed(MathTuple point, PointLight? light = null)
    {
        if (light == null)
        {
            light = Lights[0];
        }

        var v            = light.Position - point;
        var distance     = v.GetMagnitude();
        var direction    = v.Normalize();
        var r            = new Ray(point, direction);
        var intersection = Intersect(r);
        var h            = Intersection.Hit(intersection);
        if (h is not null && (h.Distance < distance))
        {
            return true;
        }

        return false;
    }

    public Color ReflectedColor(Computation comps, int remaining = 5)
    {
        if (comps.Obj.Material.Reflective == 0 || remaining == 0)
        {
            return Color.Black;
        }

        var reflectRay = new Ray(comps.OverPoint, comps.ReflectV);
        return ColorAt(reflectRay, remaining - 1) * comps.Obj.Material.Reflective;
    }

    public Color RefractedColor(Computation comps, int remaining)
    {
        var nRatio = comps.N1 / comps.N2;
        var cosI   = MathTuple.DotProduct(comps.EyeV, comps.NormalV);
        var sin2T  = Math.Pow(nRatio, 2) * (1 - Math.Pow(cosI, 2));
        if (comps.Obj.Material.Transparency == 0 || remaining == 0 || sin2T > 1)
        {
            return Color.Black;
        }

        var cosT       = Math.Sqrt(1 - sin2T);
        var direction  = comps.NormalV * (nRatio * cosI - cosT) - comps.EyeV * nRatio;
        var refractRay = new Ray(comps.UnderPoint, direction);
        return ColorAt(refractRay, remaining - 1) * comps.Obj.Material.Transparency;
    }
}
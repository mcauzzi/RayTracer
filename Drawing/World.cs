using MainLib;

namespace Drawing;

public class World
{
    public World()
    {
        Lights = new List<PointLight>();
        Spheres = new List<Sphere>();
        Lights.Add(new PointLight(new Color(1, 1, 1), new Point(-10, 10, -10)));
        var s1 = new Sphere
        {
            Material = new Material() { Color = new Color(0.8, 1.0, 0.6), Diffuse = 0.7, Specular = 0.2 }
        };
        Spheres.Add(s1);
        var s2 = new Sphere
        {
            Transformation = Transforms.GetScalingMatrix(0.5, 0.5, 0.5)
        };
        Spheres.Add(s2);
    }

    public List<PointLight> Lights { get; }
    public List<Sphere> Spheres { get; }

    public List<Intersection> Intersect(Ray ray)
    {
        var intersection = new List<Intersection>();

        foreach (var sphere in Spheres)
        {
            intersection.AddRange(ray.Intersects(sphere));
        }

        return intersection.OrderBy(x => x.Distance).ToList();
    }

    public Color ShadeHit(Computation comps)
    {
        var s = comps.Obj as Sphere;
        var color = new Color(0, 0, 0);
        return Lights.Aggregate(color,
            (current, _) => current + s.Material.GetLighting(Lights[0], comps.Point, comps.EyeV, comps.NormalV));
    }

    public Color ColorAt(Ray r)
    {
        var inter = Intersect(r);
        var hits = Intersection.Hit(inter);
        if (hits == null)
        {
            return new Color(0, 0, 0);
        }
        else
        {
            var comps = hits.PrepareComputation(r);
            return ShadeHit(comps);
        }
    }
}
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

    public World(List<PointLight> lights, List<Sphere> spheres)
    {
        Lights = lights;
        Spheres = spheres;
    }

    public List<PointLight> Lights { get; }
    public List<Sphere> Spheres { get; set; }

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
            (total, light) => total + s.Material.GetLighting(light, comps.OverPoint, comps.EyeV, comps.NormalV,
                IsShadowed(comps.OverPoint, light)));
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

    public bool IsShadowed(MathTuple point, PointLight? light = null)
    {
        if (light == null)
        {
            light = Lights[0];
        }

        var v = light.Position - point;
        var distance = v.GetMagnitude();
        var direction = v.Normalize();
        var r = new Ray(point, direction);
        var intersection = Intersect(r);
        var h = Intersection.Hit(intersection);
        if (h is not null && (h.Distance < distance))
        {
            return true;
        }

        return false;
    }
}
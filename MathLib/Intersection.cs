using System.Collections.Generic;
using System.Linq;

namespace MainLib;

public class Intersection
{
    public Intersection(object obj, double distance)
    {
        Obj = obj;
        Distance = distance;
    }

    public double Distance { get; }
    public object Obj { get; }

    /// <summary>
    /// Controlla le intersezioni con una sfera
    /// </summary>
    /// <param name="s">La sfera con cui verficare le intersezioni</param>
    /// <returns>Le distanze in cui il raggio interseca la sfera</returns>
    public static Intersection Hit(List<Intersection> xs)
    {
        return xs.OrderBy(x => x.Distance).FirstOrDefault(x => x.Distance > 0);
    }
}
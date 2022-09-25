namespace MainLib;

public class PointLight
{
    public PointLight(Color intensity, Point position)
    {
        Intensity = intensity;
        Position = position;
    }

    public Color Intensity { get; }
    public Point Position { get; }
}
namespace Drawing;

public class Canvas
{
    public Canvas(int width, int height)
    {
        Matrix = new Color[height][];
        for (var i = 0; i < Matrix.Length; i++)
        {
            Matrix[i] = new Color[width];
            for (int j = 0;
                 j < Matrix[i]
                     .Length;
                 j++)
            {
                Matrix[i][j] = Color.Black;
            }
        }
    }

    public int Width => Matrix[0]
        .Length;

    public int Height => Matrix.Length;

    private Color[][] Matrix { get; }

    public void WritePixel(Color c, int x, int y)
    {
        if (y > Matrix.Length || y < 0)
        {
            return;
        }

        if (x > Matrix[y]
                .Length || x < 0)
        {
            return;
        }

        Matrix[y][x] = c;
    }

    public Color PixelAt(int x, int y)
    {
        if (y > Matrix.Length || y < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        if (x > Matrix[y]
                .Length || x < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }

        return Matrix[y][x];
    }
}
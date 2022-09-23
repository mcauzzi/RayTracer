using MainLib;
using Xunit;

namespace MainLibTests;

public class TransformationTests
{
    [Fact]
    public void Translation()
    {
        var trans = Transforms.GetTranslationMatrix(5, -3, 2);
        Assert.Equal(trans * new Point(-3, 4, 5), new Point(2, 1, 7));
    }

    [Fact]
    public void InvertedTranslation()
    {
        var trans = Transforms.GetTranslationMatrix(5, -3, 2).GetInverse();
        Assert.Equal(trans * new Point(-3, 4, 5), new Point(-8, 7, 3));
    }

    [Fact]
    public void Scaling()
    {
        Point tempQualifier = new Point(2, 3, 4);
        var trans = Transforms.GetScalingMatrix(2, 3, 4);
        Assert.Equal(trans * new Point(-4, 6, 8), new Point(-8, 18, 32));
    }

    [Fact]
    public void ScalingVector()
    {
        var trans = Transforms.GetScalingMatrix(2, 3, 4);
        Assert.Equal(trans * new Vector(-4, 6, 8), new Vector(-8, 18, 32));
    }

    [Fact]
    public void InvertedScaling()
    {
        var trans = Transforms.GetScalingMatrix(2, 3, 4).GetInverse();
        Assert.Equal(trans * new Point(-4, 6, 8), new Point(-2, 2, 2));
    }

    [Fact]
    public void Reflection()
    {
        Point tempQualifier = new Point(-1, 1, 1);
        var trans = Transforms.GetScalingMatrix(-1, 1, 1).GetInverse();
        Assert.Equal(trans * new Point(2, 3, 4), new Point(-2, 3, 4));
    }
}
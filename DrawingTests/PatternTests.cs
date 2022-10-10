using Drawing.Patterns;
using Drawing.Shapes;
using MainLib;

namespace DrawingTests;

public class PatternTests
{
    [Fact]
    public void Creation()
    {
        var a = new StripePattern(Color.White, Color.Black);
        Assert.Equal(Color.White, a.FirstColor);
        Assert.Equal(Color.Black, a.SecondColor);
    }

    [Fact]
    public void StripeShouldBeConstantInY()
    {
        var a = new StripePattern(Color.White, Color.Black);
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0, 0, 0)));
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0, 1, 0)));
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0, 2, 0)));
    }

    [Fact]
    public void StripeShouldBeConstantInZ()
    {
        var a = new StripePattern(Color.White, Color.Black);
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0, 0, 0)));
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0, 0, 1)));
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0, 0, 2)));
    }

    [Fact]
    public void StripeShouldNotBeConstantInX()
    {
        var a = new StripePattern(Color.White, Color.Black);
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0,    0, 0)));
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(0.9,  0, 0)));
        Assert.Equal(Color.Black, a.ColorAt(MathTuple.GetPoint(1,    0, 0)));
        Assert.Equal(Color.Black, a.ColorAt(MathTuple.GetPoint(-0.1, 0, 0)));
        Assert.Equal(Color.Black, a.ColorAt(MathTuple.GetPoint(-1,   0, 0)));
        Assert.Equal(Color.White, a.ColorAt(MathTuple.GetPoint(-1.1, 0, 0)));
    }

    [Fact]
    public void StripesWithObjectTransformation()
    {
        var s = new Sphere();
        s.Transformation = Transforms.GetScalingMatrix(2, 2, 2);
        var pattern = new StripePattern(Color.White, Color.Black);
        Assert.Equal(Color.White, pattern.ColorAtObject(s, MathTuple.GetPoint(1.5, 0, 0)));
    }

    [Fact]
    public void StripesWithPatternTransformation()
    {
        var s       = new Sphere();
        var pattern = new StripePattern(Color.White, Color.Black);
        pattern.Transformation = Transforms.GetScalingMatrix(2, 2, 2);
        Assert.Equal(Color.White, pattern.ColorAtObject(s, MathTuple.GetPoint(1.5, 0, 0)));
    }

    [Fact]
    public void StripesWithPatternAndObjectTransformation()
    {
        var s = new Sphere();
        s.Transformation = Transforms.GetScalingMatrix(2, 2, 2);
        var pattern = new StripePattern(Color.White, Color.Black);
        pattern.Transformation = Transforms.GetTranslationMatrix(0.5, 0, 0);
        Assert.Equal(Color.White, pattern.ColorAtObject(s, MathTuple.GetPoint(2.5, 0, 0)));
    }

    [Fact]
    public void Gradient()
    {
        var p = new GradientPattern(Color.White, Color.Black);
        Assert.Equal(Color.White,                 p.ColorAt(MathTuple.GetPoint(0,    0, 0)));
        Assert.Equal(new Color(0.75, 0.75, 0.75), p.ColorAt(MathTuple.GetPoint(0.25, 0, 0)));
        Assert.Equal(new Color(0.5,  0.5,  0.5),  p.ColorAt(MathTuple.GetPoint(0.5,  0, 0)));
        Assert.Equal(new Color(0.25, 0.25, 0.25), p.ColorAt(MathTuple.GetPoint(0.75, 0, 0)));
    }

    [Fact]
    public void Ring()
    {
        var p = new RingPattern(Color.White, Color.Black);
        Assert.Equal(Color.White, p.ColorAt(MathTuple.GetPoint(0,     0, 0)));
        Assert.Equal(Color.Black, p.ColorAt(MathTuple.GetPoint(1,     0, 0)));
        Assert.Equal(Color.Black, p.ColorAt(MathTuple.GetPoint(0,     0, 1)));
        Assert.Equal(Color.Black, p.ColorAt(MathTuple.GetPoint(0.708, 0, 0.708)));
    }

    [Fact]
    public void CheckerRepeatInX()
    {
        var p = new CheckerPattern(Color.White, Color.Black);
        Assert.Equal(Color.White, p.ColorAt(MathTuple.GetPoint(0,    0, 0)));
        Assert.Equal(Color.White, p.ColorAt(MathTuple.GetPoint(0.99, 0, 0)));
        Assert.Equal(Color.Black, p.ColorAt(MathTuple.GetPoint(1.01, 0, 0)));
    }

    [Fact]
    public void CheckerRepeatInY()
    {
        var p = new CheckerPattern(Color.White, Color.Black);
        Assert.Equal(Color.White, p.ColorAt(MathTuple.GetPoint(0, 0,    0)));
        Assert.Equal(Color.White, p.ColorAt(MathTuple.GetPoint(0, 0.99, 0)));
        Assert.Equal(Color.Black, p.ColorAt(MathTuple.GetPoint(0, 1.01, 0)));
    }

    [Fact]
    public void CheckerRepeatInZ()
    {
        var p = new CheckerPattern(Color.White, Color.Black);
        Assert.Equal(Color.White, p.ColorAt(MathTuple.GetPoint(0, 0, 0)));
        Assert.Equal(Color.White, p.ColorAt(MathTuple.GetPoint(0, 0, 0.99)));
        Assert.Equal(Color.Black, p.ColorAt(MathTuple.GetPoint(0, 0, 1.01)));
    }
}
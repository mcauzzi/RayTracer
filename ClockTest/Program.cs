// See https://aka.ms/new-console-template for more information


using Drawing;
using FileManagement;
using MainLib;

var canvas = new Canvas(500, 500);
var red = new Color(1, 0, 0);
var startingPoint = MathTuple.GetPoint(0, 230, 0);
for (int i = 0; i < 12; i++)
{
    var rotation = Transforms.RotateZ(i * (Math.PI / 6));
    var translation = Transforms.GetTranslationMatrix(canvas.Width / 2, canvas.Height / 2, 0);
    var hour = (translation * rotation) * startingPoint;

    canvas.WritePixel(red, (int)Math.Round(hour.X), (int)Math.Round(hour.Y));
    ;
}

canvas.WritePixel(red, 0, 0);
canvas.WritePixel(red, 0, 0);
new PPMCreator(canvas).WriteToFile("Clock");
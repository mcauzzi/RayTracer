using System;
using MainLib;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var proj = new Projectile(MathTuple.GetPoint(0, 1, 0), MathTuple.GetVector(1, 1.8, 0).Normalize() * 11.25);
            var env = new Environment(MathTuple.GetVector(0, -0.1, 0), MathTuple.GetVector(-0.01, 0, 0));
            var canvas = new Canvas(900, 550);
            for (int i = 0; proj.Position.Y > 0; i++)
            {
                proj = Tick(env, proj);
                Console.WriteLine($"{i}:{proj.Position.ToString()}");
                canvas.WritePixel(new Color(1, 0, 0), (int)Math.Round(proj.Position.X),
                    (int)Math.Round(canvas.Height - proj.Position.Y));
            }

            new PPMCreator(canvas).WriteToFile();
        }

        private static Projectile Tick(Environment env, Projectile proj)
        {
            return new Projectile(proj.Position + proj.Velocity, proj.Velocity + env.Gravity + env.Wind);
        }
    }

    internal class Projectile
    {
        public Projectile(MathTuple pos, MathTuple velocity)
        {
            Position = pos;
            Velocity = velocity;
        }

        public MathTuple Position { get; set; }
        public MathTuple Velocity { get; set; }
    }

    internal class Environment
    {
        public Environment(MathTuple grav, MathTuple wind)
        {
            Gravity = grav;
            Wind = wind;
        }

        public MathTuple Gravity { get; set; }
        public MathTuple Wind { get; set; }
    }
}
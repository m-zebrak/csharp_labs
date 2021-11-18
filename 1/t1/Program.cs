using System;

namespace t1
{
    public abstract class Triangle
    {
        protected float a { get; set; }
        protected float b { get; set; }
        protected float c { get; set; }

        public double area()
        {
            var p = perimeter() / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }

        public float perimeter() =>
            a + b + c;


        public override string ToString() =>
            $"{this.GetType().Name} a={a:f2}  b={b:f2}  c={c:f2}";
    }

    public class Equilateral : Triangle
    {
        public Equilateral(float a) =>
            this.a = b = c = a;
    }

    public class Isosceles : Triangle
    {
        public Isosceles(float a, float b)
        {
            this.a = c = a;
            this.b = b;
        }
    }

    public class Rectangular : Triangle
    {
        public Rectangular(float a, float b)
        {
            this.a = a;
            this.b = b;
            c = (float) Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var triangles = new Triangle[3];
            triangles[0] = new Equilateral(3);
            triangles[1] = new Isosceles(10, 2);
            triangles[2] = new Rectangular(3, 4);

            foreach (Triangle t in triangles)
            {
                Console.WriteLine(t.ToString());
                Console.WriteLine($"Perimeter: {t.perimeter():f2}  Area: {t.area():f2}\n");
            }
        }
    }
}
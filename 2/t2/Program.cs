using System;
using System.Collections.Generic;
using System.Linq;

namespace t2
{
    public class CompareList<T> where T : IComparable<T>
    {
        private readonly List<T> _list;

        public CompareList(List<T> list)
        {
            _list = list;
            var type = typeof(T);

            if (type != typeof(string)) return;

            foreach (var str in list.Where(str => !double.TryParse((string) (object) str, out _)))
                throw new ArgumentException($"{(string) (object) str} is not a double");
        }

        public static bool operator <(CompareList<T> x, CompareList<T> y) =>
            x._list.Sum(val => Convert.ToDouble(val)) < y._list.Sum(val => Convert.ToDouble(val));


        public static bool operator >(CompareList<T> x, CompareList<T> y) =>
            x._list.Sum(val => Convert.ToDouble(val)) > y._list.Sum(val => Convert.ToDouble(val));
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var a = new CompareList<string>(new List<string> {"1", "2,0", "3,0", "4"});
            var b = new CompareList<string>(new List<string> {"20", "30"});
            Console.WriteLine(a < b);

            var c = new CompareList<int>(new List<int> {1, 2, 3, 4});
            var d = new CompareList<int>(new List<int> {30, 40});
            Console.WriteLine(c < d);
        }
    }
}
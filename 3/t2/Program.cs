using System;
using System.Collections.Generic;

namespace t2
{
    public class SortedList
    {
        private readonly List<string> _list = new();
        private ISortStrategy _sortStrategy;

        public SortedList()
        {
        }

        public SortedList(ISortStrategy sortStrategy)
        {
            _sortStrategy = sortStrategy;
        }

        public void SetStrategy(ISortStrategy sortStrategy) =>
            _sortStrategy = sortStrategy;

        public void Add(string name) =>
            _list.Add(name);


        public void Sort()
        {
            _sortStrategy.Sort(_list);
            _list.ForEach(name => Console.Write($"{name}, "));
            Console.WriteLine();
        }

        public List<string> GetList() => _list;
    }

    public interface ISortStrategy
    {
        public void Sort(List<string> list);
    }


    public class QuickSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            Console.WriteLine("QuickSort");
            list.Sort(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class BubbleSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            Console.WriteLine("BubbleSort");
            bool swap;

            do
            {
                swap = false;
                for (var index = 0; index < (list.Count - 1); index++)
                {
                    if (string.Compare(list[index], list[index + 1], StringComparison.OrdinalIgnoreCase) <= 0) continue;
                    (list[index], list[index + 1]) = (list[index + 1], list[index]);
                    swap = true;
                }
            } while (swap);
        }
    }

    public class InsertSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            Console.WriteLine("InsertSort");

            for (var i = 1; i < list.Count; i++)
            {
                var value = list[i];
                var j = i - 1;
                while ((j >= 0) && (string.Compare(list[j], value, StringComparison.OrdinalIgnoreCase) > 0))
                {
                    list[j + 1] = list[j];
                    j--;
                }

                list[j + 1] = value;
            }
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var original = new SortedList();
            original.Add("Samual");
            original.Add("Jimmy");
            original.Add("Sandra");
            original.Add("Vivek");
            original.Add("Anna");

            var copy = original;
            copy.SetStrategy(new QuickSort());
            copy.Sort();

            copy = original;
            copy.SetStrategy(new BubbleSort());
            copy.Sort();

            copy = original;
            copy.SetStrategy(new InsertSort());
            copy.Sort();
        }
    }
}
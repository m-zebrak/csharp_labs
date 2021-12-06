using System;
using System.Collections.Generic;
using System.Linq;


namespace t1
{
    public class SortedList
    {
        private readonly List<int> _list = new();
        private IStrategy _strategy;

        public SortedList()
        {
        }

        public SortedList(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IStrategy strategy) =>
            _strategy = strategy;

        public void Add(int number) =>
            _list.Add(number);


        public void Sort()
        {
            var result = _strategy.DoAlgorithm(_list);
            result.ForEach(i => Console.Write($"{i}, "));
            Console.WriteLine();
        }

        public List<int> GetList() => _list;
    }

    public interface IStrategy
    {
        public List<int> DoAlgorithm(List<int> list);
    }


    public class QuickSort : IStrategy
    {
        public List<int> DoAlgorithm(List<int> list)
        {
            Console.WriteLine("QuickSort");
            return Do(list, 0, list.Count - 1);
        }


        private List<int> Do(List<int> list, int low, int high)
        {
            if (low >= high) return list;
            var pi = partition(list, low, high);
            Do(list, low, pi - 1);
            Do(list, pi + 1, high);
            return list;
        }

        private int partition(List<int> list, int low, int high)
        {
            var pivot = list[high];
            var i = low - 1;

            for (var j = low; j <= high - 1; j++)
            {
                if (list[j] < pivot)
                {
                    i++;
                    (list[i], list[j]) = (list[j], list[i]);
                }
            }

            (list[i + 1], list[high]) = (list[high], list[i + 1]);
            return i + 1;
        }
    }

    public class BubbleSort : IStrategy
    {
        public List<int> DoAlgorithm(List<int> list)
        {
            Console.WriteLine("BubbleSort");

            for (var i = 0; i < list.Count; i++)
            for (var j = i + 1; j < list.Count; j++)
            {
                if (list[i] <= list[j]) continue;
                (list[i], list[j]) = (list[j], list[i]);
            }

            return list;
        }
    }

    public class MergeSort : IStrategy
    {
        public List<int> DoAlgorithm(List<int> list)
        {
            Console.WriteLine("MergeSort");
            return new List<int>();
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var studentRecords = new SortedList();
            
            // 4, 952, 997, 181, 316, 870, 413, 682, 866, 147, 944, 180, 270, 594, 898, 896, 264, 900, 701, 388, 897, 918, 847, 306, 74, 455, 444, 696, 123, 976

            studentRecords.Add(1);
            studentRecords.Add(133);
            studentRecords.Add(23);
            studentRecords.Add(53);
            studentRecords.Add(0);

            studentRecords.SetStrategy(new QuickSort());
            studentRecords.Sort();

            studentRecords.SetStrategy(new BubbleSort());
            studentRecords.Sort();

            studentRecords.SetStrategy(new MergeSort());
            studentRecords.Sort();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace t3
{
    public class ObservableList<T>
    {
        public List<T> OldList = new();
        public List<T> List = new();
        private readonly List<Observer> _observers = new();


        public void Attach(Observer observer)
        {
            Console.WriteLine("Attached an observer.");
            _observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            _observers.Remove(observer);
            Console.WriteLine("Detached an observer.");
        }

        public void Notify() =>
            _observers.ForEach(o => o.Update(this));


        public void Add(T value)
        {
            List.Add(value);
            Notify();
            OldList.Add(value);
        }

        public void Remove(T value)
        {
            List.Remove(value);
            Notify();
            OldList.Remove(value);
        }

        public int Count() =>
            List.Count;

        public void Clear()
        {
            List.Clear();
            Notify();
            OldList.Clear();
        }

        public override string ToString() =>
            GetType().Name + "[" + string.Join(", ", List) + "]";
    }


    public class Observer
    {
        public void Update<T>(ObservableList<T> observable)
        {
            Console.WriteLine("State has just changed.");
            var actual = observable.List;
            var old = observable.OldList;
            if (actual.Count > old.Count)
            {
                var diff = actual.Except(old).ToList();
                Console.WriteLine($"Added: {string.Join(", ", diff)}\n");
            }
            else
            {
                var diff = old.Except(actual).ToList();
                Console.WriteLine($"Removed: {string.Join(", ", diff)}\n");
            }
        }
    }


    internal static class Program
    {
        private static void Main(string[] args)
        {
            var observable = new ObservableList<int>();
            var observer = new Observer();

            observable.Attach(observer);
            observable.Add(1);
            observable.Add(2);
            observable.Add(3);

            Console.WriteLine($"Show:\n{observable}\n");

            observable.Remove(3);
            observable.Clear();

            observable.Detach(observer);
            observable.Add(10);
        }
    }
}
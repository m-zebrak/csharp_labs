using System;
using System.Collections.Generic;
using System.Linq;

namespace t3
{
    public class Student
    {
        public List<Degree> Degrees;
        public int IndexNumber;
        public int Age;
        public char Sex;
        public int StudyYear;
        public int Semester;

        public Student(int indexNumber, int age, char sex, int studyYear, int semester)
        {
            IndexNumber = indexNumber;
            Age = age;
            Sex = sex;
            StudyYear = studyYear;
            Semester = semester;
            Degrees = new List<Degree>();
        }

        public override string ToString() =>
            $"{this.GetType().Name}(indexNumber={IndexNumber},  age={Age},  sex={Sex}, studyYear={StudyYear}, semester={Semester})";

        public void AddDegree(Degree degree) =>
            Degrees.Add(degree);

        public void ShowAllDegrees() =>
            Degrees.ForEach(degree => Console.WriteLine(degree.ToString()));
    }

    public class Degree
    {
        public string Subject;
        public int Rate;
        public int YearOfCredit;
        public int Semester;

        public Degree(string subject, int rate, int yearOfCredit, int semester)
        {
            Subject = subject;
            Rate = rate;
            YearOfCredit = yearOfCredit;
            Semester = semester;
        }

        public override string ToString() =>
            $"{this.GetType().Name}(subject={Subject},  rate={Rate},  yearOfCredit={YearOfCredit}, semester={Semester})";
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var students = new List<Student>
            {
                new(1239, 19, 'K', 2, 3),
                new(1234, 20, 'M', 3, 5),
                new(1235, 18, 'K', 3, 4),
                new(1236, 21, 'M', 3, 4),
                new(1237, 25, 'K', 3, 5),
                new(1240, 26, 'M', 1, 2),
                new(1241, 21, 'M', 1, 1),
                new(1238, 30, 'M', 2, 4)
            };

            students.ForEach(s => s.AddDegree(new Degree("Math", new Random().Next(2, 6), 2020, 2)));
            students.ForEach(s => s.AddDegree(new Degree("English", new Random().Next(2, 6), 2021, 3)));
            students.ForEach(s => s.AddDegree(new Degree("IT", new Random().Next(2, 6), 2021, 3)));

            Console.WriteLine("ALL STUDENTS:");
            students.ForEach(Console.WriteLine);


            var groups = students.GroupBy(s => s.StudyYear)
                .Select(g => new {StudyYear = g.Key, AvgAge = g.Average(s => s.Age)});

            Console.WriteLine($"\n{"StudyYear",10} {"AvgAge",10}");
            foreach (var g in groups)
                Console.WriteLine($"{g.StudyYear,10} {g.AvgAge,10}");

            Console.WriteLine($"\nSTUDENTS WITH AVGAGE HIGHER THAN AVERAGE AGE OF THEIR STUDY YEAR:");
            students.Where(s => s.Age > groups.Where(g => g.StudyYear == s.StudyYear).ElementAt(0).AvgAge)
                .ToList()
                .ForEach(Console.WriteLine);


            var groups2 = students.GroupBy(s => s.StudyYear)
                .Select(g => new {StudyYear = g.Key, AvgRate = g.ElementAt(0).Degrees.Average(d => d.Rate)});

            Console.WriteLine($"\n{"StudyYear",10} {"AvgRate",10}");
            foreach (var g in groups2)
                Console.WriteLine($"{g.StudyYear,10} {"",6}{g.AvgRate:F2}");

            Console.WriteLine($"\nSTUDENTS WITH AVGRATE HIGHER THAN AVERAGE RATE OF THEIR STUDY YEAR:");
            var better_students = students.Where(s =>
                    s.Degrees.Average(d => d.Rate) >
                    groups2.Where(g => g.StudyYear == s.StudyYear).ElementAt(0).AvgRate)
                .ToList();

            better_students.ForEach(s => Console.WriteLine($"{s}, AVERAGE RATE: {s.Degrees.Average(d => d.Rate):F2}"));
        }
    }
}
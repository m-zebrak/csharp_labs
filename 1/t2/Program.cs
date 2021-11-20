using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace t2
{
    public class EmployeeFile<T> where T : Employee
    {
        private List<T> _employeeList = new();

        public bool Validate(T employee) =>
            _employeeList.All(emp => !emp.IsMatch(employee));

        public void AddEmployee(T employee)
        {
            if (Validate(employee)) _employeeList.Add(employee);
        }

        public bool RemoveEmployee(T employee)
        {
            if (!_employeeList.Contains(employee)) return false;
            _employeeList.Remove(employee);
            return true;
        }

        public void ShowEmployees() =>
            _employeeList.ForEach(employee => Console.WriteLine(employee.Show()));

        public void Search(string firstName = null, string lastName = null, int age = 0, string jobPosition = null)
        {
            var matches = _employeeList;
            List<T> firstNames = new();
            List<T> lastNames = new();
            List<T> ages = new();
            List<T> jobPositions = new();

            foreach (var employee in _employeeList)
            {
                if (firstName is not null && employee.FirstName.Contains(firstName)) firstNames.Add(employee);
                if (lastName is not null && employee.LastName.Contains(lastName)) lastNames.Add(employee);
                if (age != 0 && employee.Age == age) ages.Add(employee);
                if (jobPosition is not null && employee.JobPosition.Contains(jobPosition)) jobPositions.Add(employee);
            }

            if (firstNames.Any()) matches = matches.Intersect(firstNames).ToList();
            if (lastNames.Any()) matches = matches.Intersect(lastNames).ToList();
            if (ages.Any()) matches = matches.Intersect(ages).ToList();
            if (jobPositions.Any()) matches = matches.Intersect(jobPositions).ToList();

            matches.ForEach(employee => Console.WriteLine(employee.Show()));
        }
    }

    public class Employee
    {
        private long _pesel;
        private string _firstName;
        private string _lastName;
        private int _age;
        private string _jobPosition;
        private readonly Regex _regex = new("^[a-z ,.'-]+$", RegexOptions.IgnoreCase);

        public long Pesel
        {
            get => _pesel;
            set
            {
                var length = value.ToString().Length;
                if (length == 11) _pesel = value;
                else throw new ArgumentException("Pesel must contains 11 digits");
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (!_regex.IsMatch(value)) throw new ArgumentException("First name is invalid!");
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (!_regex.IsMatch(value)) throw new ArgumentException("Last name is invalid!");
                _lastName = value;
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value is < 18 or > 67) throw new ArgumentException("Employee age must be between 18-67");
                _age = value;
            }
        }

        public string JobPosition
        {
            get => _jobPosition;
            set => _jobPosition = value;
        }

        public Employee(long pesel, string firstName, string lastName, int age, string jobPosition = "")
        {
            Pesel = pesel;
            Age = age;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            JobPosition = jobPosition;
        }

        public string Show() =>
            $"{GetType().Name}(pesel={Pesel}, firstName=\"{FirstName}\",  lastName=\"{LastName}\",  age={Age}, jobPosition=\"{JobPosition}\")";

        public bool IsMatch(Employee employee) =>
            Pesel == employee.Pesel;
    }


    internal static class Program
    {
        private static void Main(string[] args)
        {
            var file = new EmployeeFile<Employee>();
            var employees = new List<Employee>
            {
                new(99123456789, "Jan", "Kowalski", 19, "programmer"),
                new(99123456782, "Adam", "Nowak", 29),
                new(99123456781, "Adam", "Drugi", 19, "tester"),
                new(99123456780, "Rafal", "Super", 20),
                new(99123456781, "Rafal", "Super", 20)
            };

            employees.ForEach(employee => file.AddEmployee(employee));

            foreach (var employee in employees) file.AddEmployee(employee);

            Console.WriteLine($"Are emp2 and emp4 same by pesel?: {employees[2].IsMatch(employees[4])}");

            Console.WriteLine("\nEmployees of age 19 and containing 'a' in their first name");
            file.Search(firstName: "a", age: 19);

            Console.WriteLine("\nEmployees list:");
            file.ShowEmployees();

            Console.WriteLine($"\nDeleting emp0: {file.RemoveEmployee(employees[0])}");

            Console.WriteLine("\nEmployees list:");
            file.ShowEmployees();
        }
    }
}
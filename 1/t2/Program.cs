using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class EmployeeFile<T> where T : Employee
    {
        private List<T> _employeeList = new();

        public bool Validate(T employee) =>
            _employeeList.All(emp => emp.Pesel != employee.Pesel);

        public void AddEmployee(T employee)
        {
            if (Validate(employee))
                _employeeList.Add(employee);
        }

        public bool RemoveEmployee(T employee)
        {
            if (!_employeeList.Contains(employee)) return false;
            _employeeList.Remove(employee);
            return true;
        }

        public void ShowEmployees()
        {
            Console.WriteLine("\nEmployees list:");
            foreach (var employee in _employeeList)
                Console.WriteLine(employee.Show());
        }

        public void Search(string firstName = null, string lastName = null, int age = 0, string jobPosition = null)
        {
            HashSet<T> matches = new();
            foreach (var employee in _employeeList)
            {
                if (firstName is not null && employee.FirstName.Contains(firstName))
                    matches.Add(employee);

                if (lastName is not null && employee.LastName.Contains(lastName))
                    matches.Add(employee);

                if (age != 0 && employee.Age == age)
                    matches.Add(employee);

                if (jobPosition is not null && employee.JobPosition.Contains(jobPosition))
                    matches.Add(employee);
            }

            foreach (var employee in matches.ToList())
                Console.WriteLine(employee.Show());
        }
    }

    public class Employee
    {
        private long _pesel;
        private string _firstName;
        private string _lastName;
        private int _age;
        private string _jobPosition;

        public long Pesel
        {
            get => _pesel;
            set
            {
                var length = value.ToString().Length;
                if (length == 11)
                    _pesel = value;
                else
                    throw new ArgumentException("Pesel must contains 11 digits");
            }
        }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }


        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value is < 18 or > 67)
                    throw new ArgumentException("Employee age must be between 18-67");
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


        public bool IsMatch(Employee employee)
        {
            if (!Pesel.Equals(employee.Pesel))
                return false;
            if (!FirstName.Equals(employee.FirstName))
                return false;
            if (!LastName.Equals(employee.LastName))
                return false;
            if (_age != employee.Age)
                return false;
            return JobPosition.Equals(employee.JobPosition);
        }
    }


    internal static class Program
    {
        private static void Main(string[] args)
        {
            var file = new EmployeeFile<Employee>();
            var employees = new Employee[5];

            employees[0] = new Employee(99123456789, "Jan", "Kowalski", 19, "programmer");
            employees[1] = new Employee(99123456782, "Adam", "Nowak", 29);
            employees[2] = new Employee(99123456781, "Adam", "Drugi", 19, "tester");
            employees[3] = new Employee(99123456780, "Rafal", "Super", 20);
            employees[4] = new Employee(99123456780, "Rafal", "Super", 20);

            foreach (var employee in employees)
                file.AddEmployee(employee);

            Console.WriteLine($"Are emp3 and emp4 same?: {employees[3].IsMatch(employees[4])}");

            Console.WriteLine("\nEmployees of age 19 or containing 'a' in their first name");
            file.Search(firstName: "a", age: 19);

            Console.WriteLine($"\nDeleting emp0: {file.RemoveEmployee(employees[0])}");

            file.ShowEmployees();
        }
    }
}
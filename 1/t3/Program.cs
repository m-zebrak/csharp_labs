using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace t3
{
    public static class Globals
    {
        public const string Path = "C:\\Users\\Matheo\\Desktop\\";
    }

    public interface IGenericBuilder<T> where T : Employee
    {
        void BuildPrint(List<T> employees);
        void BuildSave(List<T> employees);
        List<T> BuildRead(string path);
    }

    public class TXTBuilder<T> : IGenericBuilder<T> where T : Employee
    {
        public void BuildPrint(List<T> employees) =>
            Console.WriteLine(string.Join("\n", employees.Select(emp => emp.Show())));

        public void BuildSave(List<T> employees) =>
            File.WriteAllLines($"{Globals.Path}\\{typeof(T).Name}s.txt", employees.Select(emp => emp.Show()).ToList());

        public List<T> BuildRead(string path)
        {
            List<T> employees = new();
            foreach (var emp in File.ReadAllLines(path + ".txt"))
            {
                var el = emp.Split(",").Select(s => s.Trim()).ToList();

                var pesel = long.Parse(el[0][(el[0].IndexOf("=") + 1)..]);
                var firstName = el[1][(el[1].IndexOf("\"") + 1)..^1];
                var lastName = el[2][(el[2].IndexOf("\"") + 1)..^1];
                var age = int.Parse(el[3][(el[3].IndexOf("=") + 1)..]);
                var jobPosition = el[4][(el[4].IndexOf("\"") + 1)..^2];

                employees.Add(new Employee(pesel, firstName, lastName, age, jobPosition) as T);
            }

            return employees;
        }
    }

    public class XMLBuilder<T> : IGenericBuilder<T> where T : Employee
    {
        public void BuildPrint(List<T> employees)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            var writer = new StringWriter();
            serializer.Serialize(writer, employees);
            Console.WriteLine(writer.ToString());
        }

        public void BuildSave(List<T> employees)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            var writer = new StreamWriter($"{Globals.Path}\\{typeof(T).Name}s.xml");
            serializer.Serialize(writer, employees);
            writer.Close();
        }

        public List<T> BuildRead(string path)
        {
            var xml = new StringReader(File.ReadAllText(path + ".xml"));
            var serializer = new XmlSerializer(typeof(List<T>));
            var employees = (List<T>) serializer.Deserialize(xml);
            return employees;
        }
    }

    public class JSONBuilder<T> : IGenericBuilder<T> where T : Employee
    {
        public void BuildPrint(List<T> employees) =>
            Console.WriteLine(JsonSerializer.Serialize(employees));

        public void BuildSave(List<T> employees) =>
            File.WriteAllText($"{Globals.Path}\\{typeof(T).Name}s.json", JsonSerializer.Serialize(employees));

        public List<T> BuildRead(string path) =>
            JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path + ".json"));
    }

    public class EmployeeFile<T> where T : Employee
    {
        private List<T> _employeeList = new();
        private readonly IGenericBuilder<T> _builder;

        public EmployeeFile(IGenericBuilder<T> builder) =>
            _builder = builder;

        public bool Validate(T employee) =>
            _employeeList.All(emp => emp.Pesel != employee.Pesel);

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

            matches.ToList()
                .ForEach(employee => Console.WriteLine(employee.Show()));
        }

        public void Print() =>
            _builder.BuildPrint(_employeeList);

        public void Save() =>
            _builder.BuildSave(_employeeList);

        public void Read(string path) =>
            _employeeList = _builder.BuildRead(path);
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

        public Employee()
        {
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
            var file = new EmployeeFile<Employee>(new XMLBuilder<Employee>());
            var employees = new Employee[5];

            employees[0] = new Employee(99123456789, "Jan", "Kowalski", 19, "programmer");
            employees[1] = new Employee(99123456782, "Adam", "Nowak", 29);
            employees[2] = new Employee(99123456781, "Adam", "Drugi", 19, "tester");
            employees[3] = new Employee(99123456780, "Rafal", "Super", 20);
            employees[4] = new Employee(99123456780, "Rafal", "Super", 20);

            foreach (var employee in employees)
                file.AddEmployee(employee);

            file.Print();
            file.Save();
            file.Read($"{Globals.Path}\\Employees");
            Console.WriteLine("\nEmployees list:");
            file.ShowEmployees();
        }
    }
}
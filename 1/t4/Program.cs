using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace t4
{
    public static class Globals
    {
        public const string Path = "C:\\Users\\Matheo\\Desktop\\";
        public static readonly int Offset;

        static Globals()
        {
            try
            {
                Offset = int.Parse(ConfigurationManager.AppSettings["offset"] ?? "2");
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }
    }

    public static class CeasarCipher
    {
        public static string Encipher(string input, int offset) =>
            input.Aggregate(string.Empty, (current, ch) => current + Cipher(ch, offset));

        public static string Decipher(string input, int offset) =>
            Encipher(input, 26 - offset);

        private static char Cipher(char ch, int offset)
        {
            if (!char.IsLetter(ch)) return ch;

            var d = char.IsUpper(ch) ? 'A' : 'a';
            return (char) ((((ch + offset) - d) % 26) + d);
        }
    }

    public interface IGenericBuilder<T> where T : Employee
    {
        void BuildPrint(List<T> employees);
        void BuildPrintEncrypted(List<T> employees, int offset);
        void BuildSave(List<T> employees);
        void BuildSaveEncrypted(List<T> employees, int offset);
        List<T> BuildRead(string path);
        List<T> BuildReadEncrypted(string path, int offset);
    }

    public class TXTBuilder<T> : IGenericBuilder<T> where T : Employee
    {
        private static void Print(List<T> employees, Func<Employee, string> f) =>
            Console.WriteLine(string.Join("\n", employees.Select(f)));

        private static void Save(List<T> employees, bool encrypted = false, int offset = 0, string info = "")
        {
            var content = encrypted
                ? CeasarCipher.Encipher(string.Join("\n", employees.Select(emp => emp.Show()).ToList()), offset)
                : string.Join("\n", employees.Select(emp => emp.Show()).ToList());

            File.WriteAllText($"{Globals.Path}\\{typeof(T).Name}s{info}.txt", content);
        }

        private static List<T> Read(string path, bool encrypted = false, int offset = 0)
        {
            List<T> employees = new();

            var lines = File.ReadLines(path + ".txt");
            if (encrypted) lines = lines.Select(line => CeasarCipher.Decipher(line, offset)).ToList();

            foreach (var emp in lines)
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

        public void BuildPrint(List<T> employees) =>
            Print(employees, emp => emp.Show());

        public void BuildPrintEncrypted(List<T> employees, int offset) =>
            Print(employees, emp => CeasarCipher.Encipher(emp.Show(), offset));

        public void BuildSave(List<T> employees) =>
            Save(employees);

        public void BuildSaveEncrypted(List<T> employees, int offset) =>
            Save(employees, true, offset, "Encrypted");

        public List<T> BuildRead(string path) =>
            Read(path);

        public List<T> BuildReadEncrypted(string path, int offset) =>
            Read(path, true, offset);
    }

    public class XMLBuilder<T> : IGenericBuilder<T> where T : Employee
    {
        private void Print(List<T> employees, bool encrypted = false, int offset = 0)
        {
            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(List<T>));
            serializer.Serialize(writer, employees);
            Console.WriteLine(encrypted ? CeasarCipher.Encipher(writer.ToString(), offset) : writer.ToString());
        }

        private void Save(List<T> employees, bool encrypted = false, int offset = 0, string info = "")
        {
            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(List<T>));
            serializer.Serialize(writer, employees);
            File.WriteAllText($"{Globals.Path}\\{typeof(T).Name}s{info}.xml",
                encrypted ? CeasarCipher.Encipher(writer.ToString(), offset) : writer.ToString());
        }

        private static List<T> Read(string path, bool encrypted = false, int offset = 0)
        {
            var xml = encrypted
                ? new StringReader(CeasarCipher.Decipher(File.ReadAllText(path + ".xml"), offset))
                : new StringReader(File.ReadAllText(path + ".xml"));
            var serializer = new XmlSerializer(typeof(List<T>));
            try
            {
                var employees = (List<T>) serializer.Deserialize(xml);
                return employees;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("The given offset is wrong!");
            }
        }

        public void BuildPrint(List<T> employees) =>
            Print(employees);

        public void BuildPrintEncrypted(List<T> employees, int offset) =>
            Print(employees, true, offset);

        public void BuildSave(List<T> employees) =>
            Save(employees);

        public void BuildSaveEncrypted(List<T> employees, int offset) =>
            Save(employees, true, offset, "Encrypted");

        public List<T> BuildRead(string path) =>
            Read(path);

        public List<T> BuildReadEncrypted(string path, int offset) =>
            Read(path, true, offset);
    }

    public class JSONBuilder<T> : IGenericBuilder<T> where T : Employee
    {
        public void BuildPrint(List<T> employees) =>
            Console.WriteLine(JsonSerializer.Serialize(employees));

        public void BuildPrintEncrypted(List<T> employees, int offset) =>
            Console.WriteLine(CeasarCipher.Encipher(JsonSerializer.Serialize(employees), offset));

        public void BuildSave(List<T> employees) =>
            File.WriteAllText($"{Globals.Path}\\{typeof(T).Name}s.json", JsonSerializer.Serialize(employees));

        public void BuildSaveEncrypted(List<T> employees, int offset) =>
            File.WriteAllText($"{Globals.Path}\\{typeof(T).Name}sEncrypted.json",
                CeasarCipher.Encipher(JsonSerializer.Serialize(employees), offset));

        public List<T> BuildRead(string path) =>
            JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path + ".json"));

        public List<T> BuildReadEncrypted(string path, int offset) =>
            JsonSerializer.Deserialize<List<T>>(CeasarCipher.Decipher(File.ReadAllText(path + ".json"), offset));
    }

    public class EmployeeFile<T> where T : Employee
    {
        private List<T> _employeeList = new();
        private readonly IGenericBuilder<T> _builder;

        public EmployeeFile(IGenericBuilder<T> builder) =>
            _builder = builder;

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

        public void Print() =>
            _builder.BuildPrint(_employeeList);

        public void PrintEncrypted(int offset) =>
            _builder.BuildPrintEncrypted(_employeeList, offset);

        public void Save() =>
            _builder.BuildSave(_employeeList);

        public void SaveEncrypted(int offset) =>
            _builder.BuildSaveEncrypted(_employeeList, offset);

        public void Read(string path) =>
            _employeeList = _builder.BuildRead(path);

        public void ReadEncrypted(string path, int offset) =>
            _employeeList = _builder.BuildReadEncrypted(path, offset);
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

        public bool IsMatch(Employee employee) =>
            Pesel == employee.Pesel;
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var file = new EmployeeFile<Employee>(new XMLBuilder<Employee>());
            var employees = new List<Employee>
            {
                new(99123456789, "Jan", "Kowalski", 19, "programmer"),
                new(99123456782, "Adam", "Nowak", 29),
                new(99123456781, "Adam", "Drugi", 19, "tester"),
                new(99123456780, "Rafal", "Super", 20),
                new(99123456781, "Rafal", "Super", 20)
            };

            employees.ForEach(employee => file.AddEmployee(employee));

            // file.Print();
            // file.PrintEncrypted(Globals.Offset);
            file.Save();
            file.SaveEncrypted(Globals.Offset);

            file.Read($"{Globals.Path}\\Employees");
            Console.WriteLine("\nREAD:");
            file.ShowEmployees();

            Console.WriteLine("\nREAD ENCRYPTED:");
            file.ReadEncrypted($"{Globals.Path}\\EmployeesEncrypted", Globals.Offset);
            file.ShowEmployees();
        }
    }
}
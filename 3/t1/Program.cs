using System;
using System.Collections.Generic;
using System.Linq;


namespace t1
{
    //SINGLETON:

    public class LoadBalancer
    {
        private static LoadBalancer _instance;
        private static readonly object Locker = new();
        public string Value { get; set; }

        public static LoadBalancer GetInstance(string value)
        {
            if (_instance == null)
            {
                lock (Locker)
                {
                    if (_instance == null)
                    {
                        _instance = new LoadBalancer();
                        _instance.Value = value;
                    }
                }
            }

            return _instance;
        }
    }

    //BUILDER:

    public class Manufacturer
    {
        public static void Construct(ComputerBuilder computerBuilder)
        {
            computerBuilder.BuildCase();
            computerBuilder.BuildCPU();
            computerBuilder.BuildRAM();
            computerBuilder.BuildGPU();
        }
    }


    public abstract class ComputerBuilder
    {
        protected Computer computer;
        public Computer Computer => computer;
        public abstract void BuildCase();
        public abstract void BuildCPU();
        public abstract void BuildRAM();
        public abstract void BuildGPU();
    }

    class OfficeComputerBuilder : ComputerBuilder
    {
        public OfficeComputerBuilder()
        {
            computer = new Computer("Office");
        }

        public override void BuildCase() =>
            computer["case"] = "Micro-ATX";

        public override void BuildCPU() =>
            computer["cpu"] = "4 cores";

        public override void BuildRAM() =>
            computer["ram"] = "8gb";

        public override void BuildGPU() =>
            computer["gpu"] = "Integrated with CPU";
    }

    class GamingComputerBuilder : ComputerBuilder
    {
        public GamingComputerBuilder()
        {
            computer = new Computer("Gaming");
        }

        public override void BuildCase() =>
            computer["case"] = "ATX";

        public override void BuildCPU() =>
            computer["cpu"] = "8 cores";

        public override void BuildRAM() =>
            computer["ram"] = "32gb";

        public override void BuildGPU() =>
            computer["gpu"] = "RTX 3060 Ti";
    }

    public class Computer
    {
        private readonly string _computerType;
        private readonly Dictionary<string, string> _parts = new();

        public Computer(string computerType)
        {
            _computerType = computerType;
        }

        public string this[string key]
        {
            get => _parts[key];
            set => _parts[key] = value;
        }

        public void Show()
        {
            Console.WriteLine("\n---------------------------");
            Console.WriteLine($"Computer Type: {_computerType}");
            Console.WriteLine($" Case : {_parts["case"]}");
            Console.WriteLine($" CPU : {_parts["cpu"]}");
            Console.WriteLine($" RAM: {_parts["ram"]}");
            Console.WriteLine($" GPU : {_parts["gpu"]}");
        }
    }

    //DECORATOR:

    public abstract class RentalVehicle
    {
        public int Amount { get; set; }

        public abstract void Display();
    }

    public class Scooter : RentalVehicle
    {
        private readonly string _type;
        private readonly int _wheels;

        public Scooter(string type, int wheels, int amount)
        {
            _type = type;
            _wheels = wheels;
            Amount = amount;
        }

        public override void Display()
        {
            Console.WriteLine("\nScooter ------ ");
            Console.WriteLine($" Type: {_type}");
            Console.WriteLine($" Wheels: {_wheels}");
            Console.WriteLine($" Amount: {Amount}\n");
        }
    }

    public class Car : RentalVehicle
    {
        private readonly string _brand;
        private readonly string _model;
        private readonly string _engine;

        public Car(string brand, string model, string engine, int amount)
        {
            _brand = brand;
            _model = model;
            _engine = engine;
            Amount = amount;
        }

        public override void Display()
        {
            Console.WriteLine("\nCar ----- ");
            Console.WriteLine($" Brand: {_brand}");
            Console.WriteLine($" Model: {_model}");
            Console.WriteLine($" Engine: {_engine}");
            Console.WriteLine($" Amount: {Amount}\n");
        }
    }

    public abstract class Decorator : RentalVehicle
    {
        protected readonly RentalVehicle RentalVehicle;

        public Decorator(RentalVehicle rentalVehicle)
        {
            RentalVehicle = rentalVehicle;
        }

        public override void Display() =>
            RentalVehicle.Display();
    }

    public class Rentable : Decorator
    {
        protected readonly List<string> Renters = new();

        public Rentable(RentalVehicle rentalVehicle) : base(rentalVehicle)
        {
        }

        public void RentVehicle(string name)
        {
            Renters.Add(name);
            RentalVehicle.Amount--;
        }

        public void ReturnVehicle(string name)
        {
            Renters.Remove(name);
            RentalVehicle.Amount++;
        }

        public override void Display()
        {
            base.Display();
            Renters.ForEach(Console.WriteLine);
        }
    }


    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("\nSINGLETON: ");

            var list = new List<LoadBalancer>
            {
                LoadBalancer.GetInstance("a"),
                LoadBalancer.GetInstance("b"),
                LoadBalancer.GetInstance("c")
            };

            if (list[0] == list[1] && list[1] == list[2])
                Console.WriteLine("Same instance\n");

            list.ForEach(b => Console.WriteLine(b.Value));


            Console.WriteLine("\nBUILDER: ");

            ComputerBuilder builder = new OfficeComputerBuilder();
            Manufacturer.Construct(builder);
            builder.Computer.Show();

            builder = new GamingComputerBuilder();
            Manufacturer.Construct(builder);
            builder.Computer.Show();
            
            Console.WriteLine("\nDECORATOR: ");
            
            new Scooter("Electric", 2, 20).Display();
            
            var car = new Car("Tesla", "S", "762 KM", 2);
            car.Display();

            Console.WriteLine("RENTED CAR:");

            var rentalcar = new Rentable(car);
            rentalcar.RentVehicle("Kamil #1");
            rentalcar.RentVehicle("Adam #2");
            rentalcar.Display();
            
            
            Console.WriteLine("\nONE CAR RETURNED:");

            rentalcar.ReturnVehicle("Adam #2");
            rentalcar.Display();
        }
    }
}
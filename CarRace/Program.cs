using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRace
{
    public class Car
    {
        public string Name { get; set; }
        public int Distance { get; set; } = 0;
        public int Position { get; set; } = 0;

        public Car(string name)
        {
            Name = name;
        }

        public void Move()
        {
            Random rand = new Random();
            while (Distance < 100)
            {
                int moveDistance = rand.Next(1, 10);
                Distance += moveDistance;

                if (Distance >= 100)
                {
                    Distance = 100;
                }

                Task.Delay(1000).Wait();
            }
        }

        public double CalculatePercentage()
        {
            return (double)Distance / 100 * 100;
        }

        public string GetProgressBar()
        {
            int totalBars = 100; 
            int filledBars = (int)(CalculatePercentage() / 100 * totalBars);
            int emptyBars = totalBars - filledBars;

            return $"[{new string('|', filledBars)}{new string(' ', emptyBars)}]";
        }
    }


    internal class Race
    {
        private static int pos = 1;

        static async Task Main(string[] args)
        {
            List<Car> cars = new List<Car>
        {
            new Car("Car1"),
            new Car("Car2"),
            new Car("Car3")
        };

            List<Task> tasks = new List<Task>();

            foreach (var car in cars)
            {
                tasks.Add(Task.Run(() =>
                {
                    car.Move();
                    car.Position = pos++;
                }));
            }

            var monitoringTask = Task.Run(async () =>
            {
                while (tasks.Exists(task => !task.IsCompleted))
                {
                    await Task.Delay(1000);

                    var progressLines = new List<string>();
                    foreach (var car in cars)
                    {
                        progressLines.Add($"{car.Name.PadRight(5)}: {car.GetProgressBar()} {car.CalculatePercentage():F2}%");
                    }


                    Console.Clear();

                    Console.WriteLine("-- Race Progress --");
                    foreach (var line in progressLines)
                    {
                        Console.WriteLine(line);
                    }
                }
            });

            await Task.WhenAll(tasks);
            await monitoringTask;

            cars.Sort((c1, c2) => c1.Position.CompareTo(c2.Position));

            Console.WriteLine("\nRace finished!");
            foreach (var car in cars)
            {
                Console.WriteLine($"{car.Name} finished #{car.Position}!");
            }
        }
    }
}
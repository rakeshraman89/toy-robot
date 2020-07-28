using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Toy.Robot.Common.Interfaces;

namespace Toy.Robot.ConsoleApp
{
    public class Program
    {
        private static IToyOperations _toyOperations;
        public static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                }).UseConsoleLifetime();

            var host = builder.Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                _toyOperations = services.GetService<IToyOperations>();
            }
            Console.WriteLine("**************");
            Console.WriteLine("\nHi this is Chitti-The robot!");
            Console.WriteLine("\nEnter the file name:");
            var fileName = Console.ReadLine();
            var commands = File.ReadAllLines($"C:\\Dev\\Source\\Sample\\toy-robot-puzzle\\TestData\\{fileName}.txt");
            if (commands == null || commands.Length == 0)
            {
                Console.WriteLine("File Name was not specified");
            }
            ReadOperations(commands);
        }

        private static void ReadOperations(string[] commands)
        {
            _toyOperations.PerformOperations(commands);
        }
    }
}

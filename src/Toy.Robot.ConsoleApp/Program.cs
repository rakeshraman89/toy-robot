using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Toy.Robot.Common.Interfaces;
using Toy.Robot.Operations;

namespace Toy.Robot.ConsoleApp
{
    public class Program
    {
        private static IToyOperations _toyOperations;
        public static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    // logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IToyOperations, ToyOperations>();
                    services.AddTransient<IRobotCommands, RobotCommands>();
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
            var operations = File.ReadAllLines($"C:\\Dev\\Source\\Sample\\toy-robot-puzzle\\TestData\\RobotCommands.txt");
            if (operations == null || operations.Length == 0)
            {
                Console.WriteLine("File Name was not specified");
            }
            ReadOperations(operations);
            Console.ReadLine();
        }

        private static void ReadOperations(string[] operations)
        {
            _toyOperations.ProcessOperations(operations);
        }
    }
}

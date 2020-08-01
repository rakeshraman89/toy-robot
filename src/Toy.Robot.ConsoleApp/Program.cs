using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Toy.Robot.Common;
using Toy.Robot.Common.Exceptions;
using Toy.Robot.Common.Interfaces;
using Toy.Robot.Operations;

namespace Toy.Robot.ConsoleApp
{
    public class Program
    {
        private static IToyOperations _toyOperations;
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:false)
                .Build();
            var builder = new HostBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddTransient<IToyOperations, ToyOperations>()
                        .AddTransient<IRobotCommands, RobotCommands>()
                        .AddOptions()
                        .Configure<ToyRobotSettings>(config.GetSection("ToyRobot"));
                }).UseConsoleLifetime();

            var host = builder.Build();
            
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                _toyOperations = services.GetService<IToyOperations>();
            }

            var exit = false;
            do
            {
                Console.WriteLine("\n************************");
                Console.WriteLine("\nWelcome to the Toy Robot puzzle!");
                Console.Write("\nEnter the file name:");
                var fileName = Console.ReadLine();
                var operations = File.ReadAllLines($"C:\\Dev\\Source\\Sample\\toy-robot-puzzle\\TestData\\RobotCommands.txt");
                if (operations == null || operations.Length == 0)
                {
                    Console.WriteLine("File does not exist");
                }
                ReadOperations(operations);
                Console.Write("\nWould you like to continue (Y/N)?");
                var isYesOrNo = Console.ReadLine();
                if (isYesOrNo == "N" || isYesOrNo == "n") exit = true;
            } while (!exit);
        }

        private static void ReadOperations(string[] operations)
        {
            try
            {
                _toyOperations.ProcessOperations(operations);
            }
            catch (CommandException ce)
            {
                Console.WriteLine(ce.Message);
                Console.WriteLine(@" The commands should be in the following form
                    PLACE X,Y,FACING
                    MOVE
                    LEFT
                    RIGHT
                    REPORT");
            }
        }
    }
}

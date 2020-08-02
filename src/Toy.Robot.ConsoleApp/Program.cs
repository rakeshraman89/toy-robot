using System;
using System.IO;
using System.Linq;
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
            ToyRobotSettings settings;
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                _toyOperations = services.GetService<IToyOperations>();
                var robotSettings = services.GetService<IOptions<ToyRobotSettings>>();
                settings = robotSettings?.Value;
            }

            var exit = false;

            Console.WriteLine("\n************************");
            Console.WriteLine("\nWelcome to the Toy Robot puzzle!");
            Console.WriteLine($"\nBoard size is {settings?.Board.Length}, {settings?.Board.Breadth} \n");

            // execute the files with test data
            if (settings != null && settings.TestFiles.Any())
            {
                foreach (var file in settings.TestFiles)
                {
                    ReadFile(file);
                }
            }

            do
            {
                try
                {
                    Console.Write("\nEnter the file name (or enter q or quit to exit):");
                    var fileName = Console.ReadLine();
                    if (fileName != null && (fileName.Equals("q", StringComparison.OrdinalIgnoreCase)
                        || fileName.Equals("quit", StringComparison.OrdinalIgnoreCase)))
                    {
                        exit = true;
                        continue;
                    }
                    ReadFile(fileName);
                }
                catch (FileNotFoundException fileException)
                {
                    Console.WriteLine($"Error finding the file - {fileException.FileName}");
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine($"Error finding the file - {argumentException.Message}");
                }
                catch (NotSupportedFileException exception)
                {
                    Console.WriteLine(exception.Message);
                }
                Console.Write("\nWould you like to continue (Y/N)?");
                var isYesOrNo = Console.ReadLine();
                if (isYesOrNo == "N" || isYesOrNo == "n") exit = true;
            } while (!exit);
        }

        private static void ReadFile(string fileName)
        {
            var operations = File.ReadAllLines($"{fileName}");
            if (operations == null || operations.Length == 0)
            {
                Console.WriteLine("There are no commands to execute");
            }
            ReadOperations(operations);
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

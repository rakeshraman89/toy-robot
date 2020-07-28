using System;
using System.IO;

namespace Toy.Robot.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("**************");
            Console.WriteLine("\nHi this is Chitti-The robot!");
            Console.WriteLine("\nEnter the file name:");
            var fileName = Console.ReadLine();
            var lines = File.ReadAllLines($"C:/Dev/Source/Sample/toy-robot-puzzle/TestData/{fileName}.txt");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }

            Console.ReadLine();
        }
    }
}

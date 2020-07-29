using System;
using Microsoft.Extensions.Logging;
using Toy.Robot.Common.Interfaces;

namespace Toy.Robot.Operations
{
    public class RobotCommands : IRobotCommands
    {
        private ILogger<RobotCommands> _logger;

        public RobotCommands(ILogger<RobotCommands> logger)
        {
            _logger = logger;
        }
        public void ExecutePlaceCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing place command");
        }

        public void ExecuteMoveCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing move command");
        }

        public void ExecuteTurnLeftCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing left command");
        }

        public void ExecuteTurnRightCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing right command");
        }

        public void ExecuteReportCommand()
        {
            Console.WriteLine("Executing report command");
        }
    }
}

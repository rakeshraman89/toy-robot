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
            _logger.LogDebug("");
            Console.WriteLine("Executing place command");
        }

        public void ExecuteMoveCommand(Common.Robot robot)
        {
            throw new NotImplementedException();
        }

        public void ExecuteTurnLeftCommand(Common.Robot robot)
        {
            throw new NotImplementedException();
        }

        public void ExecuteTurnRightCommand(Common.Robot robot)
        {
            throw new NotImplementedException();
        }
    }
}

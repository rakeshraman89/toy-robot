using System;
using Microsoft.Extensions.Logging;
using Toy.Robot.Common;
using Toy.Robot.Common.Interfaces;
using Toy.Robot.Common.Utils;

namespace Toy.Robot.Operations
{
    public class RobotCommands : IRobotCommands
    {
        private ILogger<RobotCommands> _logger;
        private TableTop _board = new TableTop();
        private Common.Robot _robot;
        public RobotCommands(ILogger<RobotCommands> logger)
        {
            _logger = logger;
        }
        public void ExecutePlaceCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing place command");
            if (!robot.IsPlacementValid(_board)) return;
            _robot = robot;
        }

        public void ExecuteMoveCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing move command");
            switch (robot.Direction)
            {
                case FacingDirection.North:
                    robot.Coordinate.Y += 1;
                    break;
                case FacingDirection.East:
                    robot.Coordinate.X += 1;
                    break;
                case FacingDirection.South:
                    robot.Coordinate.Y -= 1;
                    break;
                case FacingDirection.West:
                    robot.Coordinate.X -= 1;
                    break;
                default:
                    return;
            }

            if (robot.IsPlacementValid(_board))
            {
                _robot = robot;
            }

        }

        public void ExecuteTurnLeftCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing left command");
            switch (robot.Direction)
            {
                case FacingDirection.East:
                    robot.Direction = FacingDirection.North;
                    break;
                case FacingDirection.North:
                    robot.Direction = FacingDirection.West;
                    break;
                case FacingDirection.South:
                    robot.Direction = FacingDirection.East;
                    break;
                case FacingDirection.West:
                    robot.Direction = FacingDirection.South;
                    break;
                default:
                    return;
            }

            if (robot.IsPlacementValid(_board))
            {
                _robot = robot;
            }
        }

        public void ExecuteTurnRightCommand(Common.Robot robot)
        {
            Console.WriteLine("Executing right command");
            switch (robot.Direction)
            {
                case FacingDirection.North:
                    robot.Direction = FacingDirection.East;
                    break;
                case FacingDirection.East:
                    robot.Direction = FacingDirection.South;
                    break;
                case FacingDirection.South:
                    robot.Direction = FacingDirection.West;
                    break;
                case FacingDirection.West:
                    robot.Direction = FacingDirection.North;
                    break;
                default:
                    return;
            }
            if (robot.IsPlacementValid(_board))
            {
                _robot = robot;
            }
        }

        public void ExecuteReportCommand()
        {
            Console.WriteLine($"{_robot.Coordinate.X},{_robot.Coordinate.Y},{_robot.Direction.ToString()}");
        }
    }
}

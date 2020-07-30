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
        public RobotCommands(ILogger<RobotCommands> logger)
        {
            _logger = logger;
        }

        public void ExecuteMoveCommand(Common.Robot robot, TableTop board)
        {
            Console.WriteLine("Executing move command");
            var tempRobot = new Common.Robot()
            {
                Coordinate = new Position
                {
                    X = robot.Coordinate.X,
                    Y = robot.Coordinate.Y
                },
                Direction = robot.Direction
            };
            switch (tempRobot.Direction)
            {
                case FacingDirection.North:
                    tempRobot.Coordinate.Y += 1;
                    break;
                case FacingDirection.East:
                    tempRobot.Coordinate.X += 1;
                    break;
                case FacingDirection.South:
                    tempRobot.Coordinate.Y -= 1;
                    break;
                case FacingDirection.West:
                    tempRobot.Coordinate.X -= 1;
                    break;
                default:
                    return;
            }

            if (!tempRobot.IsPlacementValid(board)) return;
            robot.Coordinate.X = tempRobot.Coordinate.X;
            robot.Coordinate.Y = tempRobot.Coordinate.Y;
            robot.Direction = tempRobot.Direction;
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
        }

        public string ExecuteReportCommand(Common.Robot robot)
        {
            var report = $"{robot.Coordinate.X},{robot.Coordinate.Y},{robot.Direction.ToString()?.ToUpper()}";
            Console.WriteLine(report);
            return report;
        }
    }
}

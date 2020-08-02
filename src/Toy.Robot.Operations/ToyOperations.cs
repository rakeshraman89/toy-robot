using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Toy.Robot.Common;
using Toy.Robot.Common.Exceptions;
using Toy.Robot.Common.Interfaces;
using Toy.Robot.Common.Utils;

namespace Toy.Robot.Operations
{
    public class ToyOperations : IToyOperations
    {
        private readonly ILogger<ToyOperations> _logger;
        private IRobotCommands _robotCommands;
        public Common.Robot Robot;
        public bool IsToyPlaced;
        private string _currentReport = string.Empty;
        private readonly ToyRobotSettings _settings;
        private readonly TableTop _board;

        public ToyOperations(ILogger<ToyOperations> logger, IRobotCommands robotCommands, IOptions<ToyRobotSettings> settings)
        {
            _logger = logger;
            _robotCommands = robotCommands;
            _settings = settings.Value;
            _board = _settings.Board;
            Robot = new Common.Robot
            {
                Coordinate = new Position<int>()
            };
        }
        public void ProcessOperations(string[] commands)
        {
            // if both length and breadth of table top is zero, then logically there does not exist a table top
            if(_board == null || (_board.Length == 0 && _board.Breadth == 0)) 
                throw new CommandException("Error: Board size is not specified");

            _logger.LogDebug("Performing robot operations!!");
            //place[\\s][\\d]+[\\s]*,[\\s]*[\\d]+[\\s]*,[\\s]*(north|east|west|south) - regex to allow spaces before and after commas in PLACE command
            foreach (var operation in commands)
            {
                if (Regex.IsMatch(operation.ToLower(), $"^{Commands.PLACE.ToString().ToLower()}"))
                {
                    Place(operation);
                }
                if (!IsToyPlaced) continue;
                if (Regex.IsMatch(operation.ToLower(), $"^{Commands.PLACE.ToString().ToLower()}\\s"))
                {
                    Place(operation);
                }
                else if (Regex.IsMatch(operation.ToLower(), $"^{Commands.MOVE.ToString().ToLower()}$"))
                {
                    Move(operation);
                }
                else if (Regex.IsMatch(operation.ToLower(), $"^{Commands.LEFT.ToString().ToLower()}$"))
                {
                    TurnLeft();
                }
                else if (Regex.IsMatch(operation.ToLower(), $"^{Commands.RIGHT.ToString().ToLower()}$"))
                {
                    TurnRight();
                }
                else if (Regex.IsMatch(operation.ToLower(), $"^{Commands.REPORT.ToString().ToLower()}$"))
                {
                    Report();
                }
                else if (Regex.IsMatch(operation.ToLower(), "^echo\\s"))
                {
                    Console.WriteLine(operation.Substring(5));
                }
                else if (string.IsNullOrEmpty(operation) || string.IsNullOrWhiteSpace(operation) || operation[0] == '#') { }
                else
                {
                    throw new CommandException($"Error command:{operation}");
                }
            }
        }

        private Common.Robot SplitOperationParameters(string operation)
        {
            var operationParameters = operation.Split(' ', ',');
            if (operationParameters.Length != 4)
            {
                throw new CommandException($"Error command:{operation}");
            }
            var isValidXCoordinate = int.TryParse(operationParameters[1], out var x);
            var isValidYCoordinate = int.TryParse(operationParameters[2], out var y);
            if (!isValidXCoordinate || !isValidYCoordinate)
            {
                throw new CommandException($"Error command:{operation}");
            }
            var direction = operationParameters[3];
            var tempRobotPosition = new Common.Robot
            {
                Coordinate = new Position<int>
                {
                    X = x,
                    Y = y
                },
                Direction = Enum.Parse<FacingDirection>(direction, true)
            };
            return tempRobotPosition;
        }

        public void Place(string operation)
        {
            _logger.LogDebug($"Place operation:{operation}");
            var robotPosition = SplitOperationParameters(operation);
            if (!robotPosition.IsPlacementValid(_board)) return;
            Robot = robotPosition;
            IsToyPlaced = true;
        }

        public void Move(string operation)
        {
            _logger.LogDebug($"Move operation:{operation}");
            _robotCommands.ExecuteMoveCommand(Robot, _board);
        }

        public void TurnLeft()
        {
            _logger.LogDebug("Turning Left");
            _robotCommands.ExecuteTurnLeftCommand(Robot);
        }

        public void TurnRight()
        {
            _logger.LogDebug("Turning Right");
            _robotCommands.ExecuteTurnRightCommand(Robot);
        }

        public void Report()
        {
            _logger.LogDebug("Reporting operation");
            _currentReport = _robotCommands.ExecuteReportCommand(Robot);
            Console.WriteLine(_currentReport);
        }

        public string GetCurrentReport()
        {
            return _currentReport;
        }
    }
}

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Toy.Robot.Common;
using Toy.Robot.Common.Interfaces;

namespace Toy.Robot.Operations
{
    public class ToyOperations : IToyOperations
    {
        private readonly ILogger<ToyOperations> _logger;
        private IRobotCommands _robotCommands;
        public readonly Common.Robot Robot;
        private bool _isToyPlaced;
        private const string PLACE = "PLACE";
        private const string MOVE = "MOVE";
        private const string LEFT = "LEFT";
        private const string RIGHT = "RIGHT";
        public ToyOperations(ILogger<ToyOperations> logger, IRobotCommands robotCommands)
        {
            _logger = logger;
            _robotCommands = robotCommands;
            Robot = new Common.Robot();
        }
        public void ProcessOperations(string[] commands)
        {
            _logger.LogDebug("Performing robot operations!!");
            foreach (var operation in commands)
            {
                if (Regex.IsMatch(operation, $"^{PLACE}\\s"))
                {
                    Place(operation);
                }
                if (_isToyPlaced) continue;
                if(Regex.IsMatch(operation, $"^{MOVE}$"))
                {
                    Move(operation);
                }
                else if (Regex.IsMatch(operation, $"^{LEFT}$"))
                {
                    TurnLeft();
                }
                else if (Regex.IsMatch(operation, $"^{RIGHT}$"))
                {
                    TurnRight();
                }
                else
                {
                    _logger.LogError("Incorrect commands in the file");
                }
            }
        }

        public void Place(string operation)
        {
            _logger.LogDebug($"Place operation:{operation}");
            var placeParameters = operation.Split(' ', ',');
            if (placeParameters.Length != 4) return;
            int.TryParse(placeParameters[1], out var x);
            int.TryParse(placeParameters[2], out var y);
            var direction = placeParameters[3];
            Robot.Coordinate = new Position {X = x, Y = y};
            Robot.Direction = direction;
            _robotCommands.ExecutePlaceCommand(Robot);
            _isToyPlaced = true;
        }

        public void Move(string operation)
        {
            _logger.LogDebug($"Move operation:{operation}");
        }

        public void TurnLeft()
        {
            _logger.LogDebug($"Turning Left");
        }

        public void TurnRight()
        {
            _logger.LogDebug($"Turning Right");
        }
    }
}

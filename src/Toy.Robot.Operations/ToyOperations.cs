using System;
using Microsoft.Extensions.Logging;
using Toy.Robot.Common.Interfaces;

namespace Toy.Robot.Operations
{
    public class ToyOperations : IToyOperations
    {
        private readonly ILogger<ToyOperations> _logger;

        public ToyOperations(ILogger<ToyOperations> logger)
        {
            _logger = logger;
        }
        public void PerformOperations(string[] commands)
        {
            _logger.LogDebug("Performing robot operations!!");
        }

        public void Place()
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void TurnLeft()
        {
            throw new NotImplementedException();
        }

        public void TurnRight()
        {
            throw new NotImplementedException();
        }
    }
}

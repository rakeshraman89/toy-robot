using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Toy.Robot.Common.Interfaces;
using Toy.Robot.Operations;
using ILogger = NUnit.Framework.Internal.ILogger;

namespace Toy.Robot.UnitTest
{
    [TestFixture(Category = "Unit Test")]
    public class ToyOperationsUnitTest
    {
        private ILogger<ToyOperations> _logger;
        private IRobotCommands _robotCommands;
        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<ToyOperations>>();
            _robotCommands = Mock.Of<RobotCommands>();
        }

        [Test]
        public void TestToyOperations()
        {
            var subject = new ToyOperations(_logger, _robotCommands);
            var commands = File.ReadAllLines("C:\\Dev\\Source\\Sample\\toy-robot-puzzle\\TestData\\RobotCommands.txt");
            subject.ProcessOperations(commands);
        }
    }
}
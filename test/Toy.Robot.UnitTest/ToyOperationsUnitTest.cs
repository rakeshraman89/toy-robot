using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Toy.Robot.Common.Exceptions;
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
            _robotCommands = Mock.Of<IRobotCommands>();
        }

        [TestCase(true, new[] {"PLACE 1,2,EAST", "MOVE"}, TestName = "Test with 2 commands")]
        [TestCase(true, new[] {"PLACE 0,0,WEST", "MOVE","LEFT"}, TestName = "Test with 3 commands")]
        [TestCase(true, new[] {"PLACE 3,2,NORTH", "MOVE","LEFT","MOVE"}, TestName = "Test with 4 commands")]
        [TestCase(true, new[] {"PLACE 1,2,SOUTH", "MOVE", "MOVE","LEFT","MOVE","REPORT"}, TestName = "Test with 5 commands")]
        public void TestSuccessfulToyPlaceOperations(bool isToyPlaced, string[] commands)
        {
            var subject = new ToyOperations(_logger, _robotCommands);
            subject.ProcessOperations(commands);
            Assert.That(subject.IsToyPlaced, Is.EqualTo(isToyPlaced), "Toy is not placed");
        }

        [TestCase(false, new[] {"PLACE1,2,EAST", "MOVE"}, TestName = "Test with incorrect place command")]
        public void TestFailureToyPlaceOperations(bool isToyPlaced, string[] commands)
        {
            var subject = new ToyOperations(_logger, _robotCommands);
            Assert.Throws<CommandException>(() => subject.ProcessOperations(commands));
        }

        [TestCase("2,2,East", new[] {"PLACE 1,2,EAST", "MOVE", "REPORT"}, TestName = "Test Report with one move")]
        [TestCase("1,2,East", new[] {"PLACE 1,2,EAST", "REPORT"}, TestName = "Test Report status after place")]
        [TestCase("1,2,East", new[] {"PLACE 1,2,EAST", "MOVE", "PLACE 1,2,EAST", "REPORT" }, TestName = "Test Report status with 2 place commands")]
        [TestCase("5,5,East", new[] {"PLACE 5,5,EAST", "MOVE", "REPORT" }, TestName = "Test when robot is at the cliff facing east")]
        [TestCase("5,5,North", new[] {"PLACE 5,5,NORTH", "MOVE", "REPORT" }, TestName = "Test robot is at the cliff facing north")]
        [TestCase("5,5,East", new[] {"PLACE 4,5,EAST", "MOVE", "REPORT" }, TestName = "Test when robot is moved to the cliff facing east")]
        [TestCase("5,5,North", new[] {"PLACE 5,4,NORTH", "MOVE", "REPORT" }, TestName = "Test when robot is moved to the cliff facing north")]
        [TestCase("3,3,West", new[] {"PLACE 3,2,NORTH", "MOVE","LEFT", "REPORT" }, TestName = "Test when robot is rotated to the left")]
        public void TestRobotOperation(string expectedReport, string[] commands)
        {
            var subject = new ToyOperations(_logger, new RobotCommands(Mock.Of<ILogger<RobotCommands>>()));
            subject.ProcessOperations(commands);
            Assert.That(subject.GetCurrentReport(), Is.EqualTo(expectedReport), "Toy Robot execution is incorrect");
        }
    }
}
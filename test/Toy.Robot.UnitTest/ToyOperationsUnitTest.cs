using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Toy.Robot.Common;
using Toy.Robot.Common.Exceptions;
using Toy.Robot.Common.Interfaces;
using Toy.Robot.Operations;

namespace Toy.Robot.UnitTest
{
    [TestFixture(Category = "Functional Test")]
    public class ToyOperationsUnitTest
    {
        private ILogger<ToyOperations> _logger;
        private IRobotCommands _robotCommands;
        private IOptions<ToyRobotSettings> _settings;
        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<ToyOperations>>();
            _robotCommands = Mock.Of<IRobotCommands>();
            _settings = Mock.Of<IOptions<ToyRobotSettings>>();
            Mock.Get(_settings).Setup(o => o.Value)
                .Returns(new ToyRobotSettings
                {
                    Board = new TableTop
                    {
                        Length = 5,
                        Breadth = 5
                    },
                    FilePath = ""
                });
        }

        [TestCase(true, new[] {"PLACE 1,2,EAST", "MOVE"}, TestName = "Test with 2 commands")]
        [TestCase(true, new[] {"PLACE 0,0,WEST", "MOVE","LEFT"}, TestName = "Test with 3 commands")]
        [TestCase(true, new[] {"PLACE 3,2,NORTH", "MOVE","LEFT","MOVE"}, TestName = "Test with 4 commands")]
        [TestCase(true, new[] {"PLACE 1,2,SOUTH", "MOVE", "MOVE","LEFT","MOVE","REPORT"}, TestName = "Test with 5 commands")]
        public void TestSuccessfulToyPlaceOperations(bool isToyPlaced, string[] commands)
        {
            var subject = new ToyOperations(_logger, _robotCommands, _settings);
            subject.ProcessOperations(commands);
            Assert.That(subject.IsToyPlaced, Is.EqualTo(isToyPlaced), "Toy is not placed");
        }

        [TestCase(new[] {"PLACE1,2,EAST", "MOVE"}, "PLACE1,2,EAST", TestName = "Incorrect PLACE command format")]
        [TestCase(new[] {"PLACE 1,2,EAST", "MOVE1"}, "MOVE1", TestName = "Incorrect MOVE command format")]
        [TestCase(new[] {"PLACE 1,2,EAST", "MOVE", "PLACE 2, 2, NORTH"}, "PLACE 2, 2, NORTH", TestName = "Incorrect PLACE command format after move")]
        public void TestFailureToyPlaceOperations(string[] commands, string expectedMessage)
        {
            var subject = new ToyOperations(_logger, _robotCommands, _settings);
            Assert.That(() => subject.ProcessOperations(commands)
                ,Throws.TypeOf<CommandException>().With.Message.EqualTo($"Error command:{expectedMessage}"), "Exception message does not match");
        }

        [TestCase("2,2,EAST", new[] {"PLACE 1,2,EAST", "MOVE", "REPORT"}, TestName = "Report with one move")]
        [TestCase("1,2,EAST", new[] {"PLACE 1,2,EAST", "REPORT"}, TestName = "Report status after place")]
        [TestCase("1,2,EAST", new[] {"PLACE 1,2,EAST", "MOVE", "PLACE 1,2,EAST", "REPORT" }, TestName = "Report status with 2 place commands")]
        [TestCase("5,5,EAST", new[] {"PLACE 5,5,EAST", "MOVE", "REPORT" }, TestName = "robot is at the cliff facing east")]
        [TestCase("5,5,NORTH", new[] {"PLACE 5,5,NORTH", "MOVE", "REPORT" }, TestName = "robot is at the cliff facing north")]
        [TestCase("5,5,EAST", new[] {"PLACE 4,5,EAST", "MOVE", "REPORT" }, TestName = "robot is moved to the cliff facing east")]
        [TestCase("5,5,NORTH", new[] {"PLACE 5,4,NORTH", "MOVE", "REPORT" }, TestName = "robot is moved to the cliff facing north")]
        [TestCase("3,3,WEST", new[] {"PLACE 3,2,NORTH", "MOVE","LEFT", "REPORT" }, TestName = "robot is rotated to the left")]
        [TestCase("5,5,NORTH", new[] {"PLACE 4,4,EAST", "MOVE","LEFT","MOVE", "REPORT" }, TestName = "robot is moved to corner")]
        [TestCase("3,3,EAST", new[] {"PLACE 3,2,NORTH", "MOVE","RIGHT", "REPORT" }, TestName = "robot is rotated to the right")]
        [TestCase("2,0,SOUTH", new[] {"PLACE 1,1,EAST", "MOVE","RIGHT","MOVE", "REPORT" }, TestName = "robot is rotated to right")]
        public void TestRobotBasicOperation(string expectedReport, string[] commands)
        {
            var subject = new ToyOperations(_logger, new RobotCommands(Mock.Of<ILogger<RobotCommands>>()), _settings);
            subject.ProcessOperations(commands);
            Assert.That(subject.GetCurrentReport(), Is.EqualTo(expectedReport), "Toy Robot execution is incorrect");
        }

        [TestCase("0,0,SOUTH", new[] { "RIGHT", "MOVE", "PLACE 0,0,EAST", "RIGHT", "MOVE", "REPORT" }, TestName = "Commands before PLACE command are omitted")]
        public void TestCommandsIgnoredBeforePlace(string expectedReport, string[] commands)
        {
            var subject = new ToyOperations(_logger, new RobotCommands(Mock.Of<ILogger<RobotCommands>>()), _settings);
            subject.ProcessOperations(commands);
            Assert.That(subject.GetCurrentReport(), Is.EqualTo(expectedReport), "Expected report is incorrect");
        }
        
        [TestCase("0,0,SOUTH", new[] {"PLACE 0,0,EAST", "RIGHT","MOVE", "REPORT"}, TestName = "Robot moved south past boundary")]
        [TestCase("0,0,SOUTH", new[] {"PLACE 0,0,WEST", "MOVE","LEFT","MOVE", "REPORT"}, TestName = "Robot moved west past boundary")]
        [TestCase("0,1,NORTH", new[] {"PLACE 0,0,EAST", "RIGHT", "RIGHT", "RIGHT", "MOVE", "REPORT"}, TestName = "Robot moved north past boundary")]
        [TestCase("0,0,SOUTH", new[] { "# this is a test data", "PLACE 0,0,EAST", "echo we are going to move", "RIGHT", "MOVE", "REPORT"}
            ,TestName = "Robot command files with comments and echo")]
        public void TestRobotEdgeCaseOperation(string expectedReport, string[] commands)
        {
            var subject = new ToyOperations(_logger, new RobotCommands(Mock.Of<ILogger<RobotCommands>>()), _settings);
            subject.ProcessOperations(commands);
            Assert.That(subject.GetCurrentReport(), Is.EqualTo(expectedReport), "Expected report is incorrect");
        }
    }
}
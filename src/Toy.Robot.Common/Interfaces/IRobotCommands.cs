using System;
using System.Collections.Generic;
using System.Text;

namespace Toy.Robot.Common.Interfaces
{
    public interface IRobotCommands
    {
        // void ExecutePlaceCommand(Robot robot);
        void ExecuteMoveCommand(Robot robot, TableTop board);
        void ExecuteTurnLeftCommand(Robot robot);
        void ExecuteTurnRightCommand(Robot robot);
        string ExecuteReportCommand(Robot robot);
    }
}

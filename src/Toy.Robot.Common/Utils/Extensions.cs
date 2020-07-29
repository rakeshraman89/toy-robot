using System;
using System.Collections.Generic;
using System.Text;

namespace Toy.Robot.Common.Utils
{
    public static class Extensions
    {
        public static bool IsPlacementValid(this Robot robot, TableTop board)
        {
            return 0 <= robot.Coordinate.X
                   && robot.Coordinate.X <= board.Length
                   && 0 <= robot.Coordinate.Y
                   && robot.Coordinate.Y <= board.Breadth;
        }
    }
}

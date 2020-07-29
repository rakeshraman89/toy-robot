using System;
using Toy.Robot.Common.Utils;

namespace Toy.Robot.Common
{
    public class Robot
    {
        public Position Coordinate { get; set; }
        public FacingDirection Direction { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

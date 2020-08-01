using System;
using Toy.Robot.Common.Utils;

namespace Toy.Robot.Common
{
    public class Robot
    {
        public Position<int> Coordinate { get; set; }
        public FacingDirection Direction { get; set; }
    }

    public class Position<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Toy.Robot.Common
{
    public class ToyRobotSettings
    {
        public string FilePath { get; set; }
        public TableTop Board { get; set; }
    }

    public class TableTop
    {
        public int Length { get; set; } = 5;
        public int Breadth { get; set; } = 5;
    }
}

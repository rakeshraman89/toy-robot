using System;
using System.Collections.Generic;
using System.Text;

namespace Toy.Robot.Common.Interfaces
{
    public interface IToyOperations
    {
        void PerformOperations(string[] commands);
        void Place();
        void Move();
        void TurnLeft();
        void TurnRight();
    }
}

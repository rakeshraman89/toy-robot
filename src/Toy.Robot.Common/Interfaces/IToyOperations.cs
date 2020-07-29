using System;
using System.Collections.Generic;
using System.Text;

namespace Toy.Robot.Common.Interfaces
{
    public interface IToyOperations
    {
        void ProcessOperations(string[] operations);
        void Place(string command);
        void Move(string command);
        void TurnLeft();
        void TurnRight();
    }
}

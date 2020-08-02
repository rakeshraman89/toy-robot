using System;
using System.Collections.Generic;
using System.Text;

namespace Toy.Robot.Common.Exceptions
{
    public class NotSupportedFileException : Exception
    {
        public NotSupportedFileException()
        {
        }

        public NotSupportedFileException(string message) : base(message)
        {
        }
    }
}

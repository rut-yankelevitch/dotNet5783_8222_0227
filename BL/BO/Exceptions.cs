using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string? message) : base(message) { }
    }
    public class MistakeUpdateException : Exception
    {
        public MistakeUpdateException(string? message) : base(message) { }
    }

    public class InvalidInputException : Exception
    {
        public InvalidInputException(string? message) : base(message) { }
    }
    public class NotExistException : Exception
    {
        public NotExistException(string? message) : base(message) { }
    }
    public class ImpossibleActionException : Exception
    {
        public ImpossibleActionException(string? message) : base(message) { }
}
}

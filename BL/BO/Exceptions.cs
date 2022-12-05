using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{

 [Serializable]
public class BLDoesNotExistException:Exception
{
    public BLDoesNotExistException(string message, Exception innerException):base(message,innerException) 
    { }

    public override string ToString()=> base.ToString()+$"Entity is not exist";
}

[Serializable]
public class BLAlreadyExistException : Exception
{
        public BLAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        { }

        public override string ToString() => base.ToString() + $"Entity is already exist";
    }

    public class BLMistakeUpdateException : Exception
    {
        public BLMistakeUpdateException(string? message) : base(message) { }
    }

    public class BLInvalidInputException : Exception
    {
        public BLInvalidInputException(string? message) : base(message) { }
    }

    public class BLImpossibleActionException : Exception
    {
        public BLImpossibleActionException(string? message) : base(message) { }
}
}

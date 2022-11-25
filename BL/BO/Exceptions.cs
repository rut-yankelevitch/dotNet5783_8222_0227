using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    //public class BLDoesNotExistException : Exception
    //{
    //    public BLDoesNotExistException(string? message) : base(message) { }
    //}

    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string? message) : base(message) { }
    }
    public class MistakeUpdateException : Exception
    {
        public MistakeUpdateException(string? message) : base(message) { }
    }
    public class InvalidFormat : Exception
    {
        public InvalidFormat(string? message) : base(message) { }
    }

    public class NotExistException : Exception
    {
        public NotExistException(string? message) : base(message) { }
    }

    //public class BLOrderSend : Exception
    //{
    //    public BLOrderSend(string? message) : base(message) { }
    //}

}

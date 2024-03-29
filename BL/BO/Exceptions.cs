﻿namespace BO
{
    /// <summary>
    /// Throws class for non-existent items
    /// </summary>
    [Serializable]
    public class BLDoesNotExistException : Exception
    {
        public BLDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        { }

        public override string ToString() => base.ToString() + $"Entity is not exist";
    }


    /// <summary>
    /// Throws class for items that exist in duplicates
    /// </summary>
    [Serializable]
    public class BLAlreadyExistException : Exception
    {
        public BLAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        { }
        public override string ToString() => base.ToString() + $"Entity is already exist";
    }


    /// <summary>
    /// Throws class for mistake update
    /// </summary>
    [Serializable]
    public class BLMistakeUpdateException : Exception
    {
        public BLMistakeUpdateException(string? message) : base(message) { }
    }


    /// <summary>
    /// Throws class for invalid input
    /// </summary>
    [Serializable]
    public class BLInvalidInputException : Exception
    {
        public BLInvalidInputException(string? message) : base(message) { }
    }


    /// <summary>
    /// Throws class for impossible action
    /// </summary>
    [Serializable]
    public class BLImpossibleActionException : Exception
    {
        public BLImpossibleActionException(string? message) : base(message) { }
    }


    /// <summary>
    /// Throws No properties found in object
    /// </summary>
    [Serializable]
    public class BLNoPropertiesInObject : Exception
    {
        public override string Message =>
                        "No properties found in object";
    }


    /// <summary>
    /// Invalid Password
    /// </summary>
    [Serializable]
    public class BLInvalidPassword : Exception
    {
        public override string Message =>
                        "worng password";
    }
}

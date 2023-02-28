namespace DO;
/// <summary>
/// Throws class for non-existent items
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public int EntityId = -1;
    public string EntityName;
    public string? Message;

    public DalDoesNotExistException(int id, string name) : base()
    { EntityId = id; EntityName = name; }

    public DalDoesNotExistException(int id, string name, string message) : base(message)
    { EntityId = id; EntityName = name; }

    public DalDoesNotExistException(int id, string name, string message, Exception innerException) : base(message, innerException)
    { EntityId = id; EntityName = name; }
   
    public DalDoesNotExistException(string? message) : base(message)
    {
        Message = message;
    }
    
    public override string ToString()
    {
        if (EntityId == -1 && EntityName == null)
        {
            return Message;
        }
        if (EntityId == -1)
        {
            return $" {EntityName} are not exist.";
        }
        return $"id:{EntityId} of type {EntityName} is not exist.";
    }
}


/// <summary>
/// Throws class for items that exist in duplicates
/// </summary>
[Serializable]
public class DalAlreadyExistException : Exception
{
    public int EntityId;
    public string EntityName;

    public DalAlreadyExistException(int id, string name) : base()
    { EntityId = id; EntityName = name; }

    public DalAlreadyExistException(int id, string name, string message) : base(message)
    { EntityId = id; EntityName = name; }

    public DalAlreadyExistException(int id, string name, string message, Exception innerException) : base(message, innerException)
    { EntityId = id; EntityName = name; }
    public DalAlreadyExistException(string? message) : base(message)
    { }

    public override string ToString() => $"id:{EntityId} of type {EntityName} is already exist.";
}


/// <summary>
/// Throws Error loading the dal
/// </summary>
[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}


/// <summary>
/// Throws Error create xml files
/// </summary>
[Serializable]
public class XMLFileLoadCreateException : Exception
{
    string path;
    public XMLFileLoadCreateException(string path, Exception ex) : base() { this.path = path; }
    public override string ToString() => $"fail to load xml file: {path}";

}


/// <summary>
/// Throws Error aml file is null
/// </summary>
[Serializable]
public class XMLFileNullExeption : Exception
{
    public override string Message =>
                    "XML file is empty";

}


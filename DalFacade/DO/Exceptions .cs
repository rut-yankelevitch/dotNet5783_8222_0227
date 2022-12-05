using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DO;
[Serializable]
public class DalDoesNotExistException:Exception
{
    public int EntityId ;
    public string EntityName;
    public DalDoesNotExistException(int id, string name):base() 
    { EntityId=id;EntityName=name;}

    public DalDoesNotExistException(int id, string name,string message):base(message) 
    { EntityId=id;EntityName=name; }

        public DalDoesNotExistException(int id, string name,string message,Exception innerException):base(message,innerException) 
    { EntityId=id;EntityName=name; }
    //public DalDoesNotExistException(string name,string message) : base()
    //{ EntityName = name; }

        public DalDoesNotExistException(string? message):base(message) 
    { }


    public override string ToString() 
    {
        if( EntityId==-1)
        {
            return $" {EntityName} are not exist.";
        }
         return $"id:{EntityId} of type {EntityName} is not exist.";
     }
}
[Serializable]
public class DalAlreadyExistException : Exception
{
    public int EntityId;
    public string EntityName;

    public DalAlreadyExistException(int id, string name):base() 
    { EntityId=id;EntityName=name;}

    public DalAlreadyExistException(int id, string name,string message):base(message) 
    { EntityId=id;EntityName=name; }

    public DalAlreadyExistException(int id, string name,string message,Exception innerException):base(message,innerException) 
    { EntityId=id;EntityName=name; }
    public DalAlreadyExistException(string? message):base(message) 
    { }

    public override string ToString()=>$"id:{EntityId} of type {EntityName} is already exist.";
}

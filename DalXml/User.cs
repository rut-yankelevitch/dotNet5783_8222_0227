using DalApi;
using DO;
using System.Xml.Serialization;

namespace Dal;

public class User : IUser
{
    static string s_user = @"User";
    public int Add(DO.User user)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        if (listUsers.FirstOrDefault(use => use?.ID == user.ID) != null)
            throw new DO.DalAlreadyExistException(user.ID, "User");
        //user.ID = (int)((listUsers.Last() == null) ? (listUsers.Last()?.ID) + 1! : 1)!;
        listUsers.Add(user);
        XMLTools.SaveListToXMLSerializer(listUsers, s_user);
        return user.ID;
    }

    public void Delete(int id)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        if (listUsers.RemoveAll(use => use?.ID == id) == 0)
            throw new DalDoesNotExistException(id, "user");
        XMLTools.SaveListToXMLSerializer(listUsers, s_user);
    }


    public DO.User GetByCondition(Func<DO.User?, bool> predicate)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        return listUsers.FirstOrDefault(predicate) ??
        throw new DalDoesNotExistException("The requested user was not found.");
    }


    public IEnumerable<DO.User?> GetAll(Func<DO.User?, bool>? predicate)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        return ((predicate == null) ?
              listUsers.Select(use => use).OrderBy(use => use?.ID) :
              listUsers.Where(predicate).OrderBy(use => use?.ID)) ?? throw new DO.DalDoesNotExistException("The requested users were not found"); ;
    }

    public void Update(DO.User user)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);
        int count = listUsers.RemoveAll(use => use?.ID == user.ID);
        if (count == 0)
            throw new DalDoesNotExistException(user.ID, "user");
        listUsers.Add(user);
        XMLTools.SaveListToXMLSerializer(listUsers, s_user);
    }
}


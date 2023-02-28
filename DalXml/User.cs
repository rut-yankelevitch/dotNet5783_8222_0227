using DalApi;
using DO;

namespace Dal;

public class User : IUser
{
    static string s_user = @"User";
   
    /// <summary>
    /// add user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="DO.DalAlreadyExistException"></exception>
    public int Add(DO.User user)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        if (listUsers.FirstOrDefault(use => use?.ID == user.ID) != null)
            throw new DO.DalAlreadyExistException(user.ID, "User");
        user.ID =XMLTools.getNextID(@"NextUserId");
        listUsers.Add(user);
        XMLTools.SaveListToXMLSerializer(listUsers, s_user);
        return user.ID;
    }


    /// <summary>
    /// delete user by id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        if (listUsers.RemoveAll(use => use?.ID == id) == 0)
            throw new DalDoesNotExistException(id, "user");
        XMLTools.SaveListToXMLSerializer(listUsers, s_user);
    }


    /// <summary>
    /// delete user by condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public DO.User GetByCondition(Func<DO.User?, bool> predicate)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        return listUsers.FirstOrDefault(predicate) ??
        throw new DalDoesNotExistException("The requested user was not found.");
    }

    /// <summary>
    /// get all users 
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="DO.DalDoesNotExistException"></exception>
    public IEnumerable<DO.User?> GetAll(Func<DO.User?, bool>? predicate)
    {
        List<DO.User?> listUsers = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user);

        return ((predicate == null) ?
              listUsers.Select(use => use).OrderBy(use => use?.ID) :
              listUsers.Where(predicate).OrderBy(use => use?.ID)) ?? throw new DO.DalDoesNotExistException("The requested users were not found"); ;
    }


    /// <summary>
    /// update user
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
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


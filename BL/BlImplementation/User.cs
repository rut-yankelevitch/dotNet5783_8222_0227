using System.Runtime.CompilerServices;

namespace BlImplementation;

public class User : BlApi.IUser
{
    private DalApi.IDal dal = DalApi.Factory.Get();

    /// <summary>
    /// add user 
    /// </summary>
    /// <param name="u"></param>
    /// <returns></returns>
    /// <exception cref="BO.BLAlreadyExistException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? AddUser(BO.User u)
    {
        // if (IsRegistered(u.Email, u.Password)) throw new BlUserExistsException();
        try { return dal.User.Add(castBoUserToDoUser(u)); }


        catch (DO.DalAlreadyExistException ex) { throw new BO.BLAlreadyExistException("user already exist", ex); }
    }

    /// <summary>
    /// the function check if the email and the password are exist in user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    /// <exception cref="BO.BLInvalidPassword"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? IsRegistered(string email, string pass)
    {
        try
        {
            DO.User user = dal.User.GetByCondition(u => u?.Email == email);
            if (user.Password == pass) return user.ID;
            else throw new BO.BLInvalidPassword();

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("user doesnot exist", ex);
        }
    }

    /// <summary>
    /// the function update the datails of user
    /// </summary>
    /// <param name="u"></param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateUser(BO.User u)
    {
        dal.User.Update(castBoUserToDoUser(u));
    }


    /// <summary>
    /// cast BoUserToDoUser
    /// </summary>
    /// <param name="use"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private DO.User castBoUserToDoUser(BO.User use)
    {
        return new DO.User{ ID = use.ID, Name = use.Name, Email = use.Email, Address = use.Address,Password=use.Password };
       
    }
}

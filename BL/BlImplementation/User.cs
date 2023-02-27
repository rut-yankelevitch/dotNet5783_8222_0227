using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;

namespace BlImplementation;

public class User : BlApi.IUser
{
    private DalApi.IDal dal = DalApi.Factory.Get();
    public int? AddUser(BO.User u)
    {
        // if (IsRegistered(u.Email, u.Password)) throw new BlUserExistsException();
        try { return dal.User.Add(BlUtils.cast<DO.User, BO.User>(u)); }
        catch (DO.DalAlreadyExistException ex) { throw new BO.BLAlreadyExistException("user already exist", ex); }
    }

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



    public void UpdateUser(BO.User u)
    {
        dal.User.Update(BlUtils.cast<DO.User, BO.User>(u));
    }
}

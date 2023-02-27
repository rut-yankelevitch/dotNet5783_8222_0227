using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal;

internal class UserDal : IUser
{
    public int Add(User t)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User?> GetAll(Func<User?, bool>? predicate = null)
    {
        throw new NotImplementedException();
    }


    public User GetByCondition(Func<User?, bool> predicate)
    {
        throw new NotImplementedException();
    }


    public void Update(User t)
    {
        throw new NotImplementedException();
    }
}

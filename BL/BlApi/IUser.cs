using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BlImplementation;
using BO;

namespace BlApi;

public interface IUser
{
    [MethodImpl(MethodImplOptions.Synchronized)]

    public int? AddUser(BO.User u);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateUser(BO.User u);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? IsRegistered(string email, string pass);

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi;

public interface IUser
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? AddUser(User u);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateUser(User u);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? IsRegistered(string email, string pass);

}

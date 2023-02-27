using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DO;


namespace DalApi;

public interface ICartItem : ICrud<DO.CartItem>
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(Func<DO.CartItem, bool> f);

}


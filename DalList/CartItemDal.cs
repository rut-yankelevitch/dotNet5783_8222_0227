using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal;

internal class CartItemDal : ICartItem
{
    public int Add(CartItem obj)
    {
        throw new NotImplementedException();
    }

    public void Delete(Func<CartItem, bool> f)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CartItem?> GetAll(Func<CartItem?, bool>? predicate = null)
    {
        throw new NotImplementedException();
    }

    public CartItem GetByCondition(Func<CartItem?, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public void Update(CartItem obj)
    {
        throw new NotImplementedException();
    }
}

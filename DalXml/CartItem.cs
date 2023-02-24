using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DalApi;
using DO;

namespace Dal;

public class CartItem : ICartItem
{

    static string s_cartItem = @"CartItem";

    public int Add(DO.CartItem cartItem)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        if (listCartItems.FirstOrDefault(cItem => cItem?.ID == cartItem.ID) != null)
            throw new DO.DalAlreadyExistException(cartItem.ID, "CartItem");
        cartItem.ID = (int)((listCartItems.Last()==null)? (listCartItems.Last()?.ID)+1!:1)!;
        listCartItems.Add(cartItem);
        XMLTools.SaveListToXMLSerializer(listCartItems, s_cartItem);
        return cartItem.ID;
    }

    public void Delete(int id)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        if (listCartItems.RemoveAll(cItem => cItem?.ID == id) == 0)
            throw new DalDoesNotExistException(id, "cartItem");
        XMLTools.SaveListToXMLSerializer(listCartItems, s_cartItem);
    }


    public DO.CartItem GetByCondition(Func<DO.CartItem?, bool> predicate)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        return listCartItems.FirstOrDefault(predicate) ??
        throw new DalDoesNotExistException("The requested cartItem was not found.");
    }


    public IEnumerable<DO.CartItem?> GetAll(Func<DO.CartItem?, bool>? predicate)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        return ((predicate == null) ?
              listCartItems.Select(cItem => cItem).OrderBy(cItem => cItem?.ID) :
              listCartItems.Where(predicate).OrderBy(cItem => cItem?.ID)) ?? throw new DO.DalDoesNotExistException("The requested cartItems were not found"); ;
    }

    public void Update(DO.CartItem cartItem)
    {
        List<DO.CartItem?> listCartItem = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);
        int count = listCartItem.RemoveAll(cItem => cItem?.ID == cartItem.ID);
        if (count == 0)
            throw new DalDoesNotExistException(cartItem.ID, "cartItem");
        listCartItem.Add(cartItem);
        XMLTools.SaveListToXMLSerializer(listCartItem, s_cartItem);
    }


    public void Delete(Func< DO.CartItem, bool> f)
    {
        List<DO.CartItem?> listCartItem = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);
        listCartItem.RemoveAll(cItem => f((DO.CartItem)cItem!));
        XMLTools.SaveListToXMLSerializer(listCartItem, s_cartItem);
    }
}

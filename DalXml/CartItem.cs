using DalApi;
using DO;

namespace Dal;

public class CartItem : ICartItem
{

    static string s_cartItem = @"CartItem";
    /// <summary>
    /// add cart item to cart
    /// </summary>
    /// <param name="cartItem"></param>
    /// <returns></returns>
    /// <exception cref="DO.DalAlreadyExistException"></exception>
    public int Add(DO.CartItem cartItem)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        if (listCartItems.FirstOrDefault(cItem => cItem?.ID == cartItem.ID) != null)
            throw new DO.DalAlreadyExistException(cartItem.ID, "CartItem");
        cartItem.ID = XMLTools.getNextID(@"NextCartItemId");

        listCartItems.Add(cartItem);
        XMLTools.SaveListToXMLSerializer(listCartItems, s_cartItem);
        return cartItem.ID;
    }


    /// <summary>
    /// delete cart item by id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        if (listCartItems.RemoveAll(cItem => cItem?.ID == id) == 0)
            throw new DalDoesNotExistException(id, "cartItem");
        XMLTools.SaveListToXMLSerializer(listCartItems, s_cartItem);
    }


    /// <summary>
    /// get cart item by condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    public DO.CartItem GetByCondition(Func<DO.CartItem?, bool> predicate)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        return listCartItems.FirstOrDefault(predicate) ??
        throw new DalDoesNotExistException("The requested cartItem was not found.");
    }


    /// <summary>
    /// get all cart items
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="DO.DalDoesNotExistException"></exception>
    public IEnumerable<DO.CartItem?> GetAll(Func<DO.CartItem?, bool>? predicate)
    {
        List<DO.CartItem?> listCartItems = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);

        return ((predicate == null) ?
              listCartItems.Select(cItem => cItem).OrderBy(cItem => cItem?.ID) :
              listCartItems.Where(predicate).OrderBy(cItem => cItem?.ID)) ?? throw new DO.DalDoesNotExistException("The requested cartItems were not found"); ;
    }


    /// <summary>
    /// update cart item
    /// </summary>
    /// <param name="cartItem"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(DO.CartItem cartItem)
    {
        List<DO.CartItem?> listCartItem = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);
        int count = listCartItem.RemoveAll(cItem => cItem?.ID == cartItem.ID);
        if (count == 0)
            throw new DalDoesNotExistException(cartItem.ID, "cartItem");
        listCartItem.Add(cartItem);
        XMLTools.SaveListToXMLSerializer(listCartItem, s_cartItem);
    }


    /// <summary>
    /// delete cart items by condition
    /// </summary>
    /// <param name="f"></param>
    public void Delete(Func<DO.CartItem, bool> f)
    {
        List<DO.CartItem?> listCartItem = XMLTools.LoadListFromXMLSerializer<DO.CartItem>(s_cartItem);
        listCartItem.RemoveAll(cItem => f((DO.CartItem)cItem!));
        XMLTools.SaveListToXMLSerializer(listCartItem, s_cartItem);
    }
}

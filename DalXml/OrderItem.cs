using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

///////////////////////////////////////////
//implement IorderItem with XML Serializer
//////////////////////////////////////////
/// <summary>
/// A department that performs operations:
/// adding, updating, repeating and deleting on the orderItem array
/// </summary>

internal class OrderItem : IOrderItem
{
    const string s_orderItem = @"OrderItem";
    const string s_product = @"Product";
    const string s_order = @"Order";
    //XML Serializer

    /// <summary>
    /// get all the order items by condition
    /// </summary>
    /// <returns>an array of all the order items</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? predicate)
    {
        List<DO.OrderItem?> listOrderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);

        return ((predicate == null) ? listOrderItems.Select(ordItem => ordItem).OrderBy(ordItem => ordItem?.ID) :
                                    listOrderItems.Where(predicate).OrderBy(ordItem => ordItem?.ID)) ??
                                    throw new DO.DalDoesNotExistException("The requested order items were not found");
    }


    /// <summary>
    /// get order item by condition
    /// </summary>
    /// <param name="predicate">the order id</param>
    /// <returns>the orderItem</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public DO.OrderItem GetByCondition(Func<DO.OrderItem?, bool> predicate)
    {
        List<DO.OrderItem?> listOrderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);

        var orderItemQuery = listOrderItems.AsParallel(); // Convert the list to a ParallelQuery<OrderItem>

        return orderItemQuery.FirstOrDefault(item => predicate(item)) ??
               throw new DalDoesNotExistException("The requested order item was not found");
    }


    /// <summary>
    /// add a orderitem to the array
    /// </summary>
    /// <param name="orderItem">the new order item </param>
    /// <returns>the id of the new order item</returns>
    /// <exception cref="Exception">if the order id or the product id doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(DO.OrderItem orderItem)
    {

        List<DO.Order?> orderList = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);
        List<DO.Product?> productList = XMLTools.LoadListFromXMLSerializer<DO.Product>(s_product);
        List<DO.OrderItem?> orderItemsList = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);


        if (orderList.FirstOrDefault(order => order?.ID == orderItem.OrderID) == null)
            throw new DalDoesNotExistException(orderItem.OrderID, "order");

        if (productList.FirstOrDefault(product => product?.ID == orderItem.ProductID) == null)
            throw new DalDoesNotExistException(orderItem.ProductID, "product");
        orderItem.ID = (int)((orderItemsList.Last()?.ID) + 1)!;
        orderItemsList.Add(orderItem);
        XMLTools.SaveListToXMLSerializer(orderItemsList, s_orderItem);
        return orderItem.ID;
    }


    /// <summary>
    /// delete a order item
    /// </summary>
    /// <param name="id">the order item id</param>
    /// <exception cref="Exception">if the order item didnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<DO.OrderItem?> orderItemsList = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);

        if (orderItemsList.RemoveAll(ordItem => ordItem?.ID == id) == 0)
            throw new DalDoesNotExistException(id, "orderItem");

        XMLTools.SaveListToXMLSerializer(orderItemsList, s_orderItem);
    }


    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(DO.OrderItem orderItem)
    {
        List<DO.OrderItem?> listOrderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItem);
        int count = listOrderItems.RemoveAll(ord => ord?.ID == orderItem.ID);
        if (count == 0)
            throw new DalDoesNotExistException(orderItem.ID, "orderItem");
        listOrderItems.Add(orderItem);
        XMLTools.SaveListToXMLSerializer(listOrderItems, s_orderItem);
    }
}


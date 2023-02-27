using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DalApi;
using DO;

namespace Dal;

///////////////////////////////////////////
//implement IOrder with XML Serializer
//////////////////////////////////////////
///
////// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order array
/// </summary>
internal class Order : IOrder
{
    const string s_order = @"Order";        //XML Serializer

    /// <summary>
    /// get all the orders
    /// </summary>
    /// <returns>an array of orders</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<DO.Order?> GetAll(Func<DO.Order?, bool>? predicate)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);

        return ((predicate == null) ?
              listOrders.Select(ord => ord).OrderBy(ord => ord?.ID) :
              listOrders.Where(predicate).OrderBy(ord => ord?.ID)) ?? throw new DO.DalDoesNotExistException("The requested orders were not found"); ;
    }


    /// <summary>
    /// get order by condition
    /// </summary>
    /// <param name="predicate">the order id</param>
    /// <returns>the order</returns>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public DO.Order GetByCondition(Func<DO.Order?, bool> predicate)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);

        return listOrders.FirstOrDefault(predicate) ??
        throw new DalDoesNotExistException("The requested order was not found.");
    }


    /// <summary>
    /// add a order to the order array
    /// </summary>
    /// <param name="order">the new order</param>
    /// <returns>the insert new order id</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(DO.Order order)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);

        if (listOrders.FirstOrDefault(ord => ord?.ID == order.ID) != null)
            throw new DO.DalAlreadyExistException(order.ID, "Order");
        order.ID = XMLTools.getNextID(@"NextorderID");

        listOrders.Add(order);
        XMLTools.SaveListToXMLSerializer(listOrders, s_order);
        return order.ID;
    }


    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);

        if (listOrders.RemoveAll(ord => ord?.ID == id) == 0)
            throw new DalDoesNotExistException(id, "order");
        XMLTools.SaveListToXMLSerializer(listOrders, s_order);
    }


    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(DO.Order order)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);
        int count = listOrders.RemoveAll(ord => ord?.ID == order.ID);
        if (count == 0)
            throw new DalDoesNotExistException(order.ID, "order");
        listOrders.Add(order);
        XMLTools.SaveListToXMLSerializer(listOrders, s_order);
    }
}


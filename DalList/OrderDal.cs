using DO;
using System.Drawing;
using static Dal.DataSource;
using DalApi;

namespace Dal;
/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order array
/// </summary>
internal class OrderDal : IOrder
{
    /// <summary>
    /// add a order to the order array
    /// </summary>
    /// <param name="order">the new order</param>
    /// <returns>the insert new order id</returns>
    public int Add(Order order)
    {
        order.ID = IDOrder;
        OrderList.Add(order);
        return order.ID;
    }


    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didnt exist</exception>
    public void Delete(int id)
    {
        //int index = search(id);
        //if (index != -1)
        //{
        //    OrderList.RemoveAt(index);
        //}
        //else
        //    throw new DalDoesNotExistException(id, "order");
       
        int count = OrderList.RemoveAll(order => order?.ID == id);
        if (count == 0)
            throw new DalDoesNotExistException(id, "order");
    }


    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public void Update(Order order)
    {
        //int index = search(order.ID);
        //if (index != -1)
        //    OrderList[index] = order;
        //else
        //    throw new DalDoesNotExistException(order.ID, "order");

        int count = OrderList.RemoveAll(ord => ord?.ID == order.ID);
        if (count == 0)
            throw new DalDoesNotExistException(order.ID, "order");
        OrderList.Add(order);

    }


    /// <summary>
    /// get all the orders
    /// </summary>
    /// <returns>an array of orders</returns>
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? predicate) =>
       (predicate == null ? OrderList.Select(item => item) : OrderList.Where(predicate)) ??
        throw new DO.DalDoesNotExistException("order missing");

    //{
    //List<Order> orders = new List<Order>();
    //foreach (Order order in OrderList)
    //{
    //    orders.Add(order);
    //}

    //}


    /// <summary>
    /// get order by condition
    /// </summary>
    /// <param name="predicate">the order id</param>
    /// <returns>the order</returns>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    public Order GetByCondition(Func<Order?, bool> predicate)=>
        OrderList.FirstOrDefault(predicate!) ??
        throw new DalDoesNotExistException("There is no order that meets the condition");
    //{
    //foreach (Order order in OrderList)
    //{
    //    if (predicate(order))
    //        return order;
    //}
    //throw new DalDoesNotExistException("There is no order that meets the condition");

    //Order? order = OrderList.FirstOrDefault(ord => predicate((Order)ord));
    //if (order==null)
    //{
    //    throw new DalDoesNotExistException("There is no order that meets the condition");
    //}
    //return order;
    //}


    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>
    //private int search(int id)
    //{
    //    for (int i = 0; i < OrderList.Count; i++)
    //    {
    //        if (OrderList[i]?.ID == id)
    //            return i;
    //    }
    //    return -1;
    //}
}



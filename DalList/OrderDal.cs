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
        int index = search(id);
        if (index != -1)
        {
            OrderList.RemoveAt(index);
        }
        else
            throw new DalDoesNotExistException(id, "order");

    }
    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>

    public void Update(Order order)
    {
        int index = search(order.ID);
        if (index != -1)
            OrderList[index] = order;
        else
            throw new DalDoesNotExistException(order.ID, "order");

    }
    /// <summary>
    /// get order by id
    /// </summary>
    /// <param name="id">the order id</param>
    /// <returns>the order</returns>
    /// <exception cref="Exception">if the order doesnt exist</exception>
    /// 


    //************************************************************************************************
    //public Order GetById(int id)
    //{
    //    int index = search(id);
    //    if (index != -1)
    //        return (Order)OrderList[index];
    //    else
    //        throw new DalDoesNotExistException(id, "order");
    //}
    //*********************************************************************************************



    /// <summary>
    /// get all the orders
    /// </summary>
    /// <returns>an array of orders</returns>
        public IEnumerable<Order> GetAll(Func<Order, bool>? predicate)
        {
            List<Order> orders = new List<Order>();
            foreach(Order order in OrderList)
            {
                orders.Add(order);
            }
        }
        return orders;
    }
    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>
    private int search(int id)
    {
        for (int i = 0; i < OrderList.Count; i++)
        {
            if (OrderList[i]?.ID == id)
                return i;
        }
        return -1;
    }
    public Order GetByCondition(Func<Order, bool>? predicate)
    {
    foreach(Order order in OrderList)
        {
            if (predicate(order))
                return order;
        }
        throw new DalDoesNotExistException("There is no order that meets the condition");
    }

}



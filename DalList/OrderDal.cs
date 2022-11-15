using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;
/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the order array
/// </summary>

public class OrderDal
{
    /// <summary>
    /// add a order to the order array
    /// </summary>
    /// <param name="order">the new order</param>
    /// <returns>the insert new order id</returns>
    public int AddOrder(Order order)
    {
        order.ID = IDOrder;
        OrderArray[indexOrder] = order;
        return OrderArray[indexOrder++].ID;
    }
    /// <summary>
    /// delete an order
    /// </summary>
    /// <param name="id">the id of the order</param>
    /// <exception cref="Exception">if the order didnt exist</exception>

    public void DeleteOrder(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            for (int i = index ; i <= indexOrder; i++)
            {
                OrderArray[i] = OrderArray[i+1];
            }
            indexOrder--;
        }
        else
            throw new Exception(" order is not exist");

    }
    /// <summary>
    /// update an order
    /// </summary>
    /// <param name="order">the updated order details</param>
    /// <exception cref="Exception">if the order doesnt exist</exception>

    public void UpdateOrder(Order order)
    {
        int index = search(order.ID);
        if (index != -1)
            OrderArray[index] = order;
        else
            throw new Exception(" order is not exist");

    }
    /// <summary>
    /// get order by id
    /// </summary>
    /// <param name="id">the order id</param>
    /// <returns>the order</returns>
    /// <exception cref="Exception">if the order doesnt exist</exception>

    public Order GetOrder(int id)
    {
        int index = search(id);
        if (index != -1)
            return OrderArray[index];
        else
            throw new Exception(" order is not exist");
    }
    /// <summary>
    /// get all the orders
    /// </summary>
    /// <returns>an array of orders</returns>

    public Order[] GetAllOrder()
    {
        Order[] orders = new Order[indexOrder];
        for (int i = 0; i < indexOrder; i++)
        {
            orders[i] = OrderArray[i];
        }
        return orders;
    }
    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>
    private int search(int id)
    {
        for (int i = 0; i <= indexOrder; i++)
        {
            if (OrderArray[i].ID == id)
                return i;
        }
        return -1;
    }
}



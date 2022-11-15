using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;
/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the orderItem array
/// </summary>
    public class OrderItemDal
{
    /// <summary>
    /// add a orderitem to the array
    /// </summary>
    /// <param name="orderItem">the new order item </param>
    /// <returns>the id of the new order item</returns>
    /// <exception cref="Exception">if the order id or the product id doesnt exist</exception>

    public int AddOrderItem(OrderItem orderItem)
    {
        int i;
        for (i = 0; i <= indexOrder && OrderArray[i].ID != orderItem.OrderID; i++) ;
        if (i == indexOrder+1)
            throw new Exception("order id is not exist");
        for (i = 0; i <= indexProduct && ProductArray[i].ID != orderItem.ProductID; i++) ;
        if (i == indexProduct+1)
            throw new Exception("product id is not exist");
        int id = IDOrderItem;
        orderItem.ID = id;
        OrderItemArray[indexOrderItem++] = orderItem;
        return id;
    }
    /// <summary>
    /// delete a order item
    /// </summary>
    /// <param name="id">the order item id</param>
    /// <exception cref="Exception">if the order item didnt exist</exception>

    public void deleteOrderItem(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            for (int i = index; i <= indexOrderItem; i++)
            {
                OrderItemArray[i] = OrderItemArray[i+1];
            }
            indexOrderItem--;
        }
        else
            throw new Exception(" OrderItem is not exist");

    }
    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>

    public void UpdateOrderItem(OrderItem orderItem)
    {
        int index = search(orderItem.ID);
        if (index != -1)
            OrderItemArray[index] = orderItem;
        else
            throw new Exception(" OrderItem is not exist");

    }
    /// <summary>
    /// get order item by order id and product id
    /// </summary>
    /// <param name="orderId">the order item orderId</param>
    /// <param name="productId">the order item productId</param>
    /// <returns>the order item</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>

    public OrderItem GetOrderItem(int id)
    {
        int index = search(id);
        if (index != -1)
            return OrderItemArray[index];
        else
            throw new Exception(" OrderItem is not exist");
    }
    /// <summary>
    /// get all the order items
    /// </summary>
    /// <returns>an array of all the order items</returns>

    public OrderItem[] GetAllOrderItem()
    {
        OrderItem[] orderItems = new OrderItem[indexOrderItem];
        for (int i = 0; i < indexOrderItem; i++)
        {
            orderItems[i] = OrderItemArray[i];
        }
        return orderItems;
    }
    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>

    private int search(int id)
    {
        for (int i = 0; i <= indexOrderItem; i++)
        {
            if (OrderItemArray[i].ID == id)
                return i;
        }
        return -1;
    }
}

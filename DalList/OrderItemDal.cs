using DO;
using System.Drawing;
using static Dal.DataSource;
using DalApi;

namespace Dal;
/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the orderItem array
/// </summary>
internal class OrderItemDal:IOrderItem
{
    /// <summary>
    /// add a orderitem to the array
    /// </summary>
    /// <param name="orderItem">the new order item </param>
    /// <returns>the id of the new order item</returns>
    /// <exception cref="Exception">if the order id or the product id doesnt exist</exception>

    public int Add(OrderItem orderItem)
    {
        int i;
        for (i = 0; i <OrderList.Count && OrderList[i]?.ID != orderItem.OrderID; i++) ;
        if (i == OrderList.Count)
            throw new DalDoesNotExistException(orderItem.OrderID,"order");
        for (i = 0; i <ProductList.Count && ProductList[i]?.ID != orderItem.ProductID; i++) ;
        if (i == ProductList.Count)
            throw new DalDoesNotExistException(orderItem.ProductID,"product");
        orderItem.ID = IDOrderItem;
        OrderItemList.Add(orderItem);
        return orderItem.ID;
    }
    /// <summary>
    /// delete a order item
    /// </summary>
    /// <param name="id">the order item id</param>
    /// <exception cref="Exception">if the order item didnt exist</exception>

    public void Delete(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            OrderItemList.RemoveAt(index);
        }
        else
            throw new DalDoesNotExistException(id,"orderItem");
    }
    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>

    public void Update(OrderItem orderItem)
    {
        int index = search(orderItem.ID);
        if (index != -1)
            OrderItemList[index] = orderItem;
        else
            throw new DalDoesNotExistException(orderItem.ID,"orderItem");

    }
    /// <summary>
    /// get order item by order id and product id
    /// </summary>
    /// <param name="orderId">the order item orderId</param>
    /// <param name="productId">the order item productId</param>
    /// <returns>the order item</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>

    public OrderItem GetById(int id)
    {
        int index = search(id);
        if (index != -1)
            //??
            return (OrderItem)OrderItemList[index];
        else
            throw new DalDoesNotExistException(id,"OrderItem");
    }
    /// <summary>
    /// get all the order items
    /// </summary>
    /// <returns>an array of all the order items</returns>

    public IEnumerable<OrderItem> GetAll(Func<OrderItem, bool>? predicate)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        foreach (OrderItem orderItem in OrderItemList)
        {
            orderItems.Add(orderItem);
        }
        return orderItems;
    }
    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>

    private int search(int id)
    {
        for (int i = 0; i <OrderItemList.Count; i++)
        {
            if (OrderItemList[i]?.ID == id)
                return i;
        }
        return -1;
    }
        /// <summary>
    /// get order item by order id and product id
    /// </summary>
    /// <param name="orderId">the order item orderId</param>
    /// <param name="productId">the order item productId</param>
    /// <returns>the order item</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>
    public OrderItem GetByOrderIdAndProductId(int orderId , int productId)
    {

        foreach(OrderItem orderItem in OrderItemList)
        {
            if(orderItem.ProductID==productId&&orderItem.OrderID==orderId)
            { 
                return orderItem;
            }
        }
        throw new DalDoesNotExistException(-1, "Order or product");
    }

    /// <summary>
    /// get all order item of a specific order
    /// </summary>
    /// <param name="orderId">the order id</param>
    /// <returns>an array of order items</returns>
    /// <exception cref="Exception">if the order is not exist</exception>
    public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId, Func<OrderItem, bool>? predicate)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        bool flag = false;
     
        foreach(OrderItem item in OrderItemList)
        {
            if(item.OrderID == orderId)
            {
                flag = true;
                orderItems.Add(item);
            }
        }
        if (flag == false)
            throw new DalDoesNotExistException(orderId,"order");
        return orderItems;
    }
}

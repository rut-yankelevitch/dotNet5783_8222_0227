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
        for (i = 0; i <OrderList.Count && OrderList[i].ID != orderItem.OrderID; i++) ;
        if (i == OrderList.Count)
            throw new DalDoesNotExistException("order id is not exist");
        for (i = 0; i <ProductList.Count && ProductList[i].ID != orderItem.ProductID; i++) ;
        if (i == ProductList.Count)
            throw new DalDoesNotExistException("product id is not exist");
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
            throw new DalDoesNotExistException(" OrderItem is not exist");
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
            throw new DalDoesNotExistException(" OrderItem is not exist");

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
            return OrderItemList[index];
        else
            throw new DalDoesNotExistException(" OrderItem is not exist");
    }
    /// <summary>
    /// get all the order items
    /// </summary>
    /// <returns>an array of all the order items</returns>

    public IEnumerable<OrderItem> GetAll()
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
            if (OrderItemList[i].ID == id)
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
        //for (int i = 0; i <OrderItemList.Count; i++)
        //{
        //    if (OrderItemList[i].ProductID == productId && OrderItemList[i].OrderID==orderId)
        //        return OrderItemList[i];
        //}
        foreach(OrderItem orderItem in OrderItemList)
        {
            if(orderItem.ProductID==productId&&orderItem.OrderID==orderId)
            { 
                return orderItem;
            }
        }
        throw new DalDoesNotExistException("Order item is not exist");
    }

    /// <summary>
    /// get all order item of a specific order
    /// </summary>
    /// <param name="orderId">the order id</param>
    /// <returns>an array of order items</returns>
    /// <exception cref="Exception">if the order is not exist</exception>
    //public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId)
    //{
    //    List<OrderItem> orderItems = new List<OrderItem>();
    //    int index = 0;
    //    for (int i = (OrderItemList.Count)-1; i >=0; i++)
    //    {
    //        if (OrderItem[i].OrderID == orderId)
    //            orderItems[index++] = OrderItemArray[i];
    //    }
    //    OrderItem[] orderItems2 = new OrderItem[index];

    //    for (int i = 0; i < index; i++)
    //    {

    //            orderItems2[i] = orderItems[i];
    //    }
    //    if (index==0)
    //        throw new DalDoesNotExistException("Order item is not exist");
    //    return orderItems2;
    //}
    public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        bool flag = false;
        //for (int i = (OrderItemList.Count) - 1; i >= 0; i--)
        //{
        //    if (OrderItemList[i].OrderID == orderId)
        //    {
        //        flag = true;
        //        orderItems.Add(OrderItemList[i]);
        //    }
        //}
        foreach(OrderItem item in OrderItemList)
        {
            if(item.OrderID == orderId)
            {
                flag = true;
                orderItems.Add(item);
            }
        }
        if (flag == false)
            throw new DalDoesNotExistException("Order item is not exist");
        return orderItems;
    }
}

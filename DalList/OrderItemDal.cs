using DO;
using static Dal.DataSource;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal;
/// <summary>
/// A department that performs operations:
/// adding, updating, repeating and deleting on the orderItem array
/// </summary>
internal class OrderItemDal : IOrderItem
{
    /// <summary>
    /// add a orderitem to the array
    /// </summary>
    /// <param name="orderItem">the new order item </param>
    /// <returns>the id of the new order item</returns>
    /// <exception cref="Exception">if the order id or the product id doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(OrderItem orderItem)
    {
        if (OrderList.FirstOrDefault(order => order?.ID == orderItem.OrderID) == null)
            throw new DalDoesNotExistException(orderItem.OrderID, "order");

        if (ProductList.FirstOrDefault(product => product?.ID == orderItem.ProductID) == null)
            throw new DalDoesNotExistException(orderItem.ProductID, "product");

        orderItem.ID = IDOrderItem;
        OrderItemList.Add(orderItem);
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
        int count = OrderItemList.RemoveAll(ordItem => ordItem?.ID == id);
        if (count == 0)
            throw new DalDoesNotExistException(id, "orderItem");
    }


    /// <summary>
    /// update an order item
    /// </summary>
    /// <param name="orderItem">the new details of the order item</param>
    /// <exception cref="Exception">if the order cdoesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(OrderItem orderItem)
    {
        int count = OrderItemList.RemoveAll(ord => ord?.ID == orderItem.ID);
        if (count == 0)
            throw new DalDoesNotExistException(orderItem.ID, "orderItem");
        OrderItemList.Add(orderItem);
    }


    /// <summary>
    /// get all the order items by condition
    /// </summary>
    /// <returns>an array of all the order items</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? predicate = null) =>
        (predicate == null ? OrderItemList.Select(item => item)
                           : OrderItemList.Where(predicate!)) ??
                             throw new DO.DalDoesNotExistException("The requested order items were not found");


    /// <summary>
    /// get order by id
    /// </summary>
    /// <param name="predicate">the order id</param>
    /// <returns>the orderItem</returns>
    /// <exception cref="Exception">if the order item doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem GetByCondition(Func<OrderItem?, bool> predicate) =>
        OrderItemList.FirstOrDefault(predicate) ??
        throw new DalDoesNotExistException("The requested order item was not found");
}

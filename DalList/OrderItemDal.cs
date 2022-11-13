using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;
    public class OrderItemDal
{
    public int AddOrderItem(OrderItem orderItem)
    {
        int i;
        for (i = 0; i < Config.indexOrder && OrderArray[i].ID != orderItem.OrderID; i++) ;
        if (i == Config.indexOrder+1)
            throw new Exception("order id is not exist");
        for (i = 0; i < Config.indexProduct && ProductArray[i].ID != orderItem.OrderID; i++) ;
        if (i == Config.indexProduct+1)
            throw new Exception("product id is not exist");
        int id = Config.IDOrderItem;
        orderItem.ID = id;
        OrderItemArray[Config.indexOrderItem++] = orderItem;
        return id;
    }
    public void deleteOrderItem(int id)
    {
        int x = search(id);
        if (x != -1)
        {
            for (int i = x ; i <= Config.indexOrderItem; i++)
            {
                OrderItemArray[i] = OrderItemArray[i+1];
            }
            Config.indexOrderItem--;
        }
        else
            throw new Exception(" OrderItem is not exist");

    }
    public void UpdateOrderItem(OrderItem orderItem)
    {
        int x = search(orderItem.ID);
        if (x != -1)
            OrderItemArray[x] = orderItem;
        else
            throw new Exception(" OrderItem is not exist");

    }
    public OrderItem GetOrderItem(int id)
    {
        int x = search(id);
        if (x != -1)
            return OrderItemArray[x];
        else
            throw new Exception(" OrderItem is not exist");
    }
    public OrderItem[] GetAllOrderItem()
    {
        OrderItem[] orderItems = new OrderItem[Config.indexOrderItem];
        for (int i = 0; i < Config.indexOrderItem; i++)
        {
            orderItems[i] = OrderItemArray[i];
        }
        return orderItems;
    }
    private int search(int id)
    {
        for (int i = 0; i <= Config.indexOrderItem; i++)
        {
            if (OrderItemArray[i].ID == id)
                return i;
        }
        return -1;
    }
}

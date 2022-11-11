using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;
{
    public class OrderItemDal
{
    public int AddOrderItem(OrderItem o)
    {
        //code of order and product?
        o.ID = Config.IDOrderItem;
        OrderItemArray[Config.indexOrderItem] = o;
        return OrderItemArray[Config.indexOrderItem++].ID;
    }
    public void deleteOrderItem(int id)
    {
        int x = search(id);
        if (x != -1)
        {
            for (int i = x + 1; i < OrderItemArray.lengthe; i++)
            {
                OrderItemArray[x - 1] = OrderItemArray[x];
            }
        }
        else
            throw new Exception(" OrderItem is not exist");

    }
    public void UpdateOrderItem(OrderItem o)
    {
        int x = search(o.ID);
        if (x != -1)
            OrderItemArray[x] = o;
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
        OrderItem[] o = new OrderItem[OrderItemtArray.Length];
        for (int i = 0; i < OrderItemArray.Length; i++)
        {
            o[i] = OrderItemArray[i];
        }
        return o;
    }
    private int search(int id)
    {
        for (int i = 0; i < OrderItemtArray.Length; i++)
        {
            if (OrderItemArray[i].ID = id)
                return i;
        }
        return -1;
    }
}
}
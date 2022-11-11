using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;
{
    public class OrderDal
{
    public int AddOrder(Order o)
    {
        o.ID = Config.IDOrder;
        OrderArray[Config.indexOrder] = o;
        return OrderArray[Config.indexOrder++].ID;
    }
    public void deleteOrder(int id)
    {
        int x = search(id);
        if (x != -1)
        {
            for (int i = x + 1; i < OrderArray.lengthe; i++)
            {
                OrderArray[x - 1] = OrderArray[x];
            }
        }
        else
            throw new Exception(" order is not exist");

    }
    public void UpdateOrder(Order o)
    {
        int x = search(o.ID);
        if (x != -1)
            OrderArray[x] = o;
        else
            throw new Exception(" order is not exist");

    }
    public Order GetOrder(int id)
    {
        int x = search(id);
        if (x != -1)
            return OrderArray[x];
        else
            throw new Exception(" order is not exist");
    }
    public Order[] GetAllOrder()
    {
        Order[] o = new Order[OrderArray.Length];
        for (int i = 0; i < OrderArray.Length; i++)
        {
            o[i] = OrderArray[i];
        }
        return o;
    }

    private int search(int id)
    {
        for (int i = 0; i < OrderArray.Length; i++)
        {
            if (OrderArray[i].ID = id)
                return i;
        }
        return -1;
    }
}
}


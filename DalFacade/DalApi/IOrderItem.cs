using DO;

namespace DalApi;
/// <summary>
/// An interface that implements the icrud interface
/// </summary>
public interface IOrderItem :ICrud <OrderItem>
    {
        public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId);
        public OrderItem GetByOrderIdAndProductId(int orderId, int productId);

    }


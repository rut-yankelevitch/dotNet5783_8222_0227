using DO;

namespace DalApi;

    public interface IOrderItem :ICrud <OrderItem>
    {
        //public int Add(OrderItem obj);
        //public void Update(OrderItem obj);
        //public void Delete(int id);
        //public OrderItem GetById(int id);
        //public IEnumerable<OrderItem> GetAll();
        public IEnumerable<OrderItem> GetAllItemsByOrderId(int orderId);
        public OrderItem GetByOrderIdAndProductId(int orderId, int productId);

    }


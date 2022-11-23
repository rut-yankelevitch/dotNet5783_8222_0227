using Dal;
using DalApi;

namespace DalList
{
    sealed public class DalList : IDal
    {
        public IOrder Order => new OrderDal();
        public IProduct Product => new ProductDal();
        public IOrderItem OrderItem => new OrderItemDal();
    }
}
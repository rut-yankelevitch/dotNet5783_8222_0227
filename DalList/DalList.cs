using Dal;
using DalApi;

namespace DalList
{
    /// <summary>
    /// A class that implements the idal interface and returns all the data entities
    /// </summary>
    sealed public class DalList : IDal
    {
        public IOrder Order => new OrderDal();
        public IProduct Product => new ProductDal();
        public IOrderItem OrderItem => new OrderItemDal();
    }
}
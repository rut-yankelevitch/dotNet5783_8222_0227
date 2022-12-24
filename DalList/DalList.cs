using Dal;
using DalApi;

namespace Dal
{
    /// <summary>
    /// A class that implements the idal interface and returns all the data entities
    /// </summary>
    sealed internal class DalList : IDal
    {
        public IOrder Order => new OrderDal();
        public IProduct Product => new ProductDal();
        public IOrderItem OrderItem => new OrderItemDal();
    }
}
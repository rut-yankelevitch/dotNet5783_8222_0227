using Dal;
using DalApi;

namespace Dal
{
    /// <summary>
    /// A class that implements the idal interface and returns all the data entities
    /// </summary>
    sealed internal class DalList : IDal
    {
        public IOrder Order { get;} = new Dal.OrderDal();
        public IProduct Product { get;}= new Dal.ProductDal();
        public IOrderItem OrderItem {get;} new Dal.OrderItemDal();

    }
}
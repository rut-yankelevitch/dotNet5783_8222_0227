using DalApi;

namespace Dal
{
    /// <summary>
    /// A class that implements the idal interface and returns all the data entities
    /// </summary>
    sealed internal class DalList : IDal
    {
        private DalList() { }
        private static readonly object key = new();
        private static DalList? instance;
        public static IDal Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (key)
                    {
                        if (instance == null)
                            instance = new();
                    }
                }
                return instance;
            }    
        }
        public IOrder Order { get;} = new Dal.OrderDal();
        public IProduct Product { get;}= new Dal.ProductDal();
        public IOrderItem OrderItem {get;}= new Dal.OrderItemDal();
        public IUser User { get; } = new Dal.UserDal();
        public ICartItem CartItem { get; } = new Dal.CartItemDal();

    }
}

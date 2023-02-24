using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal;

sealed public class DalXml : IDal
{
    //static private Lazy<DalXml>? instance = null;
    private DalXml() { }
    private static readonly object key = new();
    private static DalXml? instance;
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

    //public static IDal Instance { get => GetInstance(); }

    public IProduct Product { get; } = new Dal.Product();
    public IOrder Order { get; } = new Dal.Order();
    public IOrderItem OrderItem { get; } = new Dal.OrderItem();
    public IUser User { get; } = new Dal.User();
    public ICartItem CartItem { get; } = new Dal.CartItem();


    //public static DalXml GetInstance()
    //{
    //    lock (instance ??= new Lazy<DalXml>(() => new DalXml()))
    //    {
    //        return instance.Value;
    //    }
    //}
}
//using DalApi;
//using DO;
//using System.Xml.Linq;

//namespace Dal;

//    /// <summary>
//    /// A class that implements the idal interface and returns all the data entities
//    /// </summary>
//    sealed internal class DalXml : IDal
//    {
//        private DalXml() { }
//        private static readonly object key = new();
//        private static DalXml? instance;
//        public static IDal Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    lock (key)
//                    {
//                        if (instance == null)
//                            instance = new();
//                    }
//                }
//                return instance;
//            }
//        }
//        public IOrder Order { get; } = new Dal.Order();
//        public IProduct Product { get; } = new Dal.Product();
//        public IOrderItem OrderItem { get; } = new Dal.OrderItem();
//}
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

    //public static DalXml GetInstance()
    //{
    //    lock (instance ??= new Lazy<DalXml>(() => new DalXml()))
    //    {
    //        return instance.Value;
    //    }
    //}
}
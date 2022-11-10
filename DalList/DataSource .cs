
namespace DalList;
internal static class DataSource
{
    static readonly Random random = new Random();

    internal static class Confige
    {
        internal static int[] orderItemArray = new int[200];
        internal static int[] orderArray = new int[100];
        internal static int[] productArray = new int[50];
        internal static int indexOrderItem = 0;
        internal static int indexOrder = 0;
        internal static int indexProduct = 0;

        private static int IDProduct = 99999;
        internal static int IDProduct { get => ++IDProduct};
        private static int IDOrder = -1;
        internal static int IDOrder { get => ++IDOrder};
        private static int IDOrderItem = -1;
        internal static int IDOrderItem { get => ++IDOrderItem };




    }
    static DataSource() => s_Initialize();

    private static s_Initilize()
    {
        initProductArray();
        initOrderArray();
        initOrderItemArray();

    }
    private static initProductArray()
    {
        for (int i = 0; i < 12; i++)
        {
            indexProduct++;
            productArray[i] = new Product { ID = IDProduct, Name = 'a', Category = '', Price = , InStock = }
        }
    }
    private static initOrderArray()
    {
        for (int i = 0; i < 21; i++)
        {
            indexOrder++;
            orderArray = new Order { ID = IDProduct, CustomerName = '', CustomerEmail='', CustomerAdress='' OrderDate = Da, ShipDate = i + 1, DeliveryrDate =  }
        }

    }

    private static initOrderItemArray()
    {
        for (int i = 0; i < 45; i++)
        {
            indexOrderItem++;
            orderItemArray = new OrderItem { ProductID = , OrderID = , Price = , Amount =   }
        }
    }

}



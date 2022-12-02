using DO;
using System.Globalization;
namespace Dal;
static class DataSource
{
    private static readonly Random randNum = new Random();

    private const int startOrderId = 0;
    private const int startOrderItemId = 0;

    private static int idOrder = startOrderId;
        internal static int IDOrder { get => ++idOrder; }
        private static int idOrderItem = startOrderItemId;
        internal static int IDOrderItem { get => ++idOrderItem; }


    internal static List<Product> ProductList = new List<Product>();
    internal static List<Order> OrderList = new List<Order>();
    internal static List<OrderItem> OrderItemList = new List<OrderItem>();



    static DataSource() => s_Initilize();



    private static void s_Initilize()
    {
        initProductArray();
        initOrderArray();
        initOrderItemArray();

    }
    private static void initProductArray()
   {

        string[] nameArray = new string[] { " Samsung Dyson", "Electrolux Dyson", "Bosch Dyson", "SAMURAI mixer", "Bosch toaster", "elextrolux toaster", "Bosch iron", "TEFAL iron", "elextrolux blender", "braun blender", "Nespresso coffee machine", " Hemilton coffee machine" };
        Category[] category = new Category[] { Category.VACUUM_CLEANER, Category.VACUUM_CLEANER, Category.MIXER, Category.COFFEE_MACHINE, Category.COFFEE_MACHINE, Category.COFFEE_MACHINE, Category.TOASTER, Category.TOASTER, Category.IRON, Category.IRON, Category.BLENDER, Category.BLENDER };
         int[] productID = new int[]
{
            6665581,858565,747541,125478,852963,741598,458963,458796,745698,147854,147985,364152
};
        for (int i = 0; i < 12; i++)
        {
            ProductList.Add(new Product { ID = productID[i], Name = nameArray[i], Category = category[i], Price = i + 200, InStock = i * randNum.Next(20) });
        }
    
    }

    private static void initOrderArray()
    {
        string[] nameArray = new string[] { "Ruti Yankelevitch", "Racheli Shafran", "Chani Alter", " Adi Bialostotzki", "Tamar Druk", "Tchelet Yshay", "Shira Mark", "Shira Nusbacher", "Chaya Fogel", "Heni Fridman", "Levana Coen", "Lea Yaakobowitch", "Rachel Diskind", "Chany Kozlic", "Chavi Shapira", "Chaya Vasertzug", "Batya Grintzaig", "Yafi Yrenshtein", "Malci Slomiansci" };
        string[] EmailArray = new string[] { "RutiY", "RacheliS", "ChaniA", "AdiB", "TamarD", "TcheletY", "ShiraM", "ShiraN", "ChayaF", "HeniF", "LevanaC", "LeaY", "RachelD", "ChanyK", "ChaviS", "ChayaV", "BatyaG", "YafiY", "MalciS" };
        string[] CitiArray = new string[] {"Geula 14 Haifa", "Michel 28 Haifa", "Chazon Ish 13 Beit Shemesh", "Rabbi Akiva 70 Bnei Brak", "Hakalanit 14 herzelia", "Haturim 8 Jerusalem",
            "Malchei Israel 40 Jerusalem", "miron 20 Bnei Brak", "Hameiri 4 Jerusalem", "begin 72 Naaria",
            "hagefen 18 Kfar Chabad", "hanurit 6 Ashdod", "Shamgar 58 Jerusalem", "Pnei Menachem 13 Petach Tikwa",
            "Hadekel 16 Tel Aviv"};
        for (int i = 0; i < 22; i++)
        {
            DateTime helpE;
            TimeSpan helpC;
            do
            {
                helpE = new DateTime(randNum.Next(2021, 2022), randNum.Next(1, 13), randNum.Next(1, 29), randNum.Next(24), randNum.Next(60), randNum.Next(60));
            }
            while (helpE >= DateTime.Now);
            DateTime OrderDate1 = helpE;
            DateTime ShipDate1 = new DateTime ();
            DateTime DeliveryrDate1 = new DateTime();
            if (i < 16)
            {
                helpC = new TimeSpan(randNum.Next(10,370), 0, 0, 0, 0);
                 ShipDate1 = OrderDate1 + helpC;

            }
            if (i < 8)
            {
                helpC = new TimeSpan(randNum.Next(1,10), 0, 0, 0, 0);
                 DeliveryrDate1 = ShipDate1 + helpC;
            }
            
            OrderList.Add(new Order { ID = IDOrder, CustomerName = nameArray[i % 15], CustomerEmail = EmailArray[i % 15], CustomerAdress = CitiArray[i % 15] , OrderDate = OrderDate1 , ShipDate = ShipDate1 , DeliveryrDate = DeliveryrDate1 });
          }       
        }
    private static void initOrderItemArray()
    {
        foreach(Order o in OrderList)
        {
            int randA = randNum.Next(12);
            double price = new double();
            double price2= new double();
            foreach (Product p in ProductList)
            {
                if (p.ID == ProductList[randA].ID)
                {
                    price = p.Price;
                    break;
                }
            }
            int randB;
            do
                randB = randNum.Next(10);
            while (randA == randB);
            foreach (Product p in ProductList)
            {
                if (p.ID == ProductList[randB].ID)
                {
                    price2 = p.Price;
                    break;
                }
            }
            OrderItemList.Add(new OrderItem { ID = IDOrderItem, OrderID = o.ID, ProductID = ProductList[randA].ID, Price = price , Amount = randNum.Next(1, 5) }) ;
            OrderItemList.Add(new OrderItem { ID = IDOrderItem, OrderID = o.ID, ProductID = ProductList[randB].ID, Price = price2, Amount = randNum.Next(1, 5) });

        }

    }
    }



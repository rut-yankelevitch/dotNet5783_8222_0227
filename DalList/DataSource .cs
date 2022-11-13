using DO;
using System.Globalization;
namespace Dal;
static class DataSource
{
    private static readonly Random randNum = new Random();
    internal static class Config
    {
        private const int startOrderId = 0;
        private const int startOrderItemId = 0;
        internal static int indexOrderItem  { get; set; }= 0;
        internal static int indexOrder { get; set; }= 0;
        internal static int indexProduct  { get; set; }= 0;

        private static int idOrder = startOrderId;
        internal static int IDOrder { get => ++idOrder; }
        private static int idOrderItem = startOrderItemId;
        internal static int IDOrderItem { get => ++idOrderItem; }
    }

    internal static Product[] ProductArray = new Product[50];
    internal static Order[] OrderArray = new Order[100];
    internal static OrderItem[] OrderItemArray = new OrderItem[200];



     static DataSource() => s_Initilize();



    private static void s_Initilize()
    {
        initProductArray();
        initOrderArray();
        initOrderItemArray();

    }
    private static void initProductArray()
   {
        int[] productID = new int[]
      {
            6665581,858565,747541,125478,852963,741598,458963,458796,745698,147854,147985,364152
      };
        string[] nameArray = new string[] { " Samsung Dyson", "Electrolux Dyson", "Bosch Dyson", "SAMURAI mixer", "Bosch toaster", "elextrolux toaster", "Bosch iron", "TEFAL iron", "elextrolux blender", "braun blender", "Nespresso coffee machine", " Hemilton coffee machine" };
        Category[] category = new Category[] { Category.VACUUM_CLEANER, Category.VACUUM_CLEANER, Category.MIXER, Category.COFFEE_MACHINE, Category.COFFEE_MACHINE, Category.COFFEE_MACHINE, Category.TOASTER, Category.TOASTER, Category.IRON, Category.IRON, Category.BLENDER, Category.BLENDER };

        for (int i = 0; i < 12; i++)
        {
            Config.indexProduct++;
            ProductArray[i] = new Product { ID = productID[i], Name = nameArray[i], Category = category[i], Price = i + 200, InStock = i * randNum.Next(20) };
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
            Config.indexOrder++;
            OrderArray[i] = new Order { ID = Config.IDOrder, CustomerName = nameArray[i % 15], CustomerEmail = EmailArray[i % 15], CustomerAdress = CitiArray[i % 15] };
            DateTime helpE;
            do
            {
                helpE = new DateTime(randNum.Next(2000, 2022), randNum.Next(1,13), randNum.Next(1,29), randNum.Next(1,24), randNum.Next(1,60), randNum.Next(1,60));
            }
            while (helpE >= DateTime.Now);
            OrderArray[i].OrderDate = helpE;
            TimeSpan helpC;
            if (i < 16)
            {
                 helpC = new TimeSpan(randNum.Next(1, 10), 0, 0, 0, 0);
                OrderArray[i].ShipDate = OrderArray[i].OrderDate + helpC;

            }
            if (i < 8)
            {
                 helpC = new TimeSpan(randNum.Next(1, 10), 0, 0, 0, 0);
                OrderArray[i].DeliveryrDate = OrderArray[i].OrderDate + helpC;
            }
          }       
        }
    private static void initOrderItemArray()
    {
        
            for (int i = 0; i < 39; i+=2)
            {
                Config.indexOrderItem+=2;
                int randA = randNum.Next(12);
                OrderItemArray[i].ID = Config.IDOrderItem;
                OrderItemArray[i].OrderID = OrderArray[i%21].ID;
                OrderItemArray[i].ProductID = ProductArray[randA].ID;
                OrderItemArray[i].Price = ProductArray[randA].Price;
                OrderItemArray[i].Amount = randNum.Next(1,5);
                int randB;
                do
                randB = randNum.Next(10);
                while (randA == randB);
                OrderItemArray[i +1].ID = Config.IDOrderItem;
                OrderItemArray[i +1].OrderID = OrderArray[i%21].ID;
                OrderItemArray[i +1].ProductID = ProductArray[randB].ID;
                OrderItemArray[i +1].Price = ProductArray[randB].Price;
                OrderItemArray[i +1].Amount = randNum.Next(1,5);
            }
            }
    }



using DO;
using System.Globalization;
namespace Dal;
 static class DataSource
{
    private static readonly Random randNum = new Random();

    internal static class Confige
    {
        private const int startOrderId = -1;
        private const int startOrderItemId = -1;
        private const int startProductId = 99999;
        internal static int indexOrderItem = { get; set; }= 0;
        internal static int indexOrder = { get; set; }= 0;
        internal static int indexProduct = { get; set; }= 0;

        private static int IDProduct = startProductId;
        internal static int IDProduct { get => ++IDProduct};
        private static int IDOrder = startOrderId;
        internal static int IDOrder { get => ++IDOrder};
        private static int IDOrderItem = startOrderItemId;
        internal static int IDOrderItem { get => ++IDOrderItem };
    }

    internal static Product[] ProductArray = new Product[50];
    internal static Order[] OrderArray = new Order[100];
    internal static OrderItem[] OrderItemArray = new OrderItem[200];


    static DataSource() => s_Initialize();

    private static void s_Initilize()
    {
        initProductArray();
        initOrderArray();
        initOrderItemArray();

    }
    enum Cat { VACUUM_CLEANER, COFFEE_MACHINE, TOASTER, IRON, MIXER, BLENDER }

    private static void initProductArray()
   {
    string[] nameArray =[" Samsung Dyson", "Electrolux Dyson", "Bosch Dyson", "SAMURAI mixer", "Bosch toaster", "elextrolux toaster", "Bosch iron", "TEFAL iron", "elextrolux blender", "braun blender", "Nespresso coffee machine", " Hemilton coffee machine"];
    Cat[] category = new Cat[] { VACUUM_CLEANER, VACUUM_CLEANER, MIXER, COFFEE_MACHINE , COFFEE_MACHINE, COFFEE_MACHINE, TOASTER, TOASTER, IRON, IRON, BLENDER, BLENDER };

        for (int i = 0; i < 12; i++)
        {
            Config.indexProduct++;
            ProductArray[i] = new Product { ID = Config.IDProduct, Name = nameArray[i], Category = category[i], Price = i+200, InStock = i*random.Next(20)] }
        }
    
    }

    private static initOrderArray()
    {
        string [] nameArray =["Ruti Yankelevitch", "Racheli Shafran", "Chani Alter", " Adi Bialostotzki", "Tamar Druk", "Tchelet Yshay", "Shira Mark", "Shira Nusbacher", "Chaya Fogel", "Heni Fridman", "Levana Coen", "Lea Yaakobowitch", "Rachel Diskind", "Chany Kozlic", "Chavi Shapira", "Chaya Vasertzug", "Batya Grintzaig", "Yafi Yrenshtein", "Malci Slomiansci"];
        string[] EmailArray=["RutiY", "RacheliS", "ChaniA", "AdiB", "TamarD", "TcheletY", "ShiraM", "ShiraN", "ChayaF", "HeniF", "LevanaC", "LeaY", "RachelD", "ChanyK", "ChaviS", "ChayaV", "BatyaG", "YafiY", "MalciS"]
        string[] CitiArray =["Geula 14 Haifa", "Michel 28 Haifa", "Chazon Ish 13 Beit Shemesh", "Rabbi Akiva 70 Bnei Brak", "Hakalanit 14 herzelia", "Haturim 8 Jerusalem",
            "Malchei Israel 40 Jerusalem", "miron 20 Bnei Brak", "Hameiri 4 Jerusalem", "begin 72 Naaria",
            "hagefen 18 Kfar Chabad", "hanurit 6 Ashdod", "Shamgar 58 Jerusalem", "Pnei Menachem 13 Petach Tikwa",
            "Hadekel 16 Tel Aviv"];
        for (int i = 0; i < 22; i++)
        {
            DateTime orderDate = new DataTime;
        //לסדר תאריכים
        Config.indexOrder++;
            OrderArray[i] = new Order { ID = Config.IDOrder, CustomerName = nameArray[i%15], CustomerEmail= EmailArray[i%15], CustomerAdress= CitiArray[i%15], OrderDate= orderDate , ShipDate = , DeliveryrDate =  }
        }

    }

    private static initOrderItemArray()
    {
    {
        for (int i = 0; i < 45; i++)
        {
            Random ran = new Random();
            int x = ran.Next(1, 6);
            Config.indexOrderItem++;
            OrderItemArray = new OrderItem { ID= Config.IDOrderItem, ProductID = ProductArray[i/4].ID , OrderID = OrderArray[i/2].ID , Price = ProductArray[i/4].Price, Amount = x  }
        }
    }

}



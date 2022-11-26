// See https://aka.ms/new-console-template for more information
using BO;
using Dal;
//using DalList;
namespace BLTest
{
    /// <summary>
    /// Enum of the main management input options
    /// </summary>
    enum ManagementProgram { EXIT, PRODUCT, ORDER, CART }
    /// <summary>
    /// Enum of the secondary menu input options
    /// </summary>
    enum ProductOptions { ADD = 1, GET_FOR_MANAGER,GET_FOR_CUSTOMER, GET_ALL, UPDATE, DELETE }
    enum OrderOptions { ADD = 1, UPDATE, MAKE_ORDER }
    enum CartOptions { GET=1, GET_ALL, UPDATE_SHIPPING_ORDER,UPDATE_DELIVERY_ORDER , ORDER_TRACKING,BONUS }


    /// <summary>
    /// The class of the main program
    /// </summary>

    public class Program
    {
        /// <summary>
        /// Instances of the data access classes
        /// </summary>

        static private DalList.DalList dalList = new DalList.DalList();
        //static private OrderDal orderDal = new OrderDal();
        //static private ProductDal productDal = new ProductDal();
        //static private OrderItemDal orderItemDal = new OrderItemDal();

        static string readString;
        static int readInt;
        static double readDouble;

        /// <summary>
        ///A static private function, that is called by the main program
        ///when the user requests to perform operations on  product 
        /// </summary>
        private static void ProductManagement()
        {
            Console.WriteLine("Product menu: \n 1- add \n 2- get for manager \n 3- get for customer \n 4- get all  \n 5- update \n 6- delete");
            readString = Console.ReadLine();
            ProductOptions productOptions = (ProductOptions)int.Parse(readString);
            Product product = new Product();
            try
            {
                switch (productOptions)
                {
                    case ProductOptions.ADD:
                        break;
                    case ProductOptions.GET_FOR_MANAGER:
                        break;
                    case ProductOptions.GET_FOR_CUSTOMER:
                        break;
                    case ProductOptions.GET_ALL:
                        break;
                    case ProductOptions.UPDATE:
                        break;
                    case ProductOptions.DELETE:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        /// <summary>
        /// A static private function, that is called by the main program
        ///when the user requests to perform operations on  order 
        /// </summary>

        private static void OrdersManagement()
        {
            Console.WriteLine("Order menu: \n 1- add \n 2- update  \n 3- nake order ");
            readString = Console.ReadLine();
            OrderOptions orderOptions = (OrderOptions)int.Parse(readString);
            Order order = new Order();
            try
            {
                switch (orderOptions)
                {
                    case OrderOptions.ADD:
                        break;
                    case OrderOptions.UPDATE:
                        break;
                    case OrderOptions.MAKE_ORDER:
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// A static private function, that is called by the main program
        ///when the user requests to perform operations on cart 
        /// </summary>

        private static void CartManagement()
        {
            Console.WriteLine("Cart menu: \n 1- get \n 2- get all \n 3- update shipping order \n 4- update delivery order  \n 5- order tracking \n 6- bonus");
            readString = Console.ReadLine();
            CartOptions cartOptions = (CartOptions)int.Parse(readString);
            Order order = new Order();
            try
            {
                switch (cartOptions)
                {
                    case CartOptions.GET:
                        break;
                    case CartOptions.GET_ALL:
                        break;
                    case CartOptions.UPDATE_SHIPPING_ORDER:
                        break;
                    case CartOptions.UPDATE_DELIVERY_ORDER:
                        break;
                    case CartOptions.ORDER_TRACKING:
                        break;
                    case CartOptions.BONUS:
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
            /// <summary>
            /// The main program
            /// </summary>
            /// <param name="args"></param>
            public static void Main(string[] args)
        {
           

            ManagementProgram managementProgram;
            Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2- order\n 3- order item .");
            string choice = Console.ReadLine();
            ManagementProgram.TryParse(choice, out managementProgram);

            while (managementProgram != ManagementProgram.EXIT)
            {
                switch (managementProgram)
	{
		
                 case ManagementProgram.PRODUCT:
                        ProductManagement();
                  break;
                 case ManagementProgram.ORDER:
                        OrdersManagement();
                  break;
                 case ManagementProgram.CART:
                        CartManagement();
                  break;
                 default:
                  break;
	}

                
                Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2- order \n 3- order item .");
                choice = Console.ReadLine();
                ManagementProgram.TryParse(choice, out managementProgram);

            }
        }
    }
}
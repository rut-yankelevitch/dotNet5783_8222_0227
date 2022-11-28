// See https://aka.ms/new-console-template for more information
using BlApi;
using BlImplementation;
using BO;

namespace BLTest
{
    /// <summary>
    /// Enum of the main management input options
    /// </summary>
    enum ManagementProgram { EXIT, PRODUCT, ORDER, CART }
    /// <summary>
    /// Enum of the secondary menu input options
    /// </summary>
    enum ProductOptions { ADD = 1, GET_FOR_MANAGER,GET_FOR_CUSTOMER, GET_LIST_FOR_MANAGER, GET_LIST_FOR_CUSTOMER, UPDATE, DELETE }
    enum OrderOptions { GET=1, GET_ALL, UPDATE_SHIPPING_ORDER,UPDATE_DELIVERY_ORDER , ORDER_TRACKING,BONUS }
    enum CartOptions { ADD = 1, UPDATE, MAKE_ORDER }


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
        static int readInt1;
        static double readDouble;
        static private IBl iBl = new Bl();
        static private Cart currentCart = new Cart();



        /// <summary>
        ///A static private function, that is called by the main program
        ///when the user requests to perform operations on  product 
        /// </summary>
        private static void ProductManagement()
        {
            Console.WriteLine("Product menu: \n 1- add \n 2- get for manager \n 3- get for customer \n 4- get all  \n 5- update \n 6- delete");
            readString = Console.ReadLine();
            ProductOptions productOptions = (ProductOptions)int.Parse(readString);
            BO.Product product = new BO.Product();
            BO.ProductItem productItem = new BO.ProductItem();
            IEnumerable<BO.ProductForList> productsForList= new List<BO.ProductForList>();
            IEnumerable<BO.ProductItem> productsItem = new List<BO.ProductItem>();


            try
            {
                switch (productOptions)
                {
                    case ProductOptions.ADD:
                        Console.WriteLine("enter product details:\n product id, product name, category , price ,amount");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product.ID = readInt;
                        product.Name = Console.ReadLine();
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product.Category = (Category)readInt;
                        readString = Console.ReadLine();
                        double.TryParse(readString, out readDouble);
                        product.Price = readDouble;
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product.InStock = readInt;
                        product = iBl.Product.AddProduct(product);
                        Console.WriteLine(product);
                        break;
                    case ProductOptions.GET_FOR_MANAGER:
                        Console.WriteLine("Enter id product:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product = iBl.Product.GetProductByIdForManager(readInt);
                        Console.WriteLine(product);
                        break;
                    case ProductOptions.GET_FOR_CUSTOMER:
                        Console.WriteLine("Enter id product:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        productItem = iBl.Product.GetProductByIdForCustomer(readInt);
                        Console.WriteLine(productItem);
                        break;
                    case ProductOptions.GET_LIST_FOR_MANAGER:
                        productsForList= iBl.Product.GetProductListForManager();
                        foreach(ProductForList productForList in productsForList) 
                            Console.WriteLine(productForList);
                        break;
                    case ProductOptions.GET_LIST_FOR_CUSTOMER:
                        productsItem = iBl.Product.GetProductListForCustomer();
                        foreach (ProductItem item in productsItem)
                            Console.WriteLine(item);
                        break;
                    case ProductOptions.UPDATE:
                        Console.WriteLine("enter product details:\n product id, product name, category , price ,amount");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product.ID = readInt;
                        product.Name = Console.ReadLine();
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product.Category = (Category)readInt;
                        readString = Console.ReadLine();
                        double.TryParse(readString, out readDouble);
                        product.Price = readDouble;
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product.InStock = readInt;
                        product = iBl.Product.UpdateProduct(product);
                        Console.WriteLine(product);
                        break;
                    case ProductOptions.DELETE:
                        Console.WriteLine("Enter id product:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        iBl.Product.DeleteProduct(readInt);
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
            Console.WriteLine("Order menu: \n 1- get \n 2- get all \n 3- update shipping order \n 4- update delivery order  \n 5- order tracking \n 6- bonus");
            readString = Console.ReadLine();
            OrderOptions orderOptions = (OrderOptions)int.Parse(readString);
            Order order = new Order();
           OrderTracking orderTracking = new OrderTracking();   
            List<BO.OrderForList> ordersForList= new List<BO.OrderForList>();   
            try
            {
                switch (orderOptions)
                {
                    case OrderOptions.GET:
                        Console.WriteLine("Enter id oreder:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        order = iBl.Order.GetOrderById(readInt);
                        Console.WriteLine(order);
                        break;
                    case OrderOptions.GET_ALL:
                        IEnumerable<BO.OrderForList> orders = new List<BO.OrderForList>();
                        orders = iBl.Order.GetOrderList();
                        foreach(BO.OrderForList o in orders)
                        {
                            Console.WriteLine(o);
                        }
                        break;
                    case OrderOptions.UPDATE_SHIPPING_ORDER:
                        Console.WriteLine("Enter id order:");
                        readString= Console.ReadLine();
                        int.TryParse(readString,out readInt);   
                        order=iBl.Order.UpdateSendOrderByManager(readInt);
                        Console.WriteLine(order);
                        break;
                    case OrderOptions.UPDATE_DELIVERY_ORDER:
                        Console.WriteLine("Enter id order:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        order = iBl.Order.UpdateSupplyOrderByManager(readInt);
                        Console.WriteLine(order);
                        break;
                    case OrderOptions.ORDER_TRACKING:
                        Console.WriteLine("Enter id order:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        orderTracking = iBl.Order.TrackingOrder(readInt);
                        Console.WriteLine(orderTracking);
                        break;
                    case OrderOptions.BONUS:
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

            Console.WriteLine("cart menu: \n 1- add \n 2- update  \n 3- make order ");
            readString = Console.ReadLine();
            CartOptions cartOptions = (CartOptions)int.Parse(readString);
            string name;
            string email;
            string address;
            Order order = new Order();
            try
            {
                switch (cartOptions)
                {
                    case CartOptions.ADD:
                        Console.WriteLine("Enter product id:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        currentCart= iBl.cart.AddProductToCart(currentCart,readInt);
                        Console.WriteLine(currentCart);

                        break;
                    case CartOptions.UPDATE:
                        Console.WriteLine("Enter product id: ");
                        readString= Console.ReadLine();
                        int.TryParse(readString,out readInt);
                        Console.WriteLine("Enter a new amount: ");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt1);
                       currentCart= iBl.cart.UpdateProductAmountInCart(currentCart, readInt, readInt1);
                        Console.WriteLine(currentCart);
                        break;
                    case CartOptions.MAKE_ORDER:
                        Console.WriteLine("Enter your name: ");
                        name = Console.ReadLine();
                        currentCart.CustomerName = name;
                        Console.WriteLine("Enter your email:");
                        email = Console.ReadLine();
                        currentCart.CustomerEmail = email;
                        Console.WriteLine("Enter your address:");
                        address = Console.ReadLine();
                        currentCart.CustomerAddress = address;
                        iBl.cart.MakeOrder(currentCart, name, email, address);
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
            Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order\n 3-cart");
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

                
                Console.WriteLine("Shop menu: \n 0-exit \n 1-product \n 2-order \n 3-cart ");
                choice = Console.ReadLine();
                ManagementProgram.TryParse(choice, out managementProgram);

            }
        }
    }
}
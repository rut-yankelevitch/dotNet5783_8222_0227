﻿// See https://aka.ms/new-console-template for more information
using DO;
using Dal;
namespace DalTest
{
    enum ManagementProgram { EXIT, PRODUCT, ORDER, ORDER_ITEM }
    enum Options { ADD = 1, GET, GET_ALL, UPDATE, DELETE }

    public class Program
    {
        //static DataSource = new DataSource();
        static private OrderDal orderDal = new OrderDal();
        static private ProductDal productDal = new ProductDal();
        static private OrderItemDal orderItemDal = new OrderItemDal();
        static string readString;
        static int readInt;
        static double readDouble;
        static Options option;
        private static void ProductManagement()
        {
            Console.WriteLine("Product menu: \n 1-add \n 2- get by id \n 3- get all  \n 4- update \n 5- delete");

            readString = Console.ReadLine();
            Options.TryParse(readString, out option);
            Product product = new Product();
            try
            {
                switch (option)
                {
                    case Options.ADD:
                        {
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
                            int insertId = productDal.AddProduct(product);
                            Console.WriteLine("insert id: " + insertId);
                        }
                        break;
                    case Options.GET:
                        Console.WriteLine("Enter id product: ");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        product = productDal.GetProduct(readInt);
                        Console.WriteLine(product);
                        break;
                    case Options.GET_ALL:
                        Product[] products = productDal.GetAllProducts();
                        foreach (Product product1 in products)
                        {
                            Console.WriteLine(product1);
                        }
                        break;
                    case Options.UPDATE:
                        Console.WriteLine("enter product details:\n product id, product name, category , price ,amount");
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            int.TryParse(readString, out readInt);
                            product.ID = readInt;
                        }
                        readString = Console.ReadLine();
                        if (readString != "")
                            product.Name = readString;
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            int.TryParse(readString, out readInt);
                            product.Category = (Category)readInt;
                        }
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            int.TryParse(readString, out readInt);
                            product.Price = readInt;
                        }
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            int.TryParse(readString, out readInt);
                            product.InStock = readInt;
                        }
                        productDal.UpdateProduct(product);
                        break;
                    case Options.DELETE:
                        Console.WriteLine("enter product id to delete:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        productDal.DeleteProduct(readInt);
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
        private static void OrdersManagement()
        {
            Console.WriteLine("Product menu: \n 1-add \n 2- get by id \n 3- get all  \n 4- update \n 5- delete");
            string op = Console.ReadLine();
            option = (Options)int.Parse(op);
            Order order = new Order();
            DateTime helpDateTime;
            try
            {
                switch (option)
                {
                    case Options.ADD:
                        Console.WriteLine("enter order details:\n customer name, email , adress");
                        order.CustomerName = Console.ReadLine();
                        order.CustomerEmail = Console.ReadLine();
                        order.CustomerAdress = Console.ReadLine();
                        order.OrderDate = DateTime.Now;
                        order.ShipDate = DateTime.MinValue;
                        order.DeliveryrDate = DateTime.MinValue;
                        int insertId = orderDal.AddOrder(order);
                        Console.WriteLine("insert id: " + insertId);
                        break;
                    case Options.GET:
                        Console.WriteLine("enter order id: ");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        order = orderDal.GetOrder(readInt);
                        Console.WriteLine(order);
                        break;
                    case Options.GET_ALL:
                        Order[] orders = orderDal.GetAllOrder();
                        foreach (Order o in orders)
                        {
                            Console.WriteLine(o);
                        }
                        break;
                    case Options.UPDATE:
                        Console.WriteLine("enter order details:\n order id, client name, email , adress,create date , shiping date ,delivery date");
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            int.TryParse(readString, out readInt);
                            order.ID = readInt;
                        }

                        readString = Console.ReadLine();
                        if (readString != "")
                            order.CustomerName = Console.ReadLine();
                        readString = Console.ReadLine();
                        if (readString != "")
                            order.CustomerEmail = readString;
                        readString = Console.ReadLine();
                        if (readString != "")
                            order.CustomerAdress = readString;
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            DateTime.TryParse(readString, out helpDateTime);
                            order.OrderDate = helpDateTime;
                        }
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            DateTime.TryParse(readString, out helpDateTime);
                            order.ShipDate = helpDateTime;
                        }
                        readString = Console.ReadLine();
                        if (readString != "")
                        {
                            DateTime.TryParse(readString, out helpDateTime);
                            order.DeliveryrDate = helpDateTime;
                        }
                        orderDal.UpdateOrder(order);
                        break;
                    case Options.DELETE:
                        Console.WriteLine("enter order id to delete:");
                        readString = Console.ReadLine();
                        int.TryParse(readString, out readInt);
                        orderDal.DeleteOrder(readInt);
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
        private static void OrderItemsManagement()
        {
            Console.WriteLine("Product menu: \n 1-add \n 2- get by id \n 3- get all  \n 4- update \n 5- delete");
            OrderItem orderItem = new OrderItem();
            string op = Console.ReadLine();
            option = (Options)int.Parse(op);
            switch (option)
            {
                case Options.ADD:
                    Console.WriteLine("enter order item  details:\n order id, product id , price , amount");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    orderItem.OrderID = readInt;
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    orderItem.ProductID = readInt;
                    readString = Console.ReadLine();
                    double.TryParse(readString, out readDouble);
                    orderItem.Price = readDouble;
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    orderItem.Amount = readInt;
                    int insertId = orderItemDal.AddOrderItem(orderItem);
                    Console.WriteLine("insert id: " + insertId);
                    break;
                case Options.GET:
                    Console.WriteLine("enter order item id:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    orderItem = orderItemDal.GetOrderItem(readInt);
                    Console.WriteLine(orderItem);
                    break;
                case Options.GET_ALL:
                    OrderItem[] ordersItems = orderItemDal.GetAllOrderItem();
                    foreach (OrderItem ordItem in ordersItems)
                        Console.WriteLine(ordItem);
                    break;
                case Options.UPDATE:
                    Console.WriteLine("enter order item details:\n  order item id, order id, product id , price , amount");
                    if (readString != "")
                    {
                        int.TryParse(readString, out readInt);
                        orderItem.ID = readInt;
                    }

                    readString = Console.ReadLine();
                    if (readString != "")
                    {
                        int.TryParse(readString, out readInt);
                        orderItem.OrderID = readInt;
                    }
                    readString = Console.ReadLine();
                    if (readString != "")
                    {
                        int.TryParse(readString, out readInt);
                        orderItem.ProductID = readInt;
                    }
                    readString = Console.ReadLine();
                    if (readString != "")
                    {
                        double.TryParse(readString, out readDouble);
                        orderItem.Price = readDouble;
                    }
                    readString = Console.ReadLine();
                    if (readString != "")
                    {
                        int.TryParse(readString, out readInt);
                        orderItem.Amount = readInt;
                    }
                    orderItemDal.UpdateOrderItem(orderItem);

                    break;
                case Options.DELETE:
                    Console.WriteLine("enter order item id to delete:");
                    readString = Console.ReadLine();
                    int.TryParse(readString, out readInt);
                    orderItemDal.deleteOrderItem(readInt);
                    break;
                default:
                    break;
            }
        }

        public static void Main(string[] args)
        {
            Product pro   = new Product();
            Order or = new Order();
            OrderItem ordei = new OrderItem();  
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
                    case ManagementProgram.ORDER_ITEM:
                        OrderItemsManagement();
                        break;
                    case ManagementProgram.ORDER:
                        OrdersManagement();
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
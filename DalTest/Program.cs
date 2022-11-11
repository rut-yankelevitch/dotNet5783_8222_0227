// See https://aka.ms/new-console-template for more information
namespace DalTest {
    Enum Entities { EXIT,PRODUCT,ORDER,ORDER_ITEM};
    Enum Options { ADD=1,GET,GET_ALL,UPDATE,DELETE};

    public class Program { 
        private Product p= new Product ();
        private Order o= new Order ();
        private OrderItem oI= new OrderItem ();
        public void ProductEntities()
        { 
            Console.WriteLine("Product menu: \n 1-add \n 2-get all \n 3- get by id \n 4- update \n 5- delete");
            string op = Console.ReadLine ();
            Options o=(Options)int.Parse(op)
            string s;
            switch (o)
	        {
                case ADD:
                    {
                        Console.WriteLine("enter product-name: ");
                        Console.ReadLine(p.Name);
                        Console.WriteLine("enter product-category: ");
                        Console.ReadLine(p.Category);
                        Console.WriteLine("enter product-price: ");
                        Console.ReadLine(p.Price);
                        Console.WriteLine("enter amount in stoke: ");
                        Console.ReadLine(p.inStoke);
                    }
                    break;
                case GET:
                    break;
                case GET_ALL:
                    break;
                case UPDATE:
                    break;
                case DELETE:
                    break;
		        default:
	        }
        }
         public void OrderEntities()
        {
             string op=Console.ReadLine ();
            Options o=(Options)int.Parse(op)
            switch (o)
	        {
                case ADD:
                    break;
                case GET:
                    break;
                case GET_ALL:
                    break;
                case UPDATE:
                    break;
                case DELETE:
                    break;
		        default:
	        }
        }
        public void OrderItemEntities()
        {
             string op=Console.ReadLine ();
            Options o=(Options)int.Parse(op)
            switch (o)
	        {
                case ADD:
                    break;
                case GET:
                    break;
                case GET_ALL:
                    break;
                case UPDATE:
                    break;
                case DELETE:
                    break;
		        default:
	        }
        }

        public static void Main(string[] args) {
            string en = Console.ReadLine();
          e = (Entities)int.Parse(en);
    switch (e)
	    {
                case EXIT:
                     break;
                     case PRODUCT:ProductEntities();
                     break;
                     case ORDER:OrderEntities();
                     break;
                case ORDER_ITEM:OrderItemEntities();
                     break;
	     	default:
              break;
	    }
	}

}

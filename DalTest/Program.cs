// See https://aka.ms/new-console-template for more information
namespace DalTest {
    Enum Entities { EXIT,PRODUCT,ORDER,ORDER_ITEM};
    Enum Options { ADD,GET,GET_ALL,UPDATE,DELETE};

    public class Program { 
        private Product p= new Product ();
        private Order o= new Order ();
        private OrderItem oI= new OrderItem ();
        public void ProductEntities()
        {   string op=Console.ReadLine ();
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

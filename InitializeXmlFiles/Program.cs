using DO;
using Dal;
using System.Xml.Serialization;

namespace IntilizeXmlFile;
public class Program
{
    static void Main()
    {
        Type staticClassInfo = typeof(Dal.DataSource);
        var staticClassConstructorInfo = staticClassInfo.TypeInitializer;
        staticClassConstructorInfo?.Invoke(null, null);

        List<Product?> PrdouctList = DataSource.ProductList;
        List<DO.Order?> OrderList = DataSource.OrderList;
        List<OrderItem?> OrderItemList = DataSource.OrderItemList;
        List<DO.User?> UserList = new() { new DO.User() { ID=1,Name = "rut",Email="rut99656@gmail.com",Address="hy",Password="1234" },
         new DO.User() { ID=2,Name = "racheli",Email="r0527197777@gmail.com",Address="ab",Password="123456" }};
        List<DO.CartItem?> CartItemList = new() { new DO.CartItem() { ID = 1, Amount = 1, ProductID = 741598, UserID = 1 }, new DO.CartItem() { ID = 1, Amount = 1, ProductID = 458963, UserID = 2 } };

        StreamWriter wProduct = new(@"..\..\..\..\xml\Product.xml");
        XmlSerializer serProduct = new(typeof(List<Product?>));
        serProduct.Serialize(wProduct, PrdouctList);
        wProduct.Close();

        StreamWriter wOrder = new(@"..\..\..\..\xml\Order.xml");
        XmlSerializer serOrder = new(typeof(List<DO.Order?>));
        serOrder.Serialize(wOrder, OrderList);
        wOrder.Close();

        StreamWriter wOrderItem = new(@"..\..\..\..\xml\OrderItem.xml");
        XmlSerializer serOrderItem = new(typeof(List<OrderItem?>));
        serOrderItem.Serialize(wOrderItem, OrderItemList);
        wOrderItem.Close();

        StreamWriter wUser = new(@"..\..\..\..\xml\User.xml");
        XmlSerializer serUser = new(typeof(List<DO.User?>));
        serUser.Serialize(wUser, UserList);
        wUser.Close();

        StreamWriter wCartItem = new(@"..\..\..\..\xml\CartItem.xml");
        XmlSerializer serCartItem = new(typeof(List<DO.CartItem?>));
        serCartItem.Serialize(wCartItem, CartItemList);
        wCartItem.Close();
    }
}
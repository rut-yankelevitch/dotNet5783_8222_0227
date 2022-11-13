
namespace DO;
/// <summary>
///  Structure for OrderItem
/// </summary>


public struct OrderItem
    {
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public override string ToString() => $@"
     order item ID = {ID}
     Order product ID= {ProductID}, 
     customer order ID: {OrderID},
     price: {Price},
     amount: {Amount},
     ";

}



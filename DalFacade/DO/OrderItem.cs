
namespace DO;
/// <summary>
///  Structure for OrderItem
/// </summary>


public struct OrderItem
    {
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public override string ToString() => $@"
     Order product ID= {ProductID}, 
     customer order ID: {OrderID},
     customer price: {Price},
     customer amount: {Amount},
     ";

}



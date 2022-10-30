
namespace DO;
/// <summary>
///  Structure for Order
/// </summary>

public struct Order
    {
    /// <summary>
    /// Unique ID of Order
    /// </summary>
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryrDate { get; set; }
    public override string ToString() => $@"
     Order ID= {ID}, 
     customer name: {CustomerName},
     customer email: {CustomerEmail},
     customer adress: {CustomerAdress},
     order date: {OrderDate}
     shiping date: {ShipDate}
     delivery date: {DeliveryrDate}
     ";




}




using System.Diagnostics;
using System.Xml.Linq;

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
    /// <summary>
    /// the name of the ordering
    /// </summary>

    public string? CustomerName { get; set; }
    /// <summary>
    /// the email of the client
    /// </summary>

    public string? CustomerEmail { get; set; }
    /// <summary>
    /// mailing adress 
    /// </summary>

    public string? CustomerAdress { get; set; }
    /// <summary>
    /// the create order date
    /// </summary>

    public DateTime? OrderDate { get; set; } 
    /// <summary>
    /// the shipping date
    /// </summary>

    public DateTime? ShipDate { get; set; }
    /// <summary>
    /// the delivery date
    /// </summary>

    public DateTime? DeliveryrDate { get; set; }
    /// <summary>
    /// to string function to the order struct
    /// </summary>
    /// <returns>string with the order details</returns>
    public override string ToString() => this.ToStringProperty();
}

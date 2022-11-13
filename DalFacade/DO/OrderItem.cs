﻿
namespace DO;
/// <summary>
///  Structure for OrderItem
/// </summary>


public struct OrderItem
    {
    /// <summary>
    /// Unique ID of item in ordering
    /// </summary>

    public int ID { get; set; }
    /// <summary>
    ///  ID of the order
    /// </summary>

    public int ProductID { get; set; }
    /// <summary>
    ///  ID of product
    /// </summary>

    public int OrderID { get; set; }
    /// <summary>
    /// price per unit
    /// </summary>

    public double Price { get; set; }
    /// <summary>
    /// amount of ordered item
    /// </summary>

    public int Amount { get; set; }
    /// <summary>
    /// to string function to the OrderItem struct
    /// </summary>
    /// <returns>string with the ordered item details</returns>
    /// 
    public override string ToString() => $@"
     order item ID = {ID}
     product ID= {ProductID}, 
     order ID: {OrderID},
     price: {Price},
     amount: {Amount},
     ";

}



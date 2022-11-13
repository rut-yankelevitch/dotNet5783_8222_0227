namespace DO;
/// <summary>
///  Structure for Product
/// </summary>
public struct Product
    {
    /// <summary>
    /// Unique ID of Product
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Name of Product
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// the category of the product
    /// </summary>

    public Category Category{ get; set; }
    /// <summary>
    /// price per unit
    /// </summary>

    public double Price { get; set; }
    /// <summary>
    /// amount available
    /// </summary>

    public int InStock { get; set; }

    /// <summary>
    /// to string function to the product struct
    /// </summary>
    /// <returns>string with the ordered item details</returns>
    public override string ToString() => $@"
     Product ID={ID}: {Name}, 
     category - {Category},
     Price: {Price},
     Amount in stock: {InStock}
     ";
}


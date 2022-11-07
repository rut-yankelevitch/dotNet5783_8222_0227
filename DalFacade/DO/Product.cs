
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
    /// Category of Product
    /// </summary>
    public string Category { get; set; }
    /// <summary>
    /// Price of Product
    /// </summary>
    public string Price { get; set; }
    /// <summary>
    /// qty in stock of Product
    /// </summary>
    public string InStock { get; set; }
    /// <summary>
    /// A ToString method that will return a string describing an object
    /// </summary>
    public override string ToString() => $@"
     Product ID={ID}: {Name}, 
     category - {Category}
     Price: {Price}
     Amount in stock: {InStock}
     ";
}


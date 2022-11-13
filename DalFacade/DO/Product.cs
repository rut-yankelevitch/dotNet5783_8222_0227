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
   public Category Category{ get; set; }
    public double Price { get; set; }
    public int InStock { get; set; }



    public override string ToString() => $@"
     Product ID={ID}: {Name}, 
     category - {Category},
     Price: {Price},
     Amount in stock: {InStock}
     ";
}


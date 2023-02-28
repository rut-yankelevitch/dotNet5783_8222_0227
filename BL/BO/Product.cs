﻿namespace BO
{
    /// <summary>
    /// A class for a logical entity: product
    /// </summary>
    public class Product
    {
        public int ID { get; set; } 
        public string? Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public int InStock { get; set; }
        public string? Image { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    /// <summary>
    /// A class for a logical entity:product item
    /// </summary>
    public class ProductItem
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public double Price  { get; set; }
        //זה נכון לעשות ?
        public Category? Category { get; set; }
        public int Amount { get; set; }
        public bool Instock { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}

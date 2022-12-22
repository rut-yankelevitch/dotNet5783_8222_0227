﻿using BlApi;
namespace BlImplementation
{
    /// <summary>
    /// A class that implements the ibl interface and returns all the main logical entities
    /// </summary>
    sealed internal class Bl : IBl
    {
        /// <summary>
        /// A method that returns the Product entity
        /// </summary>
        public IProduct Product =>  new Product();

        /// <summary>
        /// A method that returns the order entity
        /// </summary>
        public IOrder Order =>  new Order();

        /// <summary>
        /// A method that returns the cart entity
        /// </summary>
        public ICart cart =>  new Cart();
    }
}

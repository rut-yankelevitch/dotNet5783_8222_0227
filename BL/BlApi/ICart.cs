﻿using BO;

namespace BlApi
{
    /// <summary>
    /// Interface for a cart logical entity
    /// </summary>
    public interface ICart
    {
        /// <summary>
        /// Definition of a function that adds a product to the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="idProduct"></param>
        /// <returns>cart</returns>
        public Cart AddProductToCart(Cart cart, int idProduct, int amount, bool isRegistered);


        /// <summary>
        /// Definition of a function that update the amount of product in the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="idProduct"></param>
        /// <param name="amount"></param>
        /// <returns>cart</returns>
        public Cart UpdateProductAmountInCart(Cart cart, int idProduct, int amount, bool isRegistered);


        /// <summary>
        /// Definition of a function that confirms an order
        /// </summary>
        /// <param name="cart"></param>
        public int? MakeOrder(Cart cart, bool isRegistered);


        /// <summary>
        /// Definition of a function that return cart
        /// </summary>
        /// <param name="cart"></param>
        public Cart GetCart(int? userId);
    }
}

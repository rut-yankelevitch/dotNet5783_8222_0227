﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface ICart
    {
        /*
    public int Add(Cart cart);
	public void Update(Cart cart );
	public void Delete(int id);
	public Cart  GetById(int id);
    public IEnumerable <Cart> GetAll() ;*/
    public Cart AddProductToCart(Cart cart, int idProduct);
    public Cart UpdateProductAmountInCart(Cart cart, int idProduct, int amount);
    public Order MakeOrder(Cart cart ,string customerName,string customerEmail,string customerAddress);

    }
}

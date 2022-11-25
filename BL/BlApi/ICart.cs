using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface ICart
    {
    public Cart AddProductToCart(Cart cart, int idProduct);
    public Cart UpdateProductAmountInCart(Cart cart, int idProduct, int amount);
    public void MakeOrder(Cart cart ,string customerName,string customerEmail,string customerAddress);

    }
}

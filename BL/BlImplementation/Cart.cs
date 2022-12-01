using System;
using BlApi;
using DalApi;
namespace BlImplementation;

/// <summary>
/// Summary description for Class1
/// </summary>
internal class Cart : ICart
{
    private IDal dal = new DalList.DalList();
    public BO.Cart AddProductToCart(BO.Cart cart, int idProduct)
    {
        try
        {
            //List<BO.OrderItem> orderItems = cart.Items;
            DO.Product product = new DO.Product();
            BO.OrderItem orderItem1 = new BO.OrderItem();
            if (cart.Items!=null)
            {
                foreach (BO.OrderItem orderItem in cart.Items)
                {
                    if (orderItem.ProductID == idProduct)
                        throw new BO.AlreadyExistException("product exist in cart");
                }
            }
            else
            {
                cart.Items = new List<BO.OrderItem>();
            }
            product = dal.Product.GetById(idProduct);
            if (product.InStock <= 0)
                throw new BO.NotExistException("product not exist in stock");
            //מאיפה לדעת כמות??
            //orderItem1.ID= מאיפה ID
            orderItem1.Name = product.Name;
            orderItem1.ProductID = idProduct;
            orderItem1.Amount = 1;
            orderItem1.Price = product.Price;
            orderItem1.TotalPrice = orderItem1.Price * orderItem1.Amount;
            cart.Items.Add(orderItem1);
            cart.TotalPrice += orderItem1.TotalPrice;
            return cart;
        }
        catch (Exception msg)
        {
            throw new BO.NotExistException(msg.Message);
        }
    }
    public BO.Cart UpdateProductAmountInCart(BO.Cart cart, int idProduct, int amount)
    {
        bool flag=false;
        try
        {
            DO.Product product = new DO.Product();
            product = dal.Product.GetById(idProduct);
            if (product.InStock <amount)
                throw new BO.NotExistException("product not exist in stock");
            if (cart.Items != null)
            {
                foreach (BO.OrderItem orderItem in cart.Items)
                {
                    flag = true;
                    if (orderItem.ProductID == idProduct)
                    {
                        if (amount < 0)
                        {
                            throw new BO.InvalidInputException("invalid amount");
                        }
                        else 
                        {
                            if (amount == 0)
                            {
                                cart.TotalPrice -= orderItem.TotalPrice;
                                cart.Items.Remove(orderItem);
                            }
                            else
                            {
                                cart.TotalPrice -= orderItem.TotalPrice;
                                orderItem.Amount = amount;
                                orderItem.TotalPrice = orderItem.Price * amount;
                                cart.TotalPrice += orderItem.TotalPrice;
                            }
                        }
                    }
                }
                if (flag==false)
                {
                    throw new Exception("This item is not in the cart");
                }
            }
            else
                throw new Exception("There are no items in the cart");
            return cart;
        }
        catch (Exception msg)
        {
            throw new BO.NotExistException(msg.Message);
        }

    }
    //void?????
    public void MakeOrder(BO.Cart cart, string customerName, string customerEmail, string customerAddress)
    {
        try
        {
            DO.Order order = new DO.Order();
            DO.Product product=new DO.Product();
            if (customerName == "" || customerEmail == "" || customerAddress == "")
            {
                throw new BO.InvalidFormat("Invalid format");
            }
            //איזה חריגה לעשות???
            if (cart.Items==null)
                throw new Exception("There are no items in the cart.");
           foreach(BO.OrderItem orderItem in cart.Items)
            {
                if (orderItem.Amount <= 0)
                    throw new BO.InvalidFormat("invalid amount");
                product = dal.Product.GetById(orderItem.ProductID);
                if (product.InStock < orderItem.Amount)
                    throw new BO.NotExistException("amount not in stock ");
            }
            order.CustomerName=customerName;
            order.CustomerEmail=customerEmail;
            order.CustomerAdress=customerAddress;
            order.OrderDate=DateTime.Now;
            order.ShipDate = new DateTime();
            order.DeliveryrDate=new DateTime();
            int id = dal.Order.Add(order);
            foreach (BO.OrderItem orderItem in cart.Items)
            {
                DO.OrderItem orderItem1 = new DO.OrderItem();
                orderItem1.OrderID = id;
                orderItem1.ProductID=orderItem.ProductID;
                orderItem1.Amount=orderItem.Amount;
                orderItem1.Price=orderItem.Price;
                product = dal.Product.GetById(orderItem1.ProductID);
                product.InStock-= orderItem.Amount;
                dal.Product.Update(product);
                dal.OrderItem.Add(orderItem1);  
            }
        }
        catch (Exception msg)
        {
            throw new BO.NotExistException(msg.Message);
        }

    }

}

using System.Security.Cryptography;
using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;

internal class Cart : ICart
{
    //??
    private DalApi.IDal dal =DalApi.Factory.Get();


    /// <summary>
    ///function that adds a product to the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="idProduct"></param>
    /// <returns>update cart</returns>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Cart AddProductToCart(BO.Cart cart, int idProduct)
    {
        try
        {
            DO.Product product = new DO.Product();
            BO.OrderItem orderItem1 = new BO.OrderItem();

//            if (cart.Items != null)
//          {
                if(cart.Items?.FirstOrDefault(item=>item?.ProductID==idProduct)!=null)
                {
                    throw new BO.BLImpossibleActionException("product exist in cart");
                }
            //}
            //else
            //{
            //    cart.Items = new List<BO.OrderItem>();
            //}
            product = dal.Product.GetByCondition(product2 => product2?.ID==idProduct);

            if (product.InStock <= 0)
                throw new BO.BLImpossibleActionException("product not exist in stock");

            orderItem1.Name = product.Name;
            orderItem1.ProductID = idProduct;
            orderItem1.Amount = 1;
            orderItem1.Price = product.Price;
            orderItem1.TotalPrice = orderItem1.Price * orderItem1.Amount;
            cart.Items?.Add(orderItem1);
            cart.TotalPrice += orderItem1.TotalPrice;
            return cart;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product dosent exsit", ex);
        }
    }


    /// <summary>
    ///function that update the amount of product in the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="idProduct"></param>
    /// <param name="amount"></param>
    /// <returns>update curt</returns>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Cart UpdateProductAmountInCart(BO.Cart cart, int idProduct, int amount)
    {
        try
        {
            DO.Product product = new DO.Product();
            BO.OrderItem newOrderItem = new BO.OrderItem();
            product = dal.Product.GetByCondition(product2=>product2?.ID==idProduct);

            if (product.InStock < amount)
                throw new BO.BLImpossibleActionException("product not exist in stock");

            if (cart.Items != null)
            {
                if (amount < 0)
                    throw new BO.BLInvalidInputException("invalid amount");
                var sum = cart.Items.Where(item => item?.ProductID == idProduct).Sum(item => item?.TotalPrice)??
                    throw new BLImpossibleActionException("This item is not in the cart");
                cart.TotalPrice -= (double)sum!;
                if (amount > 0) 
                {
                    var newItem = cart.Items.FirstOrDefault(item=> item?.ProductID == idProduct);
                    if(newItem != null)
                    {
                        newOrderItem = newItem;
                        newOrderItem.Amount = amount;
                        newOrderItem.TotalPrice= amount*newItem.Price;
                        cart.TotalPrice += newOrderItem.TotalPrice;
                    }
                 }
                cart.Items.RemoveAll(item => item?.ProductID == idProduct);
                if (amount > 0)
                    cart.Items.Add(newOrderItem);




                //if (amount == 0)
                //{
                //}
                //else
                //{
                //  //var a= cart.Items.Where(item => item?.ProductID == idProduct).Select(item => item.ID = amount );
                //}


                //foreach (BO.OrderItem orderItem in cart.Items)
                //{

                //        if (orderItem.ProductID == idProduct)
                //    {
                //        flag = true;
                //        if (amount < 0)
                //        {
                //            throw new BO.BLInvalidInputException("invalid amount");
                //        }
                //        else
                //        {
                //            if (amount == 0)
                //            {
                //                cart.TotalPrice -= orderItem.TotalPrice;
                //                cart.Items.Remove(orderItem);
                //            }
                //            else
                //            {
                //                cart.TotalPrice -= orderItem.TotalPrice;
                //                orderItem.Amount = amount;
                //                orderItem.TotalPrice = orderItem.Price * amount;
                //                cart.TotalPrice += orderItem.TotalPrice;
                //            }
                //        }
                //    }
                //}
                //if (flag == false)
                //{
                //    throw new BLImpossibleActionException("This item is not in the cart");
                //}
            }
            else
                throw new BLImpossibleActionException("There are no items in the cart");
            return cart;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product dosent exsit", ex);
        }

    }


    /// <summary>
    /// function that confirms an order
    /// </summary>
    /// <param name="cart"></param>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    public void MakeOrder(BO.Cart cart)
    {
        try
        {
            DO.Order order = new DO.Order();
            //DO.Product product = new DO.Product();

            if (cart.CustomerName == "" || cart.CustomerEmail == "" || cart.CustomerAddress == "")
            {
                throw new BO.BLInvalidInputException("Invalid details");
            }
            if (cart.Items == null)
                throw new BLImpossibleActionException("There are no items in the cart.");

            order.OrderDate = DateTime.Now;
            order.ShipDate = null;
            order.DeliveryrDate =null;
            int id = dal.Order.Add(order);
            var a = from ordItem in cart.Items
                    let product = dal.Product.GetByCondition(product2 => product2?.ID == ordItem.ProductID)
                    where product.InStock >= ordItem.Amount
                    select new
                    {
                        orderItemId =dal.OrderItem.Add(new DO.OrderItem { OrderID = id, ProductID = ordItem.ProductID, Amount = ordItem.Amount, Price = ordItem.Price })
                        /*להוסיף זריקה*/,
                        product2 = new DO.Product { Price = product.Price, Category = product.Category, ID = product.ID, Name = product.Name, InStock = (product.InStock) - ordItem.Amount }
                    };
             a.ToList().ForEach(a => dal.Product.Update(a.product2));/*להוסיף זריקה*/


            //foreach (BO.OrderItem orderItem in cart.Items)
            //{
            //    try
            //    {
            //        product = dal.Product.GetByCondition(product2=>product2?.ID==orderItem.ProductID);
            //    }
            //    catch (DO.DalDoesNotExistException ex)
            //    {
            //        throw new BO.BLDoesNotExistException("product dosent exsit", ex);
            //    }
            //////    if (orderItem?.Amount <= 0)
            //////        throw new BO.BLImpossibleActionException("invalid amount");
            //    if (product.InStock < orderItem.Amount)
            //        throw new BO.BLImpossibleActionException("amount not in stock ");

            //    DO.OrderItem orderItem1 = new DO.OrderItem();
            //    orderItem1.OrderID = id;
            //    orderItem1.ProductID = orderItem.ProductID;
            //    orderItem1.Amount = orderItem.Amount;
            //    orderItem1.Price = orderItem.Price;
            //    product.InStock -= orderItem.Amount;
            //    try
            //    {
            //        dal.Product.Update(product);
            //    }
            //    catch (DO.DalDoesNotExistException ex)
            //    {
            //        throw new BO.BLDoesNotExistException("product dosent exsit", ex);
            //    }
            //    try
            //    {
            //        dal.OrderItem.Add(orderItem1);
            //    }
            //    catch (DO.DalDoesNotExistException ex)
            //    {
            //        throw new BO.BLDoesNotExistException($"{ex.EntityName} dosent exsit", ex);
            //    }
            //}
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product dosent exsit", ex);
        }

    }

}

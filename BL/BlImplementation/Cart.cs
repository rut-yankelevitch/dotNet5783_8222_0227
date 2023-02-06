﻿using BlApi;
using BO;
namespace BlImplementation;

internal class Cart : ICart
{
    private DalApi.IDal dal = DalApi.Factory.Get();

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

            if (cart.Items?.FirstOrDefault(item => item?.ProductID == idProduct) != null)
            {
                throw new BO.BLImpossibleActionException("product exist in cart");
            }
            product = dal.Product.GetByCondition(product2 => product2?.ID == idProduct);

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
    /// <param name="productId"></param>
    /// <param name="amount"></param>
    /// <returns>update curt</returns>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    /// <exception cref="BO.InvalidInputBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    public BO.Cart UpdateProductAmountInCart(BO.Cart cart, int productId, int amount)
    {
        try
        {
            if (cart.Items == null)
                throw new BO.BLImpossibleActionException("There are no items in the cart");
            var result = cart.Items.FirstOrDefault(item => item?.ProductID == productId);

            if (result == null)
                throw new BLImpossibleActionException("This item is not in the cart");
            DO.Product product = dal.Product.GetByCondition(prod => prod?.ID == productId);

            if (product.InStock < amount)
                throw new BLImpossibleActionException("product not exist in stock");
            cart.TotalPrice -= ( result.Amount) * result.Price;
            if (amount == 0)
            {
                cart.Items.Remove(result);
                return cart;
            }
            else
            {
                var cartItems = from item in cart.Items
                                let orderItem = (BO.OrderItem?)item
                                select new BO.OrderItem
                                {
                                    ID = orderItem.ID,
                                    Name = orderItem.Name,
                                    ProductID = orderItem.ProductID,
                                    Price = orderItem.Price,
                                    Amount =( orderItem?.ProductID == productId) ? amount : orderItem.Amount,
                                    TotalPrice = orderItem?.ProductID == productId ? amount * orderItem.Price : orderItem.TotalPrice
                                };
                cart.Items=cartItems.ToList();
            }
            return cart;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BLDoesNotExistException("product dosent exsit", ex);
        }
    }


    /// <summary>
    /// function that confirms an order
    /// </summary>
    /// <param name="cart"></param>
    /// <exception cref="BO.InvalidInputBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    public void MakeOrder(BO.Cart cart)
    {
        try
        {
            if (cart.CustomerName == "" || cart.CustomerEmail == "" || cart.CustomerAddress == "")
                throw new BLInvalidInputException("Invalid details");

            if (cart.Items == null)
                throw new BLImpossibleActionException("There are no items in the cart.");

            int id = dal.Order.Add(
                new DO.Order
                {
                    CustomerName = cart.CustomerName,
                    CustomerEmail = cart.CustomerEmail,
                    CustomerAdress = cart.CustomerAddress,
                    OrderDate = DateTime.Now,
                    ShipDate = null,
                    DeliveryrDate = null
                });

            IEnumerable<DO.Product?> products = dal.Product.GetAll();

            //return a list of tuples that everyone ave a orderItem to add and a updated product
            var result = from item in cart.Items
                         join prod in products on item.ProductID equals prod?.ID into empdept
                         from ed in empdept.DefaultIfEmpty()
                         let product = ed ?? throw new BLImpossibleActionException("product does not exist")
                         let orderItem = item!
                         select new
                         {
                             orderItem = new DO.OrderItem
                             {
                                 OrderID = id,
                                 ProductID = item.ProductID,
                                 Amount = item.Amount > 0 ? item.Amount : throw new BLImpossibleActionException("invalid amount"),
                                 Price = item.Price
                             },
                             prod = new DO.Product
                             {
                                 ID = product.ID,
                                 Price = product.Price,
                                 Category = product.Category,
                                 InStock = product.InStock > item?.Amount ? product.InStock - item?.Amount ?? 0 : throw new BLImpossibleActionException("amount not in stock "),
                                 Name = product.Name
                             }
                         };

            //for each tuple we update the product list and add a orderItem to the order Item list
            result.ToList().ForEach(res =>
            {
                try
                {
                    dal.Product.Update(res.prod);
                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BLDoesNotExistException("product update failes", ex);
                }
                try
                {
                    dal.OrderItem.Add(res.orderItem);
                }
                catch (DO.DalAlreadyExistException ex)
                {
                    throw new BLAlreadyExistException("order Item alredy exist ", ex);
                }
            });
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BLDoesNotExistException($"{ex.EntityName} dosent exsit", ex);
        }
    }

}
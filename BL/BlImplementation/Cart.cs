using BlApi;
using BO;
namespace BlImplementation;

internal class Cart : ICart
{
    private readonly DalApi.IDal dal = DalApi.Factory.Get();

    /// <summary>
    ///function that adds a product to the cart
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="idProduct"></param>
    /// <returns>update cart</returns>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Cart AddProductToCart(BO.Cart cart, int idProduct, int amount, bool isRegistered = false)
    {
        if (isRegistered) addToCartDal(cart, idProduct);

        DO.Product product = new DO.Product();
        BO.OrderItem orderItem1 = new BO.OrderItem();
        if (amount == 0)
        {
            throw new BO.BLInvalidInputException("Invalid input ");
        }
        if (cart.Items?.FirstOrDefault(item => item?.ProductID == idProduct) != null)
        {
            throw new BO.BLImpossibleActionException("product exist in cart");
        }

        try { product = dal.Product.GetByCondition(product2 => product2?.ID == idProduct); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("product dosent exsit", ex); }

        if (product.InStock < amount)
            throw new BO.BLImpossibleActionException("product not exist in stock");

        orderItem1.Name = product.Name;
        orderItem1.ProductID = idProduct;
        orderItem1.Amount = amount;
        orderItem1.Price = product.Price;
        orderItem1.TotalPrice = orderItem1.Price * orderItem1.Amount;
        cart.Items?.Add(orderItem1);
        cart.TotalPrice += orderItem1.TotalPrice;
        return cart;
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
    public BO.Cart UpdateProductAmountInCart(BO.Cart cart, int productId, int amount, bool isRegistered = false)
    {
        if (isRegistered)
            updateAmountDal(cart, productId, amount);

        DO.Product product;
        if (cart.Items == null)
            throw new BO.BLImpossibleActionException("There are no items in the cart");
        var result = cart.Items.FirstOrDefault(item => item?.ProductID == productId);

        if (result == null)
            throw new BLImpossibleActionException("This item is not in the cart");

        try { product = dal.Product.GetByCondition(prod => prod?.ID == productId); }
        catch (DO.DalDoesNotExistException ex) { throw new BLDoesNotExistException("product dosent exsit", ex); }

        if (product.InStock < amount)
            throw new BLImpossibleActionException("product not exist in stock");
        cart.TotalPrice -= (result.Amount) * result.Price;
        cart.TotalPrice += amount * result.Price;
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
                                Amount = (orderItem?.ProductID == productId) ? amount : orderItem.Amount,
                                TotalPrice = orderItem?.ProductID == productId ? amount * orderItem.Price : orderItem.TotalPrice
                            };
            cart.Items = cartItems.ToList();
        }
        return cart;
    }



    /// <summary>
    /// function that confirms an order
    /// </summary>
    /// <param name="cart"></param>
    /// <exception cref="BO.InvalidInputBlException"></exception>
    /// <exception cref="ImpossibleActionBlException"></exception>
    /// <exception cref="BO.DoesNotExistedBlException"></exception>
    /// <exception cref="BO.ImpossibleActionBlException"></exception>
    public void MakeOrder(BO.Cart cart , bool isRegistered = false)
    {
        if (isRegistered)
            confirmOrderDal(cart);

        int id;
        if (cart.CustomerName == "" || cart.CustomerEmail == "" || cart.CustomerAddress == "")
            throw new BLInvalidInputException("Invalid details");

        if (cart?.Items?.Count == 0)
            throw new BLImpossibleActionException("There are no items in the cart.");

        try
        {
            id = dal.Order.Add(
                    new DO.Order
                    {
                        CustomerName = cart?.CustomerName,
                        CustomerEmail = cart?.CustomerEmail,
                        CustomerAdress = cart?.CustomerAddress,
                        OrderDate = DateTime.Now,
                        ShipDate = null,
                        DeliveryDate = null
                    });
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BLDoesNotExistException($"{ex.EntityName} dosent exsit", ex);
        }

        IEnumerable<DO.Product?> products = dal.Product.GetAll();

        //return a list of tuples that everyone ave a orderItem to add and a updated product
        var result = from item in cart?.Items
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
                             InStock = product.InStock >= item?.Amount ? product.InStock - item?.Amount ?? 0 : throw new BLImpossibleActionException("amount not in stock "),
                             Name = product.Name,
                             Image = product.Image
                         }
                     };

        //for each tuple we update the product list and add a orderItem to the order Item list
        result.ToList().ForEach(res =>
        {
            try { dal.Product.Update(res.prod); }
            catch (DO.DalDoesNotExistException ex) { throw new BLDoesNotExistException("product update failes", ex); }

            try { dal.OrderItem.Add(res.orderItem); }
            catch (DO.DalDoesNotExistException ex) { throw new BLDoesNotExistException("product update failes", ex); }
        });
    }

    public BO.Cart GetCart(int userId)
    {
        BO.Cart cart = new();
        try
        {
            if (userId < 0) throw new BLInvalidInputException("invalid id");

            IEnumerable<DO.CartItem?> cartItems = dal.CartItem.GetAll(o => o?.UserID == userId);
            List<BO.OrderItem> orderItems = new();
            DO.Product pro = new();
            BO.OrderItem oItem = new();
            foreach (var item in cartItems)
            {
                oItem = Tools.cast<BO.OrderItem, DO.CartItem>((DO.CartItem)item!);
                pro = dal.Product.GetByCondition(o => o?.ID == item?.ProductID);
                oItem.Name = pro.Name;
                oItem.Price = pro.Price;
                oItem.TotalPrice = oItem.Price * oItem.Amount;
                orderItems.Add(oItem);
            }
            //IEnumerable<DO.OrderItem> orderItems = from item in cartItems
            //                                       select dal.OrderItem.Get(o => o.ProductID == item.ProductID);
            //IEnumerable<BO.OrderItem> BOItems = from item in orderItems
            //                                    select BlUtils.cast<BO.OrderItem, DO.Product>(item);
            cart.UserID = userId;
            cart.Items = orderItems!;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BLDoesNotExistException($"{ex.EntityName} dosent exsit", ex);
        }
        return cart;
    }


    private void addToCartDal(BO.Cart? cart, int productID)
    {
        DO.CartItem cartItem = new();
        cartItem.ProductID = productID;
        cartItem.Amount = 1;
        cartItem.UserID = cart?.UserID ?? throw new BO.BLImpossibleActionException("null id");
        dal.CartItem.Add(cartItem);
    }


    private void confirmOrderDal(BO.Cart? cart)
    {
        dal.CartItem.Delete(c => c.UserID == cart.UserID);
    }


    private void updateAmountDal(BO.Cart cart, int productID, int newAmount)
    {
        DO.CartItem cartItem = new();
        cartItem.ProductID = productID;
        cartItem.Amount = newAmount;
        cartItem.ID = dal.CartItem.GetByCondition(c => c?.ProductID == productID && c?.UserID == cart.UserID).ID;
        cartItem.UserID = cart?.UserID ?? throw new BO.BLImpossibleActionException("null id");
        dal.CartItem.Update(cartItem);
    }


}
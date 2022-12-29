namespace BlImplementation;

/// <summary>
/// A class that implements the iorder interface
/// </summary>
internal class Order : BlApi.IOrder
{
    //??
    private DalApi.IDal dal =  DalApi.Factory.Get();

    /// <summary>
    /// function that returns all orders
    /// </summary>
    /// <returns>list of orders</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public IEnumerable<BO.OrderForList> GetOrderList()
    {
        try
        {
           IEnumerable<DO.Order?> orders = dal.Order.GetAll();
            var ordersForList = from order in orders
                                let orderItems = dal.OrderItem.GetAll(orderitem2 => orderitem2?.OrderID == order?.ID)
                                let amount = orderItems.Sum(o => ((DO.OrderItem)o!).Amount)
                                let totalPrice = orderItems.Sum(o => ((DO.OrderItem)o!).Amount*((DO.OrderItem)o!).Price)
                                select new BO.OrderForList
                                {
                                    ID = ((DO.Order)order!).ID,
                                    CustomerName = ((DO.Order)order!).CustomerName,
                                    AmountOfItems = amount,
                                    TotalPrice = totalPrice,
                                    Status = (((DO.Order)order!).DeliveryrDate != null && ((DO.Order)order!).DeliveryrDate < DateTime.Now) ?
                                                BO.OrderStatus.ProvidedOrder : ((DO.Order)order!).ShipDate != null && ((DO.Order)order!).ShipDate < DateTime.Now ?
                                                BO.OrderStatus.SendOrder : BO.OrderStatus.ConfirmedOrder
                                };

            return ordersForList;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }
    }


    /// <summary>
    /// function that returns order by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Order GetOrderById(int id)
    {
        try
        {
            BO.Order order = new BO.Order();
            DO.Order orderDal = new DO.Order();
            IEnumerable<DO.OrderItem?> orderitemsDal;

            try
            {
                orderDal = dal.Order.GetByCondition(order2 => order2?.ID == id);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order doesnot exist", ex);
            }
            try
            {
                orderitemsDal = dal.OrderItem.GetAll(orderitem2 => orderitem2?.OrderID == id);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order doesnot exist", ex);
            }
            var orderitems = from orderItem in orderitemsDal
            let  product = dal.Product.GetByCondition(product2 => product2?.ID == orderItem?.ProductID)
            select new BO.OrderItem
            {
                ID = ((DO.OrderItem)orderItem!).ID,
                Name= product.Name,    
                ProductID = ((DO.OrderItem)orderItem!).ProductID,
                Price = ((DO.OrderItem)orderItem!).Price,
                Amount = ((DO.OrderItem)orderItem!).Amount,
                TotalPrice = ((DO.OrderItem)orderItem!).Amount * ((DO.OrderItem)orderItem!).Price
            };
            order.ID = orderDal.ID;
            order.CustomerName = orderDal.CustomerName;
            order.CustomerAddress = orderDal.CustomerAdress;
            order.CustomerEmail = orderDal.CustomerEmail;
            order.OrderDate = orderDal.OrderDate;
            order.ShipDate = orderDal.ShipDate;
            order.DeliveryDate = orderDal.DeliveryrDate;
            order.TotalPrice = orderitems.Sum(x => x.TotalPrice);
            order.Items = orderitems.ToList();
            if (orderDal.DeliveryrDate != null && orderDal.DeliveryrDate < DateTime.Now)
                order.Status = BO.OrderStatus.ProvidedOrder;
            else
            {
                if (orderDal.ShipDate != null && order.ShipDate < DateTime.Now)
                    order.Status = BO.OrderStatus.SendOrder;
                else
                    order.Status = BO.OrderStatus.ConfirmedOrder;
            }
            return order;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }

        {

            //foreach (DO.OrderItem item in orderitemsDal)
            //{
            //    BO.OrderItem orderitem = new BO.OrderItem();
            //    orderitem.ID = item.ID;
            //    orderitem.ProductID = item.ProductID;
            //    orderitem.Price = item.Price;
            //    orderitem.Amount = item.Amount;
            //    orderitem.TotalPrice = item.Amount * item.Price;
            //    try
            //    {
            //        product = dal.Product.GetByCondition(product2=>product2?.ID==orderitem.ProductID);
            //    }
            //    catch (DO.DalDoesNotExistException ex)
            //    {
            //        throw new BO.BLDoesNotExistException("order does not exist", ex);
            //    }
            //    orderitem.Name = product.Name;
            //    orderitems.Add(orderitem);
            //    totalPrice += orderitem.TotalPrice;
        }
    }


    /// <summary>
    /// function that update the send order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLMistakeUpdateException"></exception>
    public BO.Order UpdateSendOrderByManager(int id)
    {
        BO.Order order = new BO.Order();
        DO.Order orderDal = new DO.Order();
        IEnumerable<DO.OrderItem?> orderitemsDal = new List<DO.OrderItem?>();

        try
        {
            orderDal = dal.Order.GetByCondition(order2=>order2?.ID==id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order does not exist", ex);
        }

        if (orderDal.ShipDate != null&& orderDal.ShipDate < DateTime.Now)
            throw new BO.BLImpossibleActionException("order send");
        else
        {
            orderDal.ShipDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order does not exist", ex);
            }
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAll(orderitem2=>orderitem2?.OrderID==id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }

        var orderItems = from ordItem in orderitemsDal
                let orderItem = (DO.OrderItem)ordItem
                let product = dal.Product.GetByCondition(product2 => product2?.ID == orderItem.ProductID)
                select new BO.OrderItem
                {
                    ID = orderItem.ID,
                    Amount = orderItem.Amount,
                    Name = product.Name,
                    Price = orderItem.Price,
                    TotalPrice = orderItem.Price * orderItem.Amount
                };
        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAddress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.OrderDate = orderDal.OrderDate;
        order.ShipDate = orderDal.ShipDate;
        order.DeliveryDate = orderDal.DeliveryrDate;
        order.TotalPrice = orderItems.Sum(o=>o.TotalPrice);
        order.Items = orderItems.ToList();
        order.Status = BO.OrderStatus.SendOrder;
        return order;
    }


    /// <summary>
    ///  function that update the supply order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    public BO.Order UpdateSupplyOrderByManager(int id)
    {
        BO.Order order = new BO.Order();
        List<BO.OrderItem> orderitems = new List<BO.OrderItem>();
        DO.Order orderDal = new DO.Order();

        IEnumerable<DO.OrderItem?> orderitemsDal = new List<DO.OrderItem?>();

        try
        {
            orderDal = dal.Order.GetByCondition(order2=>order2?.ID==id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }

        if (orderDal.DeliveryrDate != null&&orderDal.DeliveryrDate < DateTime.Now)
            throw new BO.BLImpossibleActionException("order Delivery");
        else
        {

            if (orderDal.ShipDate > DateTime.Now)
                throw new BO.BLImpossibleActionException("It is not possible to update a delivery date before a shipping date");
            orderDal.DeliveryrDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order dosent exsit", ex);
            }
        }
        try
        {
            orderitemsDal = dal.OrderItem.GetAll(orderitem2=>orderitem2?.OrderID==id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }
         var orderItems = from ordItem in orderitemsDal
                 let orderItem = (DO.OrderItem)ordItem
                 let product = dal.Product.GetByCondition(product2 => product2?.ID == orderItem.ProductID)
                 select new BO.OrderItem
                 {
                     ID = orderItem.ID,
                     Amount = orderItem.Amount,
                     Name = product.Name,
                     Price = orderItem.Price,
                     TotalPrice = orderItem.Price * orderItem.Amount
                 };
        order.ID = orderDal.ID;
        order.CustomerName = orderDal.CustomerName;
        order.CustomerAddress = orderDal.CustomerAdress;
        order.CustomerEmail = orderDal.CustomerEmail;
        order.OrderDate = orderDal.OrderDate;
        order.ShipDate = orderDal.ShipDate;
        order.DeliveryDate = orderDal.DeliveryrDate;
        order.TotalPrice = orderItems.Sum(o => o.TotalPrice);
        order.Items = orderItems.ToList();
        order.Status = BO.OrderStatus.SendOrder;
        return order;
    }


    /// <summary>
    /// function that tracks the order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order tracking</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.OrderTracking TrackingOrder(int id)
    {
        DO.Order order = new DO.Order();
        BO.OrderTracking orderTracking = new BO.OrderTracking();
        List<Tuple<DateTime?, string>> tList = new List<Tuple<DateTime?, string>>
            {
                new Tuple<DateTime?, string>(order.OrderDate, "the order has been created")
             };

        try
        {
            order = dal.Order.GetByCondition(order2=>order2?.ID==id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
        }

        orderTracking.ID = order.ID;
        if (order.DeliveryrDate != null&&order.DeliveryrDate < DateTime.Now)
            orderTracking.Status = BO.OrderStatus.ProvidedOrder;
        else
        {
            if (order.ShipDate != null&&order.ShipDate < DateTime.Now)
                orderTracking.Status = BO.OrderStatus.SendOrder;
            else
                orderTracking.Status = BO.OrderStatus.ConfirmedOrder;
        }
        if (order.ShipDate != null)
        {
            tList.Add(new Tuple<DateTime?, string>(order.ShipDate, "the order has been sent"));
        }
        if (order.DeliveryrDate != null)
        {
            tList.Add(new Tuple<DateTime?, string>(order.DeliveryrDate, "the order provided"));
        }
        orderTracking.Tuples = tList;
        return orderTracking;
    }


    /// <summary>
    /// Bonus:function that updates the quantity of a product in the order
    /// </summary>
    /// <param name="idOrder"></param>
    /// <param name="idProduct"></param>
    /// <param name="amount"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="Exception"></exception>
    public BO.OrderItem UpdateAmountOfOProductInOrder(int idOrder, int idProduct, int amount)
    {
        DO.Product product = new DO.Product();
        BO.OrderItem orderItem = new BO.OrderItem();
        DO.OrderItem item = new DO.OrderItem();
        try
        {
            item = dal.OrderItem.GetByCondition(item=>(item?.OrderID==idOrder&&item?.ProductID==idProduct));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException($"{ex.EntityName}  are does not exist", ex);
        }
        DO.Order order = new DO.Order();
        try
        {
            order = dal.Order.GetByCondition(order2=>order2?.ID==idOrder);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order does not exist", ex);
        }
        if (order.ShipDate != null && order.ShipDate < DateTime.Now)
        {
            throw new BO.BLImpossibleActionException("It is not possible to update an order after it has been sent");
        }
        if (amount < 0)
            throw new BO.BLInvalidInputException("invalid amount");
        try
        {
            product = dal.Product.GetByCondition(item2=>item2?.ID==item.ProductID);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }
        product.InStock += item.Amount;
        item.Amount = amount;
        try
        {
            dal.OrderItem.Update(item);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException($"{ex.EntityName} does not exist", ex);
        }
        product.InStock -= amount;
        try
        {
            dal.Product.Update(product);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(" product does not exist", ex);
        }

        if (amount == 0)
        {
            try
            {
                dal.OrderItem.Delete(item.ID);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("id does not exist", ex);
            }
            throw new Exception("The product remove from the order");
        }

        orderItem.ID = item.ID;
        orderItem.Amount = amount;
        orderItem.Price = item.Price;
        orderItem.TotalPrice = amount * item.Price;
        orderItem.ProductID = idProduct;
        orderItem.Name = product.Name;
        return orderItem;
    }
}

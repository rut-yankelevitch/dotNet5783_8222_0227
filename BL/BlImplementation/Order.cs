﻿using System.Linq;

namespace BlImplementation;

/// <summary>
/// A class that implements the iorder interface
/// </summary>
internal class Order : BlApi.IOrder
{

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
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == id);
            return returnBOOrder(orderDal);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order doesnot exist", ex);
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
        try
        {
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == id);
            if (orderDal.ShipDate != null && orderDal.ShipDate < DateTime.Now)
                throw new BO.BLImpossibleActionException("order send");
            orderDal.ShipDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order update failes", ex);
            }
            return returnBOOrder(orderDal);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order does not exist", ex);
        }
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
        try
        {
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == id);

            if (orderDal.DeliveryrDate != null && orderDal.DeliveryrDate < DateTime.Now)
                throw new BO.BLImpossibleActionException("order Delivery");

            if (orderDal.ShipDate == null || orderDal.ShipDate > DateTime.Now)
                throw new BO.BLImpossibleActionException("It is not possible to update a delivery date before a shipping date");

            orderDal.DeliveryrDate = DateTime.Now;
            try
            {
                dal.Order.Update(orderDal);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("order update failes", ex);
            }
            return returnBOOrder(orderDal);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order does not exist", ex);
        }
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
        List<Tuple<DateTime?, string>> tList = new ();
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
        tList.Add(new Tuple<DateTime?, string>(order.OrderDate, "the order has been created"));
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
        DO.Order order = new DO.Order();
        try
        {
            order = dal.Order.GetByCondition(order2=>order2?.ID==idOrder);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order does not exist", ex);
        }
        //if (order.ShipDate != null && order.ShipDate < DateTime.Now)
         if (order.ShipDate < DateTime.Now)
        {
            throw new BO.BLImpossibleActionException("It is not possible to update an order after it has been sent");
        }
        try
        {
            item = dal.OrderItem.GetByCondition(item => (item?.OrderID == idOrder && item?.ProductID == idProduct));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException($"{ex.EntityName}  are does not exist", ex);
        }
        if (amount < 0)
            throw new BO.BLInvalidInputException("invalid amount");
        if (amount == 0)
        {
            try
            {
                dal.OrderItem.Delete(item.ID);
                 int count=dal.OrderItem.GetAll(item=>item?.OrderID==idOrder).Count();
                if (count == 0)
                    dal.Order.Delete(idOrder);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("id does not exist", ex);
            }
            return orderItem;
        }
        else
        {
            try
            {
                product = dal.Product.GetByCondition(item2 => item2?.ID == item.ProductID);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BLDoesNotExistException("product does not exist", ex);
            }
                if (amount > product.InStock+ item.Amount)
                    throw new BO.BLImpossibleActionException("Amount not in stock");
            
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
                throw new BO.BLDoesNotExistException("product does not exist", ex);
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


    /// <summary>
    /// convert a DO.Order to a BO.Order 
    /// </summary>
    /// <param name="orderDal">a DO.order</param>
    /// <returns>a BO.order</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    private BO.Order returnBOOrder(DO.Order orderDal)
    {
        try
        {
            IEnumerable<DO.OrderItem?> orderItemsDal = dal.OrderItem.GetAll(ord => ord?.OrderID == orderDal.ID);

            var orderItems = from item in orderItemsDal
                             let orderItem = (DO.OrderItem)item
                             let product = dal.Product.GetByCondition(prod => prod?.ID == orderItem.ProductID)
                             select new BO.OrderItem
                             {
                                 ID = orderItem.ID,
                                 ProductID = orderItem.ProductID,
                                 Price = orderItem.Price,
                                 Amount = orderItem.Amount,
                                 TotalPrice = orderItem.Amount * orderItem.Price,
                                 Name = product.Name
                             };

            return new BO.Order
            {
                ID = orderDal.ID,
                CustomerName = orderDal.CustomerName,
                CustomerAddress = orderDal.CustomerAdress,
                CustomerEmail = orderDal.CustomerEmail,
                OrderDate = orderDal.OrderDate,
                ShipDate = orderDal.ShipDate,
                DeliveryDate = orderDal.DeliveryrDate,
                TotalPrice = orderItems.Sum(item => item.TotalPrice),
                Items = orderItems.ToList(),
                Status = (((DO.Order)orderDal!).DeliveryrDate != null && ((DO.Order)orderDal!).DeliveryrDate < DateTime.Now) ?
                                                BO.OrderStatus.ProvidedOrder : ((DO.Order)orderDal!).ShipDate != null && ((DO.Order)orderDal!).ShipDate < DateTime.Now ?
                                                BO.OrderStatus.SendOrder : BO.OrderStatus.ConfirmedOrder
            };
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new  BO.BLDoesNotExistException("order items doesnot exist", ex);

        }

    }

}

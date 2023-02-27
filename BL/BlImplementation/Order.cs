using System.Linq;
using System.Runtime.CompilerServices;

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
    [MethodImpl(MethodImplOptions.Synchronized)]
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
                                    Status = (((DO.Order)order!).DeliveryDate != null && ((DO.Order)order!).DeliveryDate < DateTime.Now) ?
                                                BO.OrderStatus.Provided_Order : ((DO.Order)order!).ShipDate != null && ((DO.Order)order!).ShipDate < DateTime.Now ?
                                                BO.OrderStatus.Send_Order : BO.OrderStatus.Confirmed_Order
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
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order GetOrderById(int id)
    {
        try
        {
            DO.Order orderDal = dal.Order.GetByCondition(ord => ord?.ID == id);
            return returnBOOrder(orderDal);
        }
        catch (DO.DalDoesNotExistException ex) {throw new BO.BLDoesNotExistException("order doesnot exist", ex); }
    }


    /// <summary>
    /// function that update the send order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLMistakeUpdateException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]

    public BO.Order UpdateSendOrderByManager(int id)
    {
        DO.Order orderDal;
        try { orderDal = dal.Order.GetByCondition(ord => ord?.ID == id); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("order does not exist", ex); }

        if (orderDal.ShipDate != null && orderDal.ShipDate < DateTime.Now)
            throw new BO.BLImpossibleActionException("order send");
        orderDal.ShipDate = DateTime.Now;

        try { dal.Order.Update(orderDal); }

        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("order update failes", ex); }
        return returnBOOrder(orderDal);
    }


    /// <summary>
    ///  function that update the supply order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>update order</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateSupplyOrderByManager(int id)
    {
        DO.Order orderDal;

        try { orderDal = dal.Order.GetByCondition(ord => ord?.ID == id); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("order does not exist", ex); }

        if (orderDal.DeliveryDate != null && orderDal.DeliveryDate < DateTime.Now)
            throw new BO.BLImpossibleActionException("order Delivery");

        if (orderDal.ShipDate == null || orderDal.ShipDate > DateTime.Now)
            throw new BO.BLImpossibleActionException("It is not possible to update a delivery date before a shipping date");

        orderDal.DeliveryDate = DateTime.Now;

        try { dal.Order.Update(orderDal); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("order update failes", ex); }

        return returnBOOrder(orderDal);

    }


    /// <summary>
    /// function that tracks the order
    /// </summary>
    /// <param name="id"></param>
    /// <returns>order tracking</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderTracking TrackingOrder(int id)
    {
        DO.Order order = new DO.Order();
        BO.OrderTracking orderTracking = new BO.OrderTracking();
        List<Tuple<DateTime?, string>> tList = new();

        try { order = dal.Order.GetByCondition(order2 => order2?.ID == id); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("order doesnot exist", ex); }

        orderTracking.ID = order.ID;
        if (order.DeliveryDate != null && order.DeliveryDate < DateTime.Now)
            orderTracking.Status = BO.OrderStatus.Provided_Order;
        else
        {
            if (order.ShipDate != null && order.ShipDate < DateTime.Now)
                orderTracking.Status = BO.OrderStatus.Send_Order;
            else
                orderTracking.Status = BO.OrderStatus.Confirmed_Order;
        }
        tList.Add(new Tuple<DateTime?, string>(order.OrderDate, "the order has been created"));
        if (order.ShipDate != null)
        {
            tList.Add(new Tuple<DateTime?, string>(order.ShipDate, "the order has been sent"));
        }
        if (order.DeliveryDate != null)
        {
            tList.Add(new Tuple<DateTime?, string>(order.DeliveryDate, "the order provided"));
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
    [MethodImpl(MethodImplOptions.Synchronized)]

    public BO.Order? UpdateAmountOfOProductInOrder(int idOrder, int idProduct, int amount)
    {
        try
        {
            DO.Product product = new DO.Product();
            BO.OrderItem orderItem = new BO.OrderItem();
            DO.OrderItem item = new DO.OrderItem();
            DO.Order order = new DO.Order();
            BO.Order BOOrder = new BO.Order();

            try { order = dal.Order.GetByCondition(order2 => order2?.ID == idOrder); }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("order does not exist", ex); }

            if (order.ShipDate < DateTime.Now)
            {
                throw new BO.BLImpossibleActionException("It is not possible to update an order after it has been sent");
            }

            try { item = dal.OrderItem.GetByCondition(item => (item?.OrderID == idOrder && item?.ProductID == idProduct)); }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException($"{ex.EntityName}  are does not exist", ex); }

            try { product = dal.Product.GetByCondition(item2 => item2?.ID == item.ProductID); }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("product does not exist", ex); }

            if (amount < 0)
                throw new BO.BLInvalidInputException("invalid amount");

            if (amount > product.InStock + item.Amount)
                throw new BO.BLImpossibleActionException("Amount not in stock");

            product.InStock += item.Amount;
            item.Amount = amount;
            lock (dal) { 
            try { dal.OrderItem.Update(item); }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException($"{ex.EntityName}  are does not exist", ex); }
            product.InStock -= amount;

            try { dal.Product.Update(product); }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("product does not exist", ex); }
            if (amount == 0)
            {
                try { dal.OrderItem.Delete(item.ID); }
                catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("id does not exist", ex); }
            }

            BOOrder = returnBOOrder(order);

            if (BOOrder?.Items!.Count == 0)
            {
                try { dal.Order.Delete(idOrder); }
                catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("id does not exist", ex); }
                return null;
            }
        }

            return BOOrder; 
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException($"{ex.EntityName}  are does not exist", ex);
        }
    }


    /// <summary>
    /// convert a DO.Order to a BO.Order 
    /// </summary>
    /// <param name="orderDal">a DO.order</param>
    /// <returns>a BO.order</returns>
    /// <exception cref="DoesNotExistedBlException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
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
                DeliveryDate = orderDal.DeliveryDate,
                TotalPrice = orderItems.Sum(item => item.TotalPrice),
                Items = orderItems.ToList(),
                Status = (((DO.Order)orderDal!).DeliveryDate != null && ((DO.Order)orderDal!).DeliveryDate < DateTime.Now) ?
                                                BO.OrderStatus.Provided_Order : ((DO.Order)orderDal!).ShipDate != null && ((DO.Order)orderDal!).ShipDate < DateTime.Now ?
                                                BO.OrderStatus.Send_Order : BO.OrderStatus.Confirmed_Order
            };
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new  BO.BLDoesNotExistException("order items doesnot exist", ex);

        }

    }
    public int? SelectOrder()
    {
        DateTime? minDate = DateTime.MaxValue;
        int? minOrderId = -1;
        List<DO.Order?>? oList = dal.Order.GetAll( o =>  o?.DeliveryDate == null)?.ToList();
        foreach (var item in oList!)
        {
            if (item?.ShipDate == null)
            {
                if (item?.OrderDate < minDate)
                {
                    minDate = item?.OrderDate;
                    minOrderId = item?.ID;
                }
            }
            else
            {
                if (item?.ShipDate < minDate)
                {
                    minDate = item?.ShipDate;
                    minOrderId = item?.ID;
                }
            }
        }
        return minDate == DateTime.MaxValue ? null : minOrderId;
    }
}

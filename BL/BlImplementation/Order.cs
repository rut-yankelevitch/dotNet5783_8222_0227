using System;
using BlApi;
using DalApi;
using System.Collections.Generic;
using BO;

namespace BlImplementation;

/// <summary>
/// Summary description for Class1
/// </summary>
internal class Order: BlApi.IOrder
{
    private IDal dal = new DalList.DalList();
    public IEnumerable<BO.OrderForList> GetOrderList()
    {
        try
        {
            List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
            IEnumerable<DO.Order> orders = dal.Order.GetAll();
            double totalPrice = 0;
            int amount = 0;
            foreach (DO.Order order in orders)
            {
                BO.OrderForList orderForList = new BO.OrderForList();
                IEnumerable<DO.OrderItem> orderitems = dal.OrderItem.GetAllItemsByOrderId(order.ID);
                foreach (DO.OrderItem item in orderitems)
                {
                    totalPrice += item.Amount * item.Price;
                    amount += item.Amount;
                }
                orderForList.ID = order.ID;
                orderForList.CustomerName = order.CustomerName;
                orderForList.AmountOfItems = amount;
                orderForList.TotalPrice = totalPrice;


                if (order.DeliveryrDate < DateTime.Now && order.DeliveryrDate != DateTime.MinValue)
                    orderForList.Status = BO.OrderStatus.PROVIDED_ORDER;
                else
                {
                    if (order.ShipDate < DateTime.Now && order.ShipDate != DateTime.MinValue)
                        orderForList.Status = BO.OrderStatus.SEND_ORDER;
                    else
                        orderForList.Status = BO.OrderStatus.CONFIRMED_ORDER;
                }
                ordersForList.Add(orderForList);
            }

            return ordersForList;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public BO.Order GetOrderById(int id)
    {
        try
        {
            BO.Order order = new BO.Order();
            DO.Product product = new DO.Product();
            List<BO.OrderItem> orderitems = new List<BO.OrderItem>();
            DO.Order orderDal = dal.Order.GetById(id);
            IEnumerable<DO.OrderItem> orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
            double totalPrice = 0;

            foreach (DO.OrderItem item in orderitemsDal)
            {
                BO.OrderItem orderitem = new BO.OrderItem();
                orderitem.ID = item.ID;
                orderitem.ProductID = item.ProductID;
                orderitem.Price = item.Price;
                orderitem.Amount = item.Amount;
                orderitem.TotalPrice = item.Amount * item.Price;
                product = dal.Product.GetById(orderitem.ProductID);
                orderitem.Name = product.Name;
                orderitems.Add(orderitem);
                totalPrice += orderitem.TotalPrice;

            }
            order.ID = orderDal.ID;
            order.CustomerName = orderDal.CustomerName;
            order.CustomerAddress = orderDal.CustomerAdress;
            order.CustomerEmail = orderDal.CustomerEmail;
            order.OrderDate = orderDal.OrderDate;
            order.ShipDate = orderDal.ShipDate;
            order.DeliveryDate = orderDal.DeliveryrDate;
            order.TotalPrice = totalPrice;
            order.Items = orderitems;
            if (orderDal.DeliveryrDate < DateTime.Now&& orderDal.DeliveryrDate!=DateTime.MinValue)
                order.Status = BO.OrderStatus.PROVIDED_ORDER;
            else
            {
                if (order.ShipDate < DateTime.Now&&orderDal.ShipDate != DateTime.MinValue)
                    order.Status = BO.OrderStatus.SEND_ORDER;
                else
                    order.Status = BO.OrderStatus.CONFIRMED_ORDER;
            }
            return order;
        }
        catch (Exception ex)
        {
            throw new BO.NotExistException(ex.Message);
        }

    }
    public BO.Order UpdateSendOrderByManager(int id)
    {
        try
        {
            BO.Order order = new BO.Order();
            DO.Product product = new DO.Product();
            List<BO.OrderItem> orderitems = new List<BO.OrderItem>();
            DO.Order orderDal = dal.Order.GetById(id);
            if (orderDal.ShipDate < DateTime.Now&&orderDal.ShipDate!=DateTime.MinValue)
                throw new BO.ImpossibleActionException("order send");
            else
            {
                orderDal.ShipDate=DateTime.Now;    
                dal.Order.Update(orderDal);
            }
            IEnumerable<DO.OrderItem> orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
            double totalPrice = 0;
             
            foreach (DO.OrderItem item in orderitemsDal)
            {
                BO.OrderItem orderitem = new BO.OrderItem();
                orderitem.ID = item.ID;
                orderitem.ProductID = item.ProductID;
                orderitem.Price = item.Price;
                orderitem.Amount = item.Amount;
                orderitem.TotalPrice = item.Amount * item.Price;
                product = dal.Product.GetById(orderitem.ProductID);
                orderitem.Name = product.Name;
                orderitems.Add(orderitem);
                totalPrice += orderitem.TotalPrice;

            }
            order.ID = orderDal.ID;
            order.CustomerName = orderDal.CustomerName;
            order.CustomerAddress = orderDal.CustomerAdress;
            order.CustomerEmail = orderDal.CustomerEmail;
            order.OrderDate = orderDal.OrderDate;
            order.ShipDate = orderDal.ShipDate;
            order.DeliveryDate = orderDal.DeliveryrDate;
            order.TotalPrice = totalPrice;
            order.Items = orderitems;
            order.Status = BO.OrderStatus.SEND_ORDER;
            return order;
        }
        catch (Exception ex)
        {
            throw new BO.NotExistException(ex.Message);
        }

    }
    public BO.Order UpdateSupplyOrderByManager(int id)
    {
        try
        {
            BO.Order order = new BO.Order();
            DO.Product product = new DO.Product();
            List<BO.OrderItem> orderitems = new List<BO.OrderItem>();


            DO.Order orderDal = dal.Order.GetById(id);
            if (orderDal.DeliveryrDate < DateTime.Now&& orderDal.DeliveryrDate != DateTime.MinValue)
                throw new BO.ImpossibleActionException("order Delivery");
            else
            {
                if (orderDal.ShipDate >DateTime.Now)
                    throw new BO.ImpossibleActionException("It is not possible to update a delivery date before a shipping date");
                orderDal.DeliveryrDate = DateTime.Now;
                dal.Order.Update(orderDal);
            }
            IEnumerable<DO.OrderItem> orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
            double totalPrice = 0;

            foreach (DO.OrderItem item in orderitemsDal)
            {
                BO.OrderItem orderitem = new BO.OrderItem();
                orderitem.ID = item.ID;
                orderitem.ProductID = item.ProductID;
                orderitem.Price = item.Price;
                orderitem.Amount = item.Amount;
                orderitem.TotalPrice = item.Amount * item.Price;
                product = dal.Product.GetById(orderitem.ProductID);
                orderitem.Name = product.Name;
                orderitems.Add(orderitem);
                totalPrice += orderitem.TotalPrice;

            }
            order.ID = orderDal.ID;
            order.CustomerName = orderDal.CustomerName;
            order.CustomerAddress = orderDal.CustomerAdress;
            order.CustomerEmail = orderDal.CustomerEmail;
            order.OrderDate = orderDal.OrderDate;
            order.ShipDate = orderDal.ShipDate;
            order.DeliveryDate = orderDal.DeliveryrDate;
            order.TotalPrice = totalPrice;
            order.Items = orderitems;
            order.Status = BO.OrderStatus.PROVIDED_ORDER;
            return order;
        }
        catch (Exception ex)
        {
            throw new BO.NotExistException(ex.Message);
        }

    }
    public BO.OrderTracking TrackingOrder(int id)
    {
        try
        {
            DO.Order order = new DO.Order();
            BO.OrderTracking orderTracking = new BO.OrderTracking();
            order = dal.Order.GetById(id);
            orderTracking.ID = order.ID;
            if (order.DeliveryrDate < DateTime.Now&&order.DeliveryrDate!=DateTime.MinValue)
                orderTracking.Status = BO.OrderStatus.PROVIDED_ORDER;
            else
            {
                if (order.ShipDate < DateTime.Now&&order.ShipDate!=DateTime.MinValue)
                    orderTracking.Status = BO.OrderStatus.SEND_ORDER;
                else
                    orderTracking.Status = BO.OrderStatus.CONFIRMED_ORDER;
            }
            List<Tuple<DateTime, string>> tList = new List<Tuple<DateTime, string>>
            
            { 
                new Tuple<DateTime, string>(order.OrderDate, "the order has been created")
             };
            if (order.ShipDate != DateTime.MinValue)
            {
                tList.Add(new Tuple<DateTime, string>(order.ShipDate, "the order has been sent"));
            }
            if (order.DeliveryrDate != DateTime.MinValue)
            {
                tList.Add(new Tuple<DateTime, string>(order.DeliveryrDate, "the order provided"));
            }
            orderTracking.Tuples = tList;
            return orderTracking;
        }
        catch(Exception ex)
        {
            throw new BO.NotExistException(ex.Message);
        }
    }
    //?????????????????????
    public BO.OrderItem UpdateAmountOfOProductInOrder(int idOrder, int idProduct, int amount)
    {
        try
        {
            DO.OrderItem item = new DO.OrderItem();
            item = dal.OrderItem.GetByOrderIdAndProductId(idOrder, idProduct);
            DO.Order order = new DO.Order();
            order = dal.Order.GetById(idOrder);
            DO.Product product = new DO.Product();
            BO.OrderItem orderItem = new BO.OrderItem();
            if (order.ShipDate!=DateTime.MinValue&&order.ShipDate < DateTime.Now)
            {
                throw new BO.ImpossibleActionException("It is not possible to update an order after it has been sent");
            }
            if (amount < 0)
                throw new BO.InvalidInputException("invalid amount");
            
            product = dal.Product.GetById(item.ProductID);
            product.InStock += item.Amount;
            item.Amount = amount;
            dal.OrderItem.Update(item);
            product.InStock-=amount;
            dal.Product.Update(product);
            //????????????????????????????
            if (amount == 0)
            {
                dal.OrderItem.Delete(item.ID);
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
        catch (Exception ex)
        {
            throw new BO.NotExistException(ex.Message);
        }
    }

}

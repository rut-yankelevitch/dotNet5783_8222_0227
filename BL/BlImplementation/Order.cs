using System;
using BlApi;
using DalApi;
using System.Collections.Generic;

namespace BlImplementation;

/// <summary>
/// Summary description for Class1
/// </summary>
internal class Order: BlApi.IOrder
{
    private IDal dal = new DalList.DalList();
    public IEnumerable<BO.OrderForList> GetOrderList()
    {
        List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
        IEnumerable<DO.Order> orders = dal.Order.GetAll();
        BO.OrderForList orderForList = new BO.OrderForList();
        double totalPrice = 0;
        int amount = 0;
        foreach (DO.Order order in orders)
        {
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


            if (order.DeliveryrDate < DateTime.Now)
                orderForList.Status = BO.OrderStatus.PROVIDED_ORDER;
            else
            {
                if (order.ShipDate < DateTime.Now)
                    orderForList.Status = BO.OrderStatus.SEND_ORDER;
                else
                    orderForList.Status = BO.OrderStatus.CONFIRMED_ORDER;
            }
            ordersForList.Add(orderForList);
        }

        return ordersForList;
    }
    public BO.Order GetOrderById(int id)
    {
        try
        {
            BO.Order order = new BO.Order();
            BO.OrderItem orderitem = new BO.OrderItem();
            DO.Product product = new DO.Product();
            List<BO.OrderItem> orderitems = new List<BO.OrderItem>();
            DO.Order orderDal = dal.Order.GetById(id);
            IEnumerable<DO.OrderItem> orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
            double totalPrice = 0;

            foreach (DO.OrderItem item in orderitemsDal)
            {
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
            if (orderDal.DeliveryrDate < DateTime.Now)
                order.Status = BO.OrderStatus.PROVIDED_ORDER;
            else
            {
                if (order.ShipDate < DateTime.Now)
                    order.Status = BO.OrderStatus.SEND_ORDER;
                else
                    order.Status = BO.OrderStatus.CONFIRMED_ORDER;
            }
            //מה אמורים למלא? לא כתוב בתיאור הכללי
            //order.PaymentDate=


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
            BO.OrderItem orderitem = new BO.OrderItem();
            DO.Product product = new DO.Product();
            List<BO.OrderItem> orderitems = new List<BO.OrderItem>();


            DO.Order orderDal = dal.Order.GetById(id);
            if (orderDal.ShipDate < DateTime.Now)
                throw new BO.MistakeUpdateException("order send");
            IEnumerable<DO.OrderItem> orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
            double totalPrice = 0;

            foreach (DO.OrderItem item in orderitemsDal)
            {
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
            //מה אמורים למלא? לא כתוב בתיאור הכללי
            //order.PaymentDate=
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
            BO.OrderItem orderitem = new BO.OrderItem();
            DO.Product product = new DO.Product();
            List<BO.OrderItem> orderitems = new List<BO.OrderItem>();


            DO.Order orderDal = dal.Order.GetById(id);
            if (orderDal.DeliveryrDate < DateTime.Now)
                throw new BO.MistakeUpdateException("order Deliveryr");
            IEnumerable<DO.OrderItem> orderitemsDal = dal.OrderItem.GetAllItemsByOrderId(id);
            double totalPrice = 0;

            foreach (DO.OrderItem item in orderitemsDal)
            {
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
            //מה אמורים למלא? לא כתוב בתיאור הכללי
            //order.PaymentDate=
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
            if (order.DeliveryrDate < DateTime.Now)
                orderTracking.Status = BO.OrderStatus.PROVIDED_ORDER;
            else
            {
                if (order.ShipDate < DateTime.Now)
                    orderTracking.Status = BO.OrderStatus.SEND_ORDER;
                else
                    orderTracking.Status = BO.OrderStatus.CONFIRMED_ORDER;
            }
            List<Tuple<DateTime, string>> tList = new List<Tuple<DateTime, string>>
            { new Tuple<DateTime, string>(order.OrderDate, "the order has been created"),
              new Tuple<DateTime, string>(order.ShipDate, "the order has been sent"),
              new Tuple<DateTime, string>(order.DeliveryrDate, "the order provided")
             };
            return orderTracking;
        }
        catch(Exception ex)
        {
            throw new BO.NotExistException(ex.Message);
        }


        


    }

}

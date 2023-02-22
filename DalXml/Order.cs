using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal;

///////////////////////////////////////////
//implement ILecturer with XML Serializer
//////////////////////////////////////////
internal class Order : IOrder
{
    const string s_order = @"..\..\xml\Order.xml";        //XML Serializer
    public IEnumerable<DO.Order?> GetAll(Func<DO.Order?, bool>? predicate)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);

       return ((predicate == null)?
             listOrders.Select(lec => lec).OrderBy(lec => lec?.ID):
             listOrders.Where(predicate).OrderBy(lec => lec?.ID)) ?? throw new DO.DalDoesNotExistException("The requested orders were not found");;
    }


    public DO.Order GetByCondition(Func<DO.Order?,bool>predicate)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);

        return listOrders.FirstOrDefault(predicate) ??
        throw new DalDoesNotExistException("The requested order was not found.");
    }


    public int Add(DO.Order order)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);
        
        if (listOrders.FirstOrDefault(ord => ord?.ID == order.ID) != null)
            throw new DO.DalAlreadyExistException(order.ID, "Order"); 
        order.ID = (int)((listOrders.Last()?.ID) + 1)!;
        listOrders.Add(order);
        XMLTools.SaveListToXMLSerializer(listOrders, s_order);
        return order.ID;
    }

    public void Delete(int id)
    {
        List<DO.Order?> listOrders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_order);

        if (listOrders.RemoveAll(ord => ord?.ID == id) == 0)
            throw new DalDoesNotExistException(id, "order");
        XMLTools.SaveListToXMLSerializer(listOrders, s_order);
    }


    public void Update(DO.Order order)
    {
        Delete(order.ID);
        Add(order);
    }
}


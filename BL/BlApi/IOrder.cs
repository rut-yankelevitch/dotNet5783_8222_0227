using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IOrder
    {
        /*
    public int Add(Order order);
	public void Update(Order order );
	public void Delete(int id);
	public Order GetById(int id);
    public IEnumerable <Oredr> GetAll() ;*/
        public IEnumerable<OrderForList> GetOrderListForManager();
        public Order GetOrderById(int id);
        public Order UpdateSendOrderByManager(int id);
        public Order UpdateSupplyOrderByManager(int id);
        public OrderTracking TrackingOrder (int id);
    }
}

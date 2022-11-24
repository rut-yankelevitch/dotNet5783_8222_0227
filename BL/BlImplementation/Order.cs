using System;
using BlApi;
using DalApi;

namespace BlImplementation;

/// <summary>
/// Summary description for Class1
/// </summary>
internal class Order
{
    private IDal Dal = new DalList.DalList();
    public Order() : IOrder
	{
		//
		// TODO: Add constructor logic here
		//
	}
}

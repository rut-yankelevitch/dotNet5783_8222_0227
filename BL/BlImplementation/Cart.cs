using System;
using BlApi;
using DalApi;
namespace BlImplementation;

/// <summary>
/// Summary description for Class1
/// </summary>
internal class Cart : ICart
{
    private IDal Dal = new DalList.DalList();
    public Cart()
	{

		//
		// TODO: Add constructor logic here
		//
	}
}

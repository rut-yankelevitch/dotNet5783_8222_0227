﻿using DO;
using static Dal.DataSource;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class ProductDal:IProduct
{
    /// <summary>
    /// add a product to the product array
    /// </summary>
    /// <param name="product">the new order</param>
    /// <returns>the insert new product id</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Product product)
    {
        if (CheckIfExist(product.ID))
            throw new DalAlreadyExistException(product.ID, "product");
        ProductList.Add(product);   
        return product.ID;
    }


    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        int count = ProductList.RemoveAll(prod => prod?.ID == id);
        if (count == 0)
            throw new DalDoesNotExistException(id,"product");
    }


    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Product product)
    {
        Delete(product.ID);
        Add(product);
    }


    /// <summary>
    /// get all products
    /// </summary>
    /// <returns>an array of all the products</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Product?> GetAll(Func<Product?, bool>? predicate) =>
    (predicate == null ? ProductList.Select(item => item) : ProductList.Where(predicate!)) ??
        throw new DO.DalDoesNotExistException("The requested products were not found.");


    /// <summary>
    /// get product by predicate
    /// </summary>
    /// <param name="predicate">the order id</param>
    /// <returns>the product</returns>
    /// <exception cref="Exception">if the product doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Product GetByCondition(Func<Product?, bool> predicate)=>
        ProductList.FirstOrDefault(predicate)??
        throw new DalDoesNotExistException("The requested product was not found.");


    /// <summary>
    /// the function check if the product exist
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private bool CheckIfExist(int id)
    {
        return ProductList.Any(item => item?.ID == id);
    }
}

using System;

using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;
/// <summary>
/// Summary description for BOProduct
/// </summary>
/// 

internal class Product : IProduct
{
    private IDal dal = new DalList.DalList();
    public IEnumerable<ProductForList> GetProductListForManager() 
    {
        //list
        IEnumerable<DO.Product> products =dal.Product.GetAll();
        List<ProductForList> productsForList = new List<ProductForList>();
        ProductForList productForList = new ProductForList();

        foreach(DO.Product product in products)
        {
            productForList.ID=product.ID;
            productForList.Name=product.Name;       
            productForList.Price=product.Price;
            productForList.Category = (Category)product.Category;
            productsForList.Add(productForList);
        }
 
    }
    public BO.Product GetProductByIdForManager(int id)
    {
        try
        {
            
            DO.Product product = dal.Product.GetById(id);
            BO.Product product1 = new BO.Product();
            product1.ID=product.ID;
            product1.Name=product.Name; 
            product1.Price=product.Price;
            product1.Category = (Category)product.Category;
            product1.InStock=product.InStock;   

            return product1;

        }
        catch (Exception msg)
        {
            throw new BO.BLDoesNotExistException(msg.Message);
        }

    }
    public BO.Product AddProduct(BO.Product product)
    {
        try
        {
            DO.Product product1 = new DO.Product();
            product1.ID = product.ID;
            product1.Name = product.Name;
            product1.Price = product.Price;
            product1.Category = (DO.Category)product.Category;
            product1.InStock = product.InStock;
            dal.Product.Add(product1);
            return product;
        }
        catch(Exception exce)
        {
            throw new BO.AlreadyExistException(exce.Message);
        }
    }
    public void DeleteProduct(int id)
    {

    }
    public Product UpdateProduct(Product product) { }
    public IEnumerable<ProductItem> GetProductListForCustomer() { }
    public ProductItem GetProductByIdForCustomer(int id) { }


}

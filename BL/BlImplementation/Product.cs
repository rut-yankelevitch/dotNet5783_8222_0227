﻿using System;

using BlApi;

using DalApi;

using IProduct = BlApi.IProduct;

namespace BlImplementation;
/// <summary>
/// Summary description for BOProduct
/// </summary>
/// 

internal class Product :IProduct
{
    private IDal dal = new DalList.DalList();
    public IEnumerable<BO.ProductForList> GetProductListForManager() 
    {
        //list
        IEnumerable<DO.Product> products = dal.Product.GetAll();
        List<BO.ProductForList> productsForList = new List<BO.ProductForList>();

        foreach(DO.Product product in products)
        {
            BO.ProductForList productForList = new BO.ProductForList();
            productForList.ID=product.ID;
            productForList.Name=product.Name;       
            productForList.Price=product.Price;
            productForList.Category = (BO.Category)product.Category;
            productsForList.Add(productForList);
        }
        
      return productsForList;    
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
            product1.Category = (BO.Category)product.Category;
            product1.InStock=product.InStock;   
            return product1;
        }
        catch (Exception msg)
        {
            throw new BO.NotExistException(msg.Message);
        }

    }
    public BO.Product AddProduct(BO.Product product)
    {
        try
        {
            if (product.ID <1 ||product.Name == "" || product.Price <1  || product.InStock < 0|| (int)product.Category>5||(int)product.Category<0)
            {
                throw new BO.InvalidInputException(" Invalid input");
            }
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
        try
        {
            IEnumerable<DO.OrderItem> orderstems = dal.OrderItem.GetAll();
            foreach (DO.OrderItem orderItem in orderstems)
            {
                if (orderItem.ProductID == id)
                {
                    throw new BO.ImpossibleActionException("product exist in order");
                }
            }
            dal.Product.Delete(id);
        }
        catch (Exception exce)
        {
            throw new BO.NotExistException(exce.Message);
        }
        //IEnumerable<DO.Order> orders = dal.Order.GetAll();
        //int orderID;
        //DO.OrderItem item;
        //foreach (DO.Order order in orders)
        //{
        //    orderID = order.ID;
        //    item= dal.OrderItem.GetById(orderID); 
        //}
        //throw new BO.AlreadyExistException("product exsit");
    }
    public BO.Product UpdateProduct(BO.Product product)
    {
        try
        {
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0)
                throw new BO.InvalidInputException(" Invalid input");
            {
                DO.Product product1 = new DO.Product();
                product1.ID = product.ID;   
                product1.Name = product.Name;   
                product1.Price=product.Price;
                product1.InStock = product.InStock;
                product1.Category=(DO.Category)product.Category;
                dal.Product.Update(product1);
            }

            return product;
        }
        catch (Exception exce)
        {
            throw new BO.NotExistException(exce.Message);
        }

    }
    public IEnumerable<BO.ProductItem> GetProductListForCustomer() 
    {
        try
        {
            
            IEnumerable<DO.Product> products = dal.Product.GetAll();
            List<BO.ProductItem> productsItems = new List<BO.ProductItem>();
            foreach (DO.Product product in products)
            {
                BO.ProductItem productItem = new BO.ProductItem();
                productItem.ID = product.ID;
                productItem.Name = product.Name;
                productItem.Price=product.Price;
                productItem.Category=(BO.Category)product.Category;
                productItem.Amount = product.InStock;
                if(product.InStock > 0)
                {
                    productItem.Instock = true;
                }
                else
                {
                    productItem.Instock = false;    
                }

                productsItems.Add(productItem);
            }
            return productsItems;
        }

        catch (Exception exce)
        {
            throw new BO.NotExistException(exce.Message);
        }
      
    }
    public BO.ProductItem GetProductByIdForCustomer(int id)
    {
        try
        {
            BO.ProductItem productItem = new BO.ProductItem();
            DO.Product product1= new DO.Product();
            product1 = dal.Product.GetById(id);
            productItem.ID = product1.ID;
            productItem.Name = product1.Name;
            productItem.Price = product1.Price;
            productItem.Category=(BO.Category) product1.Category;
            productItem.Amount = product1.InStock;
            if (product1.InStock > 0)
                productItem.Instock = true;
            else
                productItem.Instock = false;
            return productItem;
        }

        catch(Exception exce)
        {
            throw new BO.NotExistException(exce.Message);
        }
    }
}

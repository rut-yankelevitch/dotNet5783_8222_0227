using System.Data;
using System.Net;
using System.Security.Cryptography;
using BO;
using DO;
using IProduct = BlApi.IProduct;
namespace BlImplementation;

/// <summary>
/// A class that implements the iproduct interface
/// </summary>
internal class Product : IProduct
{
    private DalApi.IDal dal = DalApi.Factory.Get();
    const int amount = 8;
    /// <summary>
    /// Definition of a function that returns a list of product by category for the manager
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public IEnumerable<BO.ProductForList> GetProductListForManagerByCategory(BO.Category? category)
    {
        return GetProductListForManager(BO.Filter.FilterByCategory, category);
    }

    /// <summary>
    /// a help function that return a list of product by filter
    /// </summary>
    /// <param name="filter1"></param>
    /// <param name="filterValue"></param>
    /// <returns></returns>
    public IEnumerable<BO.ProductItem> GetPopularProductList()
    {
        IEnumerable<DO.OrderItem?> orderItems;
        try
        {
            orderItems = dal.OrderItem.GetAll();
        }
        catch (DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("order item does not exist", ex);
        }
        var popularItems = from orderItem in orderItems
                               group orderItem by orderItem?.ProductID into orderItemGroup
                               select new { ID = orderItemGroup.Key, Items = orderItemGroup };
        popularItems = popularItems.OrderByDescending(p => p.Items.Count()).Take(amount);

        try { 
        var popularProducts = from popularItem in popularItems
                                  let DOProduct = dal.Product.GetByCondition(prod => prod?.ID == popularItem?.ID)
                                  select new BO.ProductItem
                                  {
                                      ID = DOProduct.ID,
                                      Name = DOProduct.Name,
                                      Category = (BO.Category)DOProduct.Category,
                                      Price = DOProduct.Price,
                                      Amount = 0,
                                      Instock = DOProduct.InStock > 0 ? true : false
                                  };
            return popularProducts;
        }
        catch (DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }
    }

        /// <summary>
        /// Definition of a function that returns the list of product for manager
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.ProductForList> GetProductListForManagerNoFilter()
    {
        return GetProductListForManager();
    }


    /// <summary>
    /// a help function that return a list of product by filter
    /// </summary>
    /// <param name="filter1"></param>
    /// <param name="filterValue"></param>
    /// <returns></returns>
    public IEnumerable<BO.ProductForList> GetProductListForManager(Filter filter1 = BO.Filter.None, object? filterValue = null)
    {
        IEnumerable<DO.Product?> products;
        Filter filter = filter1;
        switch (filter)
        {
            case Filter.FilterByCategory:
                products = dal.Product.GetAll(product => product?.Category == (filterValue != null ? (DO.Category)filterValue : product?.Category));
                break;
            case Filter.None:
                products = dal.Product.GetAll();
                break;
            default:
                products = dal.Product.GetAll();
                break;
        }
        return from pro in products
               let product = (DO.Product)pro
               select new ProductForList
               {
                   ID = product.ID,
                   Name = product.Name,
                   Price = product.Price,
                   Category = (BO.Category)product.Category,
               };
    }


    /// <summary>
    /// function that returns a product by id for the manager
    /// </summary>
    /// <param name="id"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Product GetProductByIdForManager(int id)
    {
        try
        {
            DO.Product product = dal.Product.GetByCondition(product2 => product2?.ID == id);
            BO.Product product1 = new BO.Product();
            product1.ID = product.ID;
            product1.Name = product.Name;
            product1.Price = product.Price;
            product1.Category = (BO.Category)product.Category;
            product1.InStock = product.InStock;
            return product1;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }

    }


    /// <summary>
    /// function that add a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BO.BLAlreadyExistException"></exception>
    public BO.Product AddProduct(BO.Product product)
    {
        try
        {
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0)
            {
                throw new BO.BLInvalidInputException("Invalid input");
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
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BLAlreadyExistException("product already exist", ex);
        }
    }


    /// <summary>
    /// function that delete a product
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public void DeleteProduct(int id)
    {
        try
        {
            IEnumerable<DO.OrderItem?> orderItems = dal.OrderItem.GetAll();
            DO.OrderItem? orderItem = orderItems.FirstOrDefault(item => ((DO.OrderItem)item!).ProductID == id);
            if (orderItem != null)
                throw new BO.BLImpossibleActionException($"product {id} exist in order {orderItem?.OrderID}");
            dal.Product.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("prouct does not exist", ex);
        }
    }


    /// <summary>
    /// function that update a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>update product</returns>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.Product UpdateProduct(BO.Product product)
    {
        try
        {
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0)
                throw new BO.BLInvalidInputException(" Invalid input");
            {
                DO.Product product1 = new DO.Product();
                product1.ID = product.ID;
                product1.Name = product.Name;
                product1.Price = product.Price;
                product1.InStock = product.InStock;
                product1.Category = (DO.Category)product.Category;

                dal.Product.Update(product1);
            }
            return product;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }
    }

 
    /// <summary>
    /// Definition of a function that returns a list of product by category for the catalog
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public IEnumerable<BO.ProductItem> GetProducItemForCatalogByCategory(BO.Category? category)
    {
        return GetProductItemForCatalog(BO.Filter.FilterByCategory, category);
    }


    /// <summary>
    /// Definition of a function that returns the list of product for catalog
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.ProductItem?> GetProductItemForCatalogNoFilter()
    {
       return GetProductItemForCatalog();
    }



    /// <summary>
    /// a help function that return a list of product by filter
    /// </summary>
    /// <param name="filter1"></param>
    /// <param name="filterValue"></param>
    /// <returns></returns>
    private IEnumerable<BO.ProductItem> GetProductItemForCatalog(Filter filter1 = BO.Filter.None, object? filterValue = null)
    {
        IEnumerable<DO.Product?> products;
        Filter filter = filter1;
        switch (filter)
        {
            case Filter.FilterByCategory:
                products = dal.Product.GetAll(product => product?.Category == (filterValue != null ? (DO.Category)filterValue : product?.Category));
                break;
            case Filter.None:
                products = dal.Product.GetAll();
                break;
            default:
                products = dal.Product.GetAll();
                break;
        }
        return from pro in products
               let product = (DO.Product)pro!
               select new ProductItem
               {
                   ID = product.ID,
                   Name = product.Name,
                   Price = product.Price,
                   Category = (BO.Category)product.Category,
                   Amount = 0,
                   Instock = product.InStock > 0 ? true : false
               };
    }



    /// <summary>
    /// function that returns a product by id for the customer
    /// </summary>
    /// <param name="id"></param>
    /// <returns>product</returns>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public BO.ProductItem GetProductByIdForCustomer(int id)
    {
        BO.ProductItem productItem = new BO.ProductItem();
        DO.Product product1 = new DO.Product();
        try
        {
            product1 = dal.Product.GetByCondition(product2 => product2?.ID == id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException("product does not exist", ex);
        }
        productItem.ID = product1.ID;
        productItem.Name = product1.Name;
        productItem.Price = product1.Price;
        productItem.Category = (BO.Category)product1.Category;
        productItem.Amount = 0;
        if (product1.InStock > 0)
            productItem.Instock = true;
        else
            productItem.Instock = false;
        return productItem;
    }



    /// <summary>
    /// Definition of a function that returns a list of product by category for the catalog
    /// </summary>
    /// <param name="category"></param>
    ///// <returns></returns>
    //public IEnumerable<BO.ProductItem> GetListOfProductItemsForCustomerByCategory(BO.Category? category)
    //{
    //    return GetListOfProductItemsForCustomer(BO.Filter.FilterByCategory, category);
    //}

    // /// <summary>
    //     /// Definition of a function that returns a list of product by category for the catalog
    //     /// </summary>
    //     /// <param name="category"></param>
    //     /// <returns></returns>
    //public IEnumerable<BO.ProductItem> GetListOfProductItemsForCustomerNoFiler()
    //{
    //    return GetListOfProductItemsForCustomer();
    //}

    /// <summary>
    /// Definition of a function that returns the list of product for catalog
    /// </summary>
    /// <returns></returns>
    //public IEnumerable<BO.ProductItem> GetProductItemForCatalogNoFilter()
    //{
    //    return GetProductItemForCatalog();
    //}



    /// <summary>
    /// a help function that return a list of product by filter
    /// </summary>
    /// <param name="filter1"></param>
    /// <param name="filterValue"></param>
    /// <returns></returns>
    //private IEnumerable<BO.ProductItem>GetListOfProductItemsForCustomer(Filter filter1 = BO.Filter.None, object? filterValue = null)
    //{
    //    IEnumerable<DO.Product?> products;
    //    Filter filter = filter1;
    //    switch (filter)
    //    {
    //        case Filter.FilterByCategory:
    //            products = dal.Product.GetAll(product => product?.Category == (filterValue != null ? (DO.Category)filterValue : product?.Category));
    //            break;
    //        case Filter.None:
    //            products = dal.Product.GetAll();
    //            break;
    //        default:
    //            products = dal.Product.GetAll();
    //            break;
    //    }
    //    return from pro in products
    //          let product = (DO.Product)pro!
    //           select new ProductItem
    //          {
    //              ID = product.ID,
    //              Name = product.Name,
    //              Price = product.Price,
    //              Category = (BO.Category)product.Category,
    //              Amount = product.InStock,
    //              Instock = product.InStock > 0 ? true : false
    //          };
    //}

}




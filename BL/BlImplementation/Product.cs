using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using BO;
using DO;
using static System.Net.Mime.MediaTypeNames;
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
        return GetProductListForManager(BO.Filter.Filter_By_Category, category);
    }

    ///// <summary>
    ///// a help function that return a list of Popular Product
    ///// </summary>
    ///// <param name="filter1"></param>
    ///// <param name="filterValue"></param>
    ///// <returns></returns>
    //public IEnumerable<BO.ProductItem> GetPopularProductList()
    //{
    //    IEnumerable<DO.OrderItem?> orderItems;

    //    try {orderItems = dal.OrderItem.GetAll();}
    //    catch (DalDoesNotExistException ex){throw new BO.BLDoesNotExistException("order item does not exist", ex);}

    //    var popularItems = from orderItem in orderItems
    //                           group orderItem by orderItem?.ProductID into orderItemGroup
    //                           select new { ID = orderItemGroup.Key, Items = orderItemGroup };
    //    popularItems = popularItems.OrderByDescending(p => p.Items.Count()).Take(amount);

    //    try {
    //        var popularProducts = from popularItem in popularItems
    //                              let DOProduct = dal.Product.GetByCondition(prod => prod?.ID == popularItem?.ID)
    //                              select new BO.ProductItem
    //                              {
    //                                  ID = DOProduct.ID,
    //                                  Name = DOProduct.Name,
    //                                  Category = (BO.Category)DOProduct.Category,
    //                                  Price = DOProduct.Price,
    //                                  Amount = 0,
    //                                  Instock = DOProduct.InStock > 0 ? true : false,
    //                                  Image=DOProduct.Image
    //                              };
    //        return popularProducts;
    //    }
    //    catch (DalDoesNotExistException ex)
    //    {
    //        throw new BO.BLDoesNotExistException("product does not exist", ex);
    //    }
    //}



    public IEnumerable<BO.ProductItem> GetPopularProductList()
    {
        IEnumerable<DO.OrderItem?> orderItems;
        IEnumerable<DO.Product?> productList;

        try { orderItems = dal.OrderItem.GetAll(); }
        catch (DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("order item does not exist", ex); }

         productList = dal.Product.GetAll();


        //var popularItems = from orderItem in orderItems
        //                   group orderItem by orderItem?.ProductID into orderItemGroup
        //                   select new { ID = orderItemGroup.Key, Items = orderItemGroup };
        //popularItems = popularItems.OrderByDescending(p => p.Items.Count()).Take(amount);

        var query = from orderItem in orderItems
                    group orderItem by orderItem?.ProductID into groupedOrderItems
                    join product in productList on groupedOrderItems.Key equals product?.ID
                    group new { Product = product, OrderItems = groupedOrderItems } by product?.Category into groupedProducts
                    select new
                    {
                        MostOrderedProduct = groupedProducts.OrderByDescending(gp => gp.OrderItems.Count()).Select(gp => gp.Product).FirstOrDefault()
                    };

        try
        {
            var popularProducts = from popularProduct in query
                                  let DOProduct = dal.Product.GetByCondition(prod => prod?.ID ==(popularProduct?.MostOrderedProduct)?.ID)
                                  select new BO.ProductItem
                                  {
                                      ID = DOProduct.ID,
                                      Name = DOProduct.Name,
                                      Category = (BO.Category)DOProduct.Category,
                                      Price = DOProduct.Price,
                                      Amount = 0,
                                      Instock = DOProduct.InStock > 0 ? true : false,
                                      Image = DOProduct.Image
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
            case Filter.Filter_By_Category:
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
        DO.Product product;

            try {  product = dal.Product.GetByCondition(product2 => product2?.ID == id); }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("product does not exist", ex); }

            BO.Product product1 = new BO.Product();
            product1.ID = product.ID;
            product1.Name = product.Name;
            product1.Price = product.Price;
            product1.Category = (BO.Category)product.Category;
            product1.InStock = product.InStock;
            product1.Image = product.Image;
            return product1;
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
            product1.Image = copyFiles(product.Image!, product.ID.ToString());

            try { dal.Product.Add(product1); }
            catch (DO.DalAlreadyExistException ex) { throw new BO.BLAlreadyExistException("product already exist", ex); }

            return product;
    }


    /// <summary>
    /// function that delete a product
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BLImpossibleActionException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public void DeleteProduct(int id)
    {
        IEnumerable<DO.OrderItem?> orderItems;
        DO.Product prod;
        try {  orderItems = dal.OrderItem.GetAll(); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("prouct does not exist", ex); }

        DO.OrderItem? orderItem = orderItems.FirstOrDefault(item => ((DO.OrderItem)item!).ProductID == id);

         if (orderItem != null)
               throw new BO.BLImpossibleActionException($"product {id} exist in order {orderItem?.OrderID}");

        try { prod = dal.Product.GetByCondition(product2 => product2?.ID == id); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("prouct does not exist", ex); }

        try { dal.Product.Delete(id); }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("prouct does not exist", ex); }

        deleteFiles(prod.Image!); 
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
        DO.Product previousProd;
            if (product.ID < 1 || product.Name == "" || product.Price < 1 || product.InStock < 0 || (int)product.Category > 5 || (int)product.Category < 0 || product.Image==null)
                throw new BO.BLInvalidInputException(" Invalid input");
            {

                try {previousProd = dal.Product.GetByCondition(product2 => product2?.ID == product.ID); }
                catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("prouct does not exist", ex); }

                string previousImg = previousProd.Image!;
                DO.Product product1 = new DO.Product();
                product1.ID = product.ID;
                product1.Name = product.Name;
                product1.Price = product.Price;
                product1.InStock = product.InStock;
                product1.Category = (DO.Category)product.Category;
                if (previousImg != product.Image)
                {
                    product.Image = copyFiles(product.Image, product.ID.ToString());
                }
                product1.Image = product.Image;

            try { dal.Product.Update(product1); }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BLDoesNotExistException("prouct does not exist", ex); }

        }
        return product;
    }


    /// <summary>
    /// function that update a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>copyFiles</returns>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    private string copyFiles(string sourcePath, string destinationName)
    {
        try
        {
            int postfixIndex = sourcePath.LastIndexOf('.');
            string postfix = sourcePath.Substring(postfixIndex);
            destinationName += postfix;
            string destinationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            destinationPath = destinationPath!.Substring(0, destinationPath.Length - 3);
            string destinationFullName = @"\img\" + destinationName;
            System.IO.File.Copy(sourcePath, destinationPath + "\\" + destinationFullName, true);
            string destinationFullNameDal = @"..\\img\" + destinationName;
            return destinationFullNameDal;
        }
        catch (Exception ex)
        {
            return @"..\img\empty_image.jpg";
        }
    }


    /// <summary>
    /// function that update a product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>deleteFiles</returns>
    /// <exception cref="BO.BLInvalidInputException"></exception>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    private void deleteFiles(string destinationFullNameDal)
    {
        try
        {
            System.IO.File.Delete(destinationFullNameDal);
        }
        catch (Exception ex)
        {
            throw  new BO.BLImpossibleActionException("img not exist");
        }
    }

    /// <summary>
    /// Definition of a function that returns a list of product by category for the catalog
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public IEnumerable<BO.ProductItem> GetProducItemForCatalogByCategory(BO.Category? category)
    {
        return GetProductItemForCatalog(BO.Filter.Filter_By_Category, category);
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
            case Filter.Filter_By_Category:
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
                   Instock = product.InStock > 0 ? true : false,
                   Image= product.Image
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

        try { product1 = dal.Product.GetByCondition(product2 => product2?.ID == id);}
        catch (DO.DalDoesNotExistException ex) {throw new BO.BLDoesNotExistException("product does not exist", ex);}

        productItem.ID = product1.ID;
        productItem.Name = product1.Name;
        productItem.Price = product1.Price;
        productItem.Category = (BO.Category)product1.Category;
        productItem.Amount = 0;
        productItem.Image = product1.Image;
        if (product1.InStock > 0)
            productItem.Instock = true;
        else
            productItem.Instock = false;
        return productItem;
    }
}




using DO;
using static Dal.DataSource;
using DalApi;
namespace Dal;

/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
internal class ProductDal:IProduct
{
    /// <summary>
    /// add a product to the array
    /// </summary>
    /// <param name="product">the new product item </param>
    /// <returns>the id of the new product</returns>
    /// <exception cref="Exception">if the product does not exist</exception>
    public int Add(Product product)
    {
        foreach (Product p in ProductList)
        {
            if (p.ID == product.ID)
                throw new DalAlreadyExistException(product.ID,"product");
        }
        ProductList.Add(product);
        return product.ID;
    }


    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Delete(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            ProductList.RemoveAt(index);
        }
        else
            throw new DalDoesNotExistException(id,"product");
    }


    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void Update(Product product)
    {
        int index = search(product.ID);
        if (index != -1)
            ProductList[index] = product;
        else
            throw new DalDoesNotExistException(product.ID,"product");
    }


    /// <summary>
    /// get product by id
    /// </summary>
    /// <param name="id">the id of the requeses product</param>
    /// <returns>the product</returns>
    /// <exception cref="Exception">if the product didnt exist throw exeption</exception>
    //***********************************************************************************************
    //public Product GetById(int id)
    //{
    //    int index = search(id);
    //    if (index != -1)
    //        return (Product)ProductList[index];
    //    else
    //        throw new DalDoesNotExistException(id, "product");
    //}
    //********************************************************************************************


    /// <summary>
    /// get all products by codition
    /// </summary>
    /// <returns>an array of all the products</returns>
    public IEnumerable<Product> GetAll(Func<Product, bool>? predicate=null)
    {
        List<Product> products = new List<Product>();
        if (predicate != null)
        {
            foreach (Product product in ProductList)
            {
                if (predicate(product))
                {
                    products.Add(product);
                }
            }
        }
        else
        {
            foreach (Product product in ProductList)
            {
                products.Add(product);
            }
        }
        return products;
    }


    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>
    private int search(int id)
    {
        for (int i = 0; i < ProductList.Count; i++)
        {
            if (ProductList[i]?.ID == id)
                return i;
        }
        return -1;
    }


    /// <summary>
    /// get product by codition
    /// </summary>
    /// <returns>a product</returns>
    public Product GetByCondition(Func<Product, bool>? predicate)
    {
        foreach (Product product in ProductList)
        {
            if (predicate(product))
                return product;
        }
        throw new DalDoesNotExistException("There is no product that meets the condition");
    }
}

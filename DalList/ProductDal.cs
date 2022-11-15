using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;
/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>
public class ProductDal
{
    public int AddProduct(Product product)
    {
        int i;
        for (i = 0; i < indexProduct && ProductArray[i].ID != product.ID; i++) ;
        if (i < indexProduct)
            throw new Exception("product id already exists");
        ProductArray[indexProduct++] = product;
        return product.ID;

    }
    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void DeleteProduct(int id)
    {
        int index = search(id);
        if (index != -1)
        {
            for (int i =index; i <=indexProduct; i++)
            {
                ProductArray[i] = ProductArray[i + 1];
            }
            indexProduct--;
        }
        else
            throw new Exception(" product is not exist");

    }
    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    public void UpdateProduct(Product product)
    {
        int index = search(product.ID);
        if (index != -1)
            ProductArray[index] = product;
        else
            throw new Exception(" product is not exist");

    }
    /// <summary>
    /// get product by id
    /// </summary>
    /// <param name="id">the id of the requeses product</param>
    /// <returns>the product</returns>
    /// <exception cref="Exception">if the product didnt exist throw exeption</exception>

    public Product GetProduct(int id)
    {
        int index = search(id);
        if (index != -1)
            return ProductArray[index];
        else
            throw new Exception(" product is not exist");
    }
    /// <summary>
    /// get all products
    /// </summary>
    /// <returns>an array of all the products</returns>
    public Product[] GetAllProducts()
    {
        Product[] products = new Product[indexProduct];
        for (int i = 0; i < indexProduct; i++)
        {
            products[i] = ProductArray[i];
        }
        return products;
    }
    /// <summary>
    ///search function
    /// </summary>
    /// <returns>returns the index of the member found</returns>
    private int search(int id)
    {
        for (int i = 0; i < indexProduct; i++)
        {
            if (ProductArray[i].ID == id)
                return i;
        }
        return -1;
    }
}

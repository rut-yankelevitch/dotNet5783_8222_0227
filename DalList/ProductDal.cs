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
    public void Update(Product product)
    {
        int count = ProductList.RemoveAll(prod => prod?.ID == product.ID);
        if (count == 0)
                throw new DalDoesNotExistException(product.ID,"product");
            ProductList.Add(product);
    }


    /// <summary>
    /// get all products
    /// </summary>
    /// <returns>an array of all the products</returns>
     
    public IEnumerable<Product?> GetAll(Func<Product?, bool>? predicate = null) =>
    (predicate == null ? ProductList.Select(item => item) : ProductList.Where(predicate!)) ??
        throw new DO.DalDoesNotExistException("product missing");



    /// <summary>
    /// get product by predicate
    /// </summary>
    /// <param name="predicate">the order id</param>
    /// <returns>the product</returns>
    /// <exception cref="Exception">if the product doesnt exist</exception>
    
    public Product GetByCondition(Func<Product?, bool> predicate)=>
        ProductList.FirstOrDefault(predicate)??
        throw new DalDoesNotExistException("There is no order item that meets the condition");



    /// <summary>
    /// the function check if the product exist
    /// </summary>
    private bool CheckIfExist(int id)
    {
        return ProductList.Any(item => item?.ID == id);
    }
}

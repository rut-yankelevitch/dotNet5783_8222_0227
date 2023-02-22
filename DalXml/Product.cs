using System.Runtime.CompilerServices;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal;
/// <summary>
/// A department that performs operations: 
/// adding, updating, repeating and deleting on the product array
/// </summary>

internal class Product : IProduct
{
    const string s_product = @"Product";

    /// <summary>
    /// return DO.Product entity
    /// </summary>
    static DO.Product? createProductfromXElement(XElement p)
    {
        return new DO.Product()
        {
            ID = (int)p.Element("ID")!,
            Category = p.ToEnum<DO.Category>("Category"),
            Name = (string?)p.Element("Name"),
            Image = (string?)p.Element("Image"),
            Price = (double)p.Element("Price")!,
            InStock =(int)p.Element("InStock")!,
        };
    }

    /// <summary>
    /// get all products
    /// </summary>
    /// <returns>an array of all the products</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<DO.Product?> GetAll(Func<DO.Product?, bool>? predicate)
    {
        XElement? productRootElement = XMLTools.LoadListFromXMLElement(s_product);


        if (predicate != null)
        {
            return from p in productRootElement.Elements()
                   let prod = createProductfromXElement(p)
                   where predicate(prod)
                   select prod;
        }
        else
        {
            return from s in productRootElement.Elements()
                   let prod= createProductfromXElement(s)
                   select prod;
        }
    }


    /// <summary>
    /// get product by predicate
    /// </summary>
    /// <param name="predicate">the order id</param>
    /// <returns>the product</returns>
    /// <exception cref="Exception">if the product doesnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public DO.Product GetByCondition(Func<DO.Product?, bool> predicate)
    {
        XElement productRootElement = XMLTools.LoadListFromXMLElement(s_product);

        return (from p in productRootElement?.Elements()
                let prod = createProductfromXElement(p)
                where predicate(prod)
                select prod).FirstOrDefault()
                ?? throw new DO.DalDoesNotExistException("The requested product was not found");
    }


    /// <summary>
    /// add a order to the order array
    /// </summary>
    /// <param name="product">the new product</param>
    /// <returns>the insert new product id</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(DO.Product doProduct)
    {
        XElement productRootElement = XMLTools.LoadListFromXMLElement(s_product);

        XElement? product = (from prod in productRootElement.Elements()
                          where (int)prod.Element("ID")! == doProduct.ID 
                          select prod).FirstOrDefault();
        if (product != null)
            throw new DalAlreadyExistException(doProduct.ID, "product"); 

        XElement productElement = new XElement("Product",
                                   new XElement("ID", doProduct.ID),
                                   new XElement("Name", doProduct.Name),
                                   new XElement("Category", doProduct.Category),
                                   new XElement("Price", doProduct.Price),
                                   new XElement("InStock", doProduct.InStock),
                                   new XElement("Image", doProduct.Image)
                                   );

        productRootElement.Add(productElement);

        XMLTools.SaveListToXMLElement(productRootElement, s_product);

        return doProduct.ID; ;
    }


    /// <summary>
    /// delete a product 
    /// </summary>
    /// <param name="id">the id of the product thet need to be deleted</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        XElement prroductRootElement = XMLTools.LoadListFromXMLElement(s_product);

        XElement? prod = (from p in prroductRootElement.Elements()
                          where (int)p.Element("ID")! == id
                          select p).FirstOrDefault() ?? throw new DalDoesNotExistException(id, "product"); 

        prod.Remove(); //<==>   Remove product from studentsRootElem

        XMLTools.SaveListToXMLElement(prroductRootElement, s_product);
    }

    /// <summary>
    /// update a product
    /// </summary>
    /// <param name="product">the product new details</param>
    /// <exception cref="Exception">if the product didnt exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(DO.Product doProd)
    {
        Delete(doProd.ID);
        Add(doProd);
    }
}

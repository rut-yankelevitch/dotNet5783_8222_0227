using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal;
internal class Product : IProduct
{
    const string s_product = "product"; //Linq to XML

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


    public IEnumerable<DO.Product?> GetAll(Func<DO.Product?, bool>? filter = null)
    {
        XElement? ProductRootElement = XMLTools.LoadListFromXMLElement(s_product);


        if (filter != null)
        {
            return from p in ProductRootElement.Elements()
                   let prod = createProductfromXElement(p)
                   where filter(prod)
                   select prod;
        }
        else
        {
            return from s in ProductRootElement.Elements()
                   select createProductfromXElement(s);
        }
    }


    public DO.Product GetByCondition(Func<DO.Product?, bool> predicate)
    {
        XElement productRootElement = XMLTools.LoadListFromXMLElement(s_product);

        return (from p in productRootElement?.Elements()
                let prod = createProductfromXElement(p)
                where predicate(prod)
                select prod).FirstOrDefault()
                ?? throw new DO.DalDoesNotExistException("The requested product was not found");
    }


    public int Add(DO.Product doProduct)
    {
        XElement productRootElement = XMLTools.LoadListFromXMLElement(s_product);

        XElement? product = (from prod in productRootElement.Elements()
                          where (int)prod.Element("ID")! == doProduct.ID //where (int?)st.Element("ID") == doStudent.ID
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


    public void Delete(int id)
    {
        XElement prroductRootElement = XMLTools.LoadListFromXMLElement(s_product);

        XElement? prod = (from p in prroductRootElement.Elements()
                          where (int)p.Element("ID")! == id
                          select p).FirstOrDefault() ?? throw new DalDoesNotExistException(id, "product"); 

        prod.Remove(); //<==>   Remove product from studentsRootElem

        XMLTools.SaveListToXMLElement(prroductRootElement, s_product);
    }


    public void Update(DO.Product doProd)
    {
        Delete(doProd.ID);
        Add(doProd);
    }
}

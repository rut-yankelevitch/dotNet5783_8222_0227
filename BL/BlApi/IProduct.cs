using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IProduct
    {
    //public int Add(Product product );
	//public void Update(Product product );
	//public void Delete(int id);
	//public Product GetById(int id);
    //public IEnumerable <Product> GetAll() ;
    public IEnumerable<ProductForList> GetProductListForManager();
    public Product GetProductByIdForManager(int id);
    public Product AddProduct(Product product);
    public void DeleteProduct(int id);
    public Product UpdateProduct(Product product);
    public IEnumerable<ProductItem> GetProductListForCustomer();
    public ProductItem GetProductByIdForCustomer(int id);
    }
}

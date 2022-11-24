using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IProduct
    {
    public int Add(Product product );
	public void Update(Product product );
	public void Delete(int id);
	public Product GetById(int id);
    public IEnumerable <Product> GetAll() ;
    }
}

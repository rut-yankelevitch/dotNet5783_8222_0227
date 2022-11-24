using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface ICart
    {
    public int Add(Cart cart);
	public void Update(Cart cart );
	public void Delete(int id);
	public Cart  GetById(int id);
    public IEnumerable <Cart> GetAll() ;
    }
}

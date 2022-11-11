using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;
{
    public class ProductDal
    {
        public int AddProduct(Product p)
        {
            p.ID = Config.IDProduct;
            ProductArray[Config.indexProduct] = p;
            return ProductArray[Config.indexProduct++].ID;
        }
        public void deleteProduct(int id)
        {
            int x = search(id);
        if (x != -1)
        {
            for (int i = x + 1; i < ProductArray.lengthe; i++)
            {
                ProductArray[x - 1] = ProductArray[x];
            }
        }
        else
            throw new Exception(" product is not exist");

        }
        public void UpdateProduct( Product p)
        {
            int x = search(p.ID);
            if (x != -1)
             ProductArray[x] = p;
            else
                throw new Exception(" product is not exist");

        }
        public Product GetProduct(int id)
        {
            int x = search(id);
            if (x != -1)
                return ProductArray[x];
            else
                throw new Exception(" product is not exist");
        }
        public Product[]  GetAllProducts()
        {
            Product[] p = new Product[ProductArray.Length];
            for (int i = 0; i < ProductArray.Length; i++)
            {
                p[i] = ProductArray[i];
            }
            return p;
        }

        private int search(int id)
        {
            for (int i = 0; i < ProductArray.Length; i++)
            {
                if(ProductArray[i].ID= id )
                    return i;
            }
            return -1;  
        }
    }
}

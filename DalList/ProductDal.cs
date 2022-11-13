using DO;
using System.Drawing;
using static Dal.DataSource;

namespace Dal;

    public class ProductDal
    {
        public int AddProduct(Product product)
        {
        int i;
        for (i = 0; i < Config.indexProduct && ProductArray[i].ID != product.ID; i++) ;
        if (i < Config.indexProduct)
            throw new Exception("product id already exists");
        ProductArray[Config.indexProduct++] = product;
        return product.ID;

    }
    public void DeleteProduct(int id)
        {
            int x = search(id);
        if (x != -1)
        {
            for (int i = x; i <= Config.indexProduct; i++)
            {
                ProductArray[i] = ProductArray[i+1];
            }
            Config.indexProduct--;
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
            Product[] products = new Product[Config.indexProduct];
            for (int i = 0; i < Config.indexProduct; i++)
            {
            products[i] = ProductArray[i];
            }
            return products;
        }

        private int search(int id)
        {
            for (int i = 0; i < Config.indexProduct; i++)
            {
                if(ProductArray[i].ID== id )
                    return i;
            }
            return -1;  
        }
    }


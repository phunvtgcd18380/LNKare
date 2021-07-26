using LNKareAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNKareAPI.Repository.IRepository
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int productId);
        bool ProductExists(string productName);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();
        bool ProductExists(int productId);

    }
}

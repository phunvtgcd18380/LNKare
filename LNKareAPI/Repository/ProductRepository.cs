using LNKareAPI.Data;
using LNKareAPI.Models;
using LNKareAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNKareAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateProduct(Product product)
        {
            var categoryFromDb = _db.Categories.FirstOrDefault(i => i.Id == product.CategoryId);
            if(categoryFromDb != null)
            {
                Product product1 = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = categoryFromDb.Id,
                    Category = categoryFromDb
                };
                _db.Products.Add(product1);
                return Save();
            }
            return false;
        }

        public bool DeleteProduct(Product product)
        {
            _db.Products.Remove(product);
            return Save();
        }

        public Product GetProduct(int productId)
        {
            return _db.Products.Include(i => i.Category).FirstOrDefault(i => i.Id == productId);
        }


        public ICollection<Product> GetProducts()
        {
            return _db.Products.Include(i => i.Category).ToList();
        }

        public bool ProductExists(int productId)
        {
            return _db.Products.Any(i => i.Id == productId);
        }
        public bool ProductExists(string productName)
        {
            return _db.Products.Any(i => i.Name == productName);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false ;
        }

        public bool UpdateProduct(Product product)
        {
            _db.Products.Update(product);
            return Save();
        }
    }
}

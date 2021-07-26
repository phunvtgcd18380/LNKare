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
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CategoryExsits(int CategoryId)
        {
            return _db.Categories.Any(i => i.Id == CategoryId);
        }

        public bool CreateCategory(Category category)
        {
            _db.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Categories.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.ToList();
        }
        public ICollection<Product> GetProductsInCategory(int CategoryId)
        {
            return _db.Products.Include(i => i.Category).Where(w => w.CategoryId == CategoryId).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _db.Categories.FirstOrDefault(i => i.Id == categoryId);
        }
        public bool CategoryExsits(string categoryName)
        {
            return _db.Categories.Any(i => i.Name == categoryName);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _db.Categories.Update(category);
            return Save();
        }
    }
}

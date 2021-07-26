using LNKareAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNKareAPI.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool CategoryExsits(string categoryName);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
        bool CategoryExsits(int CategoryId);
        ICollection<Product> GetProductsInCategory(int CategoryId);
    }
}

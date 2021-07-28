using LNKareAPI.Data;
using LNKareAPI.Models;
using LNKareAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNKareAPI.Repository
{
    public class ImageProductRepository : IImageProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ImageProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateImage(ImageProduct Image)
        {
            var ProductFromDb = _db.Products.FirstOrDefault(i => i.Id == Image.ProductId);
            if (ProductFromDb != null)
            {
                ImageProduct image1 = new ImageProduct()
                {
                    ImageUrl = Image.ImageUrl,
                    ProductId = ProductFromDb.Id,
                    Product = ProductFromDb
                };
                _db.ImageProducts.Add(image1);
                return Save();
            }
            return false;
        }

        public bool DeleteImage(ImageProduct Image)
        {
            _db.ImageProducts.Remove(Image);
            return Save();
        }

        public ImageProduct GetImage(int imageId)
        {
            return _db.ImageProducts.FirstOrDefault(i => i.Id == imageId);
        }

        public ICollection<ImageProduct> GetImagesInProduct(int ProductId)
        {
           return _db.ImageProducts.Where(i => i.Product.Id == ProductId).ToList();
        }


        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}

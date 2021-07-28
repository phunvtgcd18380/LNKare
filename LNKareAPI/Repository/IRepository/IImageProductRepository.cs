using LNKareAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNKareAPI.Repository.IRepository
{
    public interface IImageProductRepository
    {
        bool CreateImage(ImageProduct Image);
        bool DeleteImage(ImageProduct Image);
        bool Save();
        ICollection<ImageProduct> GetImagesInProduct(int ProductId);
        ImageProduct GetImage(int imageId);
    }
}

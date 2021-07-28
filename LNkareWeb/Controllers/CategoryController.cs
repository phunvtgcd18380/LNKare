using LNkareWeb.Models;
using LNkareWeb.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LNkareWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IImageProductRepository _imageRepo;
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly IProductRepository _productRepo;
        public CategoryController(ICategoryRepository categoryRepo,IImageProductRepository imageRepo, IProductRepository productRepo, IWebHostEnvironment hostEnvironment)
        {
            _categoryRepo = categoryRepo;
            _imageRepo = imageRepo;
            _hostEnviroment = hostEnvironment;
            _productRepo = productRepo;
        }
        public IActionResult Index()
        {
            return View(new Category());
        }
        public async Task<IActionResult> GetAllCategory()
        {
            return Json(new { data = await _categoryRepo.GetAllAsync(SD.CategoryAPIPath) });
        }
        public IActionResult Upsert()
        {
            Category category = new Category();
                return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                    await _categoryRepo.CreateAsync(SD.CategoryAPIPath, category);               
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            string webRoot = _hostEnviroment.WebRootPath;
            var productInCategory = await _categoryRepo.GetAllInSomeTableAsync(SD.CategoryAPIPath,id);
            foreach (var product in productInCategory)
            {
                var imgFromdb = await _imageRepo.GetAllInSomeTableAsync(SD.ImageProductAPIPath, product.Id);
                List<string> imgFromDb = new List<string>();
                foreach (var item in imgFromdb)
                {
                    if(item == null)
                    {
                        break;
                    }
                    imgFromDb.Add(item.ImageUrl);
                    await _imageRepo.DeleteAsync(SD.ImageProductAPIPath, item.Id);
                }
                foreach (var item in imgFromDb)
                {
                    if (item != null)
                    {
                        System.IO.File.Delete(webRoot + item);
                    }
                }
                System.IO.Directory.Delete(Path.Combine(webRoot, @"images\products\", product.Name));
            }
            var status = await _categoryRepo.DeleteAsync(SD.CategoryAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete SuccessFul" });
            }
            return Json(new { success = false, message = "Delete Unsuccessful" });
        }
    }
}

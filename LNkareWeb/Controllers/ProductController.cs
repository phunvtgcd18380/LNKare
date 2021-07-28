using LNkareWeb.Data;
using LNkareWeb.Models;
using LNkareWeb.Models.ViewModel;
using LNkareWeb.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LNkareWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IImageProductRepository _imageRepo;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnviroment;
        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, ApplicationDbContext db, IWebHostEnvironment hostEnvironment, IImageProductRepository imageRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _db = db;
            _hostEnviroment = hostEnvironment;
            _imageRepo = imageRepo;
        }
        public IActionResult Index()
        {
            return View(new Product());
        }
        public async Task<IActionResult> GetAllProduct()
        {
            return Json(new { data = await _productRepo.GetAllAsync(SD.ProductAPIPath) });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            var objCategory = await _categoryRepo.GetAllAsync(SD.CategoryAPIPath);
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                ListCategory = objCategory.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is create
                return View(productVM);
            }
            productVM.Product = await _productRepo.GetAsync(SD.ProductAPIPath, id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }
            else
            {
                return View(productVM);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRoot = _hostEnviroment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var objFromdb = await _imageRepo.GetAllInSomeTableAsync(SD.ImageProductAPIPath, productVM.Product.Id);
                    var folderName = productVM.Product.Name + @"\";
                    System.IO.Directory.CreateDirectory(Path.Combine(webRoot , @"images\products\" , folderName));
                    List<string> imgFromDb = new List<string>();
                    foreach(var item in objFromdb)
                    {
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
                    //this for create or edit when update image
                    for (int i = 0; i <= files.Count - 1; i++)
                    {
                        string fileName = Path.GetFileName(files[i].FileName); 
                        var upload = Path.Combine(webRoot, @"images\products\",folderName);

                       
                        using (var filesStream = new FileStream(Path.Combine(upload, fileName), FileMode.Create))
                        {
                            files[i].CopyTo(filesStream);
                        }
                        ImageProduct image = new ImageProduct()
                        {
                            ImageUrl = @"\images\products\"+ folderName  + fileName,
                            Product = productVM.Product,
                            ProductId = productVM.Product.Id
                        };
                        await _imageRepo.CreateAsync(SD.ImageProductAPIPath, image);
                    }
                }
                if (productVM.Product.Id == 0)
                {
                    var obj = await _productRepo.CreateAsync(SD.ProductAPIPath, productVM.Product);
                    return RedirectToAction(nameof(Upsert),new { id = obj.Id});
                }
                else
                {
                    await _productRepo.UpdateAsync(SD.ProductAPIPath + productVM.Product.Id, productVM.Product);
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                var objCategory = await _categoryRepo.GetAllAsync(SD.CategoryAPIPath);
                productVM.ListCategory = objCategory.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if(productVM.Product.Id != 0)
                {
                    productVM.Product = await _productRepo.GetAsync(SD.ProductAPIPath, productVM.Product.Id);
                }
                return View(productVM);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            string webRoot = _hostEnviroment.WebRootPath;
            var productFromdb = await _productRepo.GetAsync(SD.ProductAPIPath, id);
            var imgFromdb = await _imageRepo.GetAllInSomeTableAsync(SD.ImageProductAPIPath, id);
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

            var status = await _productRepo.DeleteAsync(SD.ProductAPIPath, id);
            if (status)
            {
                System.IO.Directory.Delete(Path.Combine(webRoot, @"images\products\", productFromdb.Name));
                return Json(new { success = true, message = "Delete SuccessFul" });
            }
            return Json(new { success = false, message = "Delete Unsuccessful" });
        }
    }
}

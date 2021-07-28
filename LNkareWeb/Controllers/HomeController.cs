using LNkareWeb.Models;
using LNkareWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LNkareWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly IImageProductRepository _imgRepo;
        public HomeController(ILogger<HomeController> logger,IProductRepository productRepo,IImageProductRepository imgRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _imgRepo = imgRepo;
        }

        public async Task<IActionResult> Index()
        {
            var objproduct = await _productRepo.GetAllAsync(SD.ProductAPIPath);

            foreach(var item in objproduct)
            {
                item.Imgage = new List<string>();
                var imgpro = await _imgRepo.GetAllInSomeTableAsync(SD.ImageProductAPIPath, item.Id);
                    foreach (var img in imgpro)
                    {
                        item.Imgage.Add(img.ImageUrl);
                    }
            }
            return View(objproduct);
        }
        public async Task<IActionResult> Details(int id)
        {
            var objproduct = await _productRepo.GetAsync(SD.ProductAPIPath,id);

                objproduct.Imgage = new List<string>();
                var imgpro = await _imgRepo.GetAllInSomeTableAsync(SD.ImageProductAPIPath, id);
                foreach (var img in imgpro)
                {
                    objproduct.Imgage.Add(img.ImageUrl);
                }
            return View(objproduct);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

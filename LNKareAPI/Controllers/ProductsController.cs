using LNKareAPI.Models;
using LNKareAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNKareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        public ProductsController(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            var allObj = _productRepo.GetProducts();
            return Ok(allObj);
        }
        [HttpGet("{productId:int}",Name ="GetProduct")]
        public IActionResult GetProduct(int productId)
        {
            var objFromDb = _productRepo.GetProduct(productId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            return Ok(objFromDb);
        }
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if(product == null)
            {
                return BadRequest(ModelState);
            }
            if (_productRepo.ProductExists(product.Name))
            {
                ModelState.AddModelError("", "Product Exsits");
                return StatusCode(404, ModelState);
            }
            if (!_productRepo.CreateProduct(product))
            {
                ModelState.AddModelError("", "Something Wrong When Create Product");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }
        [HttpPatch("{productId:int}",Name ="UpdateProduct")]
        public IActionResult UpdateProduct(int productId, Product product)
        {
            if(product == null)
            {
                return BadRequest(ModelState);
            }
            if (!_productRepo.ProductExists(productId))
            {
                return NotFound();
            }
            if (!_productRepo.UpdateProduct(product))
            {
                ModelState.AddModelError("", "Something Wrong When Update");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{productId:int}",Name ="DeleteProduct")]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_productRepo.ProductExists(productId))
            {
                return NotFound();
            }
            var objFromDb = _productRepo.GetProduct(productId);
            if (!_productRepo.DeleteProduct(objFromDb))
            {
                ModelState.AddModelError("", "Something Wrong When Delete");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}

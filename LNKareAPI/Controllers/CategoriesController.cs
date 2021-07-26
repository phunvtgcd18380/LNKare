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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoriesController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var allObj = _categoryRepo.GetCategories();
            return Ok(allObj);
        }
        [HttpGet("{categoryId:int}", Name = "GetProductsInCategory")]
        public IActionResult GetProductsInCategory(int categoryId)
        {
            var allObjInCategory = _categoryRepo.GetProductsInCategory(categoryId);
            return Ok(allObjInCategory);
        }
        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepo.CategoryExsits(category.Name))
            {
                ModelState.AddModelError("", "Category Exsits");
                return StatusCode(404, ModelState);
            }
            if (!_categoryRepo.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Something Wrong when Create");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok();
            }
        }
        [HttpPatch("{categoryId:int}", Name ="UpdateCategory")]
        public IActionResult UpdateCategory(int categoryId,[FromBody] Category category)
        {
            if(category == null)
            {
                return BadRequest(ModelState);
            }
            if (!_categoryRepo.CategoryExsits(category.Id))
            {
                return NotFound();
            }
            else
            {
                if (!_categoryRepo.UpdateCategory(category))
                {
                    ModelState.AddModelError("", $"Something Wrong When Update");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }
        }
        [HttpDelete("{categoryId:int}",Name ="DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepo.CategoryExsits(categoryId))
            {
                return NotFound();
            }
            var objFromDb = _categoryRepo.GetCategory(categoryId);
            if (!_categoryRepo.DeleteCategory(objFromDb))
            {
                ModelState.AddModelError("", "Something Wrong when Delete");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}

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
    public class ImageProductsController : ControllerBase
    {
        private readonly IImageProductRepository _imgRepo;
        public ImageProductsController(IImageProductRepository imgRepo,IProductRepository productRepo)
        {
            _imgRepo = imgRepo;
        }
        [HttpPost]
        public IActionResult CreateImage([FromBody] ImageProduct image)
        {
            if (image == null)
            {
                return BadRequest(ModelState);
            }
            if (!_imgRepo.CreateImage(image))
            {
                ModelState.AddModelError("", $"Something Wrong when Create");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok();
            }
        }
        [HttpDelete("{imageId:int}", Name = "DeleteImage")]
        public IActionResult DeleteImage(int imageId)
        {
            var obj = _imgRepo.GetImage(imageId);
                if (!_imgRepo.DeleteImage(obj))
                {
                    ModelState.AddModelError("", "Something Wrong when Delete");
                    return StatusCode(500, ModelState);
                }
            return NoContent();
        }
        [HttpGet("{ProductId:int}", Name = "GetImageInProduct")]
        public IActionResult GetImageInProduct(int ProductId)
        {
            var allObjInCategory = _imgRepo.GetImagesInProduct(ProductId);
            return Ok(allObjInCategory);
        }
    }
}

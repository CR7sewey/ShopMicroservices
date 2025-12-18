using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.ProductAPI.Models;
using Shop.ProductAPI.Models.DTOs;
using Shop.ProductAPI.Services;

namespace Shop.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IService<CategoryDTO, Category> service;

        public CategoryController(IService<CategoryDTO, Category> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCats()
        {
            var categoriesDTO = await service.GetAll(null);
            return Ok(categoriesDTO);
        }

    }
}

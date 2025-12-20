using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Shop.ProductAPI.Models;
using Shop.ProductAPI.Models.DTOs;
using Shop.ProductAPI.Services;

namespace Shop.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IService<ProductDTO, Product> service;

        public ProductController(IService<ProductDTO, Product> service)
        {
            this.service = service;
        }

        [HttpGet]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var products = await service.GetAll(null);
            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get(Guid id)
        {
            var prod = await service.GetById(id);         
            return prod is ProductDTO product ? Ok(product) : BadRequest("Product does not exist: "+id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Create(ProductDTO productDTO)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest("Product not valid");
            }*/

            if (productDTO == null)
            {
                return BadRequest("Product not valid");
            }

            var prod = await service.Create(productDTO);
            return CreatedAtAction("Get", new { id = prod.Id }, prod);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(Guid id, ProductDTO productDTO)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest("Product not valid");
            }*/

            if (productDTO == null)
            {
                return BadRequest("Product not valid");
            }

            //var prodExist = await service.GetById(id);
            if (productDTO.Id != id) {
                return BadRequest("Ids do not match");
            }

            await service.Update(productDTO);
            return NoContent();


        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var product = await service.GetById(id);
            if (product is null)
            {
                return NotFound("Product does not exist");
            }
            await service.Delete(id);
            return NoContent();


        }
    }
}

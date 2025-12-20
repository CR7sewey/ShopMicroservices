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

        private readonly ICategoryService service;

        public CategoryController(ICategoryService service)
        {
            this.service = service;
        }


        /// <summary>
        /// Handles HTTP GET requests to retrieve all cat categories.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing a collection of cat category data if successful. Returns status
        /// code 200 (OK) with the data.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllCats()
        {
            var categoriesDTO = await service.GetAll(null);
            return Ok(categoriesDTO);
        }

        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetAllCatsProducts()
        {
            var categoriesDTO = await service.GetCategoriesProducts();
            return Ok(categoriesDTO);
        }

        /// <summary>
        /// Retrieves the category with the specified unique identifier.
        /// </summary>
        /// <remarks>Returns a 200 OK response if the category exists. If the category does not exist,
        /// returns a 404 Not Found response with a descriptive message. This method is intended for use in HTTP GET
        /// requests to retrieve category details by ID.</remarks>
        /// <param name="id">The unique identifier of the category to retrieve.</param>
        /// <returns>An <see cref="ActionResult"/> containing the category data if found; otherwise, a 404 Not Found response
        /// with an error message.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Get(Guid id)
        {
            var categoryDTO = await service.GetById(id);
            if (categoryDTO is null)
            {
                return NotFound("Category does not exist: " + id);
            }
            return Ok(categoryDTO);

        }

        /// <summary>
        /// Creates a new category using the provided data transfer object.
        /// </summary>
        /// <remarks>If the category is successfully created, the response includes a location header
        /// referencing the newly created resource. The method returns a bad request if the input is null.</remarks>
        /// <param name="categoryDTO">The data transfer object containing the details of the category to create. Cannot be null.</param>
        /// <returns>An HTTP 201 Created response containing the created category if successful; otherwise, an HTTP 400 Bad
        /// Request response if the input is invalid.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Create([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                return BadRequest("Insert category");
            }

            await service.Create(categoryDTO);
            return CreatedAtAction("Get", new { id = categoryDTO.Id }, categoryDTO);
        }

        /// <summary>
        /// Deletes the category with the specified identifier if it exists.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>A 204 No Content response if the category was successfully deleted; otherwise, a 404 Not Found response if
        /// the category does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var categoria = await service.GetById(id);
            if (categoria is null)
            {
                return NotFound("Categoria does not exist");
            }
            await service.Delete(id);
            return NoContent();


        }

        /// <summary>
        /// Updates the category with the specified identifier using the provided data.
        /// </summary>
        /// <remarks>Returns a 400 Bad Request response if <paramref name="categoryDTO"/> is null or if
        /// the identifiers do not match. Returns a 204 No Content response on successful update.</remarks>
        /// <param name="id">The unique identifier of the category to update. Must match the identifier in <paramref
        /// name="categoryDTO"/>.</param>
        /// <param name="categoryDTO">The data transfer object containing the updated category information. Cannot be null. The <c>Id</c> property
        /// must match <paramref name="id"/>.</param>
        /// <returns>A <see cref="NoContentResult"/> if the update is successful; otherwise, a <see
        /// cref="BadRequestObjectResult"/> if the input is invalid.</returns>
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(Guid id, [FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                return BadRequest("Insert category");
            }

            if (id != categoryDTO.Id)
            {
                return BadRequest("Ids do not match");
            }
            /*var categoria = await service.GetById(id);
            if (categoria is null)
            {
                return NotFound("Categoria does not exist");
            }*/

            await service.Update(categoryDTO);
            return NoContent();


        }


    }
}

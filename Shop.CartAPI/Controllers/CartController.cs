using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.CartAPI.Context;

namespace Shop.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;

        }

        [HttpGet]
        public async Task<ActionResult> GetCartItems()
        {
            var cartItems = _context.CartItems.ToList();
            return Ok(cartItems);
        }

    }
}

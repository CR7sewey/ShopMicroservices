using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.CartAPI.Context;
using Shop.CartAPI.Models.DTOs;
using Shop.CartAPI.Models.ViewModel;
using Shop.CartAPI.Repositories;
using Shop.CartAPI.Services;

namespace Shop.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CartService _cartService;

        public CartController(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, CartService cartService)
        {
            _context = applicationDbContext;
            _unitOfWork = unitOfWork;
            _cartService = cartService;

        }


        [HttpPost("createCart")]
        public async Task<ActionResult> CreateCart([FromBody] CartDTO cartDTO)
        {
            var createdCart = await _cartService.UpdateCart(cartDTO);
            if (createdCart == null)
            {
                return BadRequest();
            }
            return Ok(createdCart);

        }

        [HttpPost("updateCart")]
        public async Task<ActionResult> Update([FromBody] CartDTO cartDTO)
        {
            var createdCart = await _cartService.UpdateCart(cartDTO);
            if (createdCart == null)
            {
                return BadRequest();
            }
            return Ok(createdCart);

        }

        [HttpGet("getCart/{userId}")]
        public async Task<ActionResult> GetCartByUserId(Guid userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId); //await _unitOfWork.CartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpDelete("removeCartItem/{cartItemId}")]
        public async Task<ActionResult> RemoveCartItem(Guid cartItemId)
        {
            var isRemoved = await _cartService.RemoveItemCartAsync(cartItemId);
            if (!isRemoved)
            {
                return BadRequest();
            }
            return NoContent();


        }
    }
}

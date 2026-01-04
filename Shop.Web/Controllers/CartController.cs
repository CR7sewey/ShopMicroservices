using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Models;
using Shop.Web.Services;

namespace Shop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private string token = string.Empty;

        public CartController(ILogger<ProductsController> logger, IProductService productsService, ICategoryService categoryService, ICartService cartService)
        {
            _logger = logger;
            _productService = productsService;
            _categoryService = categoryService;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "01c3d0c8-3e3c-421c-b19d-c53d0bc751e5");
            var cart = await _cartService.GetCartByUserId(userId, await GetToken());
            if (cart == null)
            {
                cart = new CartViewModel();
            }
            cart.CartHeader.TotalAmount = CalculateTotalAmount(cart);


            return View(cart);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveItem(Guid id)
        {
            var userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "01c3d0c8-3e3c-421c-b19d-c53d0bc751e5");
            var result = await _cartService.RemoveFromCart(userId, id, await GetToken());
            if (result == false)
            {
                ViewBag.Erro = "Erro ao remover item do carrinho...";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GetToken()
        {
            return await HttpContext.GetTokenAsync("access_token");

            /*if (HttpContext.Request.Cookies["X-Access-Token"] == null)
            {
                return string.Empty;
            }
            return HttpContext.Request.Cookies["X-Access-Token"];*/
        }

        private double CalculateTotalAmount(CartViewModel cart)
        {
            double total = 0;
            if (cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    total += (double) (item.Quantity * item.Product.Price);
                }
            }
            return total;
        }   
    }
}

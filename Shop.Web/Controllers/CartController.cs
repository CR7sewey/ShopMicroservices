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
        private readonly ICouponService _couponService;
        private string token = string.Empty;

        public CartController(ILogger<ProductsController> logger, IProductService productsService, ICategoryService categoryService, ICartService cartService, ICouponService couponService)
        {
            _logger = logger;
            _productService = productsService;
            _categoryService = categoryService;
            _cartService = cartService;
            _couponService = couponService;
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

            if (cart.CartHeader != null && !string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode, await GetToken());
                if (coupon?.CouponCode != null)
                {
                    cart.CartHeader.Discount = coupon.DiscountAmount;
                }
            }
            cart.CartHeader.TotalAmount = CalculateTotalAmount(cart) * (1 - (double) cart.CartHeader.Discount/100);


            return View(cart);
        }

        [HttpDelete]
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

        [HttpPost]
        public async Task<ActionResult> ApplyCoupon(CartViewModel cartViewModel)
        {
            var userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "01c3d0c8-3e3c-421c-b19d-c53d0bc751e5");

            var coupon = await _couponService.GetCoupon(cartViewModel.CartHeader.CouponCode, await GetToken());
            if (coupon == null) {
                ViewBag.Erro = "Cupom inválido...";
                return RedirectToAction(nameof(Index));
            }
            if (coupon.ExpiryDate <= DateTime.Now)
            {
                ViewBag.Erro = "Cupom expirado...";
                return RedirectToAction(nameof(Index));
            }

            bool result = await _cartService.ApplyCoupon(cartViewModel, userId, await GetToken());

            if (result == false)
            {
                ViewBag.Erro = "Erro ao aplicar cupom...";
            }
            


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCoupon(CartViewModel cartViewModel)
        {
            var userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "01c3d0c8-3e3c-421c-b19d-c53d0bc751e5");
            bool result = await _cartService.RemoveCoupon(userId, await GetToken());
            if (result == false)
            {
                ViewBag.Erro = "Erro ao remover cupom...";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Checkout()
        {
            var userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "01c3d0c8-3e3c-421c-b19d-c53d0bc751e5");
            var cart = await _cartService.GetCartByUserId(userId, await GetToken());
            if (cart == null)
            {
                cart = new CartViewModel();
            }

            if (cart.CartHeader != null && !string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode, await GetToken());
                if (coupon?.CouponCode != null)
                {
                    cart.CartHeader.Discount = coupon.DiscountAmount;
                }
            }
            cart.CartHeader.TotalAmount = CalculateTotalAmount(cart) * (1 - (double)cart.CartHeader.Discount / 100);

            CheckoutViewModel checkoutViewModel = new CheckoutViewModel()
            {
                CartHeader = cart.CartHeader,
                CartItems = cart.CartItems
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutViewModel checkoutViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(checkoutViewModel);
            }
            var cart = await _cartService.GetCartByUserId(checkoutViewModel.CartHeader.UserId, await GetToken());
            checkoutViewModel.CartItems = cart.CartItems;
            checkoutViewModel.CartHeader = cart.CartHeader;
            var response = await _cartService.CheckoutCompleted(checkoutViewModel, await GetToken());
            
            if (response is null)
            {
                ViewBag.Erro = "Erro ao processar o pedido...";
                return View(checkoutViewModel);
            }
            return View(nameof(CheckoutCompleted));
        }

        [HttpGet]
        public IActionResult CheckoutCompleted()
        {
            return View();
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

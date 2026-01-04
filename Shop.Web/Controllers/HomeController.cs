using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Models;
using Shop.Web.Services;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Shop.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _productService = productService;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            if (products == null)
            {
                products = [];
            }
            return View(products);           
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
            {
                return View("Error");
            }
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize] // to send to identity server
        public async Task<ActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            /*Response.Cookies.Append("X-Access-Token", accessToken, new CookieOptions() // append to cookies to sned the requests with authentication
            {
                Secure = true, // proteger cookies durante o transporte - se houver alguma sessao htto o cookie nao é enviado
                HttpOnly = true, // impedir Cross site scripting - impede ataque xss
                SameSite = SameSiteMode.Strict // impedir o cross site forgery, cookie enviado apenas no contexto primario, site do cookie corresponder ao do url
            });*/
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Logout()
        {
            return SignOut("Cookies", "oidc");
        }


        [HttpPost]
        [ActionName("ProductDetails")]
        [Authorize]
        public async Task<ActionResult> AddToCart(ProductViewModel productViewModel)
        {
            var userId = Guid.Parse(User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? "01c3d0c8-3e3c-421c-b19d-c53d0bc751e5"); // pre definido para testes

            var cart = await _cartService.GetCartByUserId(userId, await GetToken());

            if (cart == null)
            {
                ProductViewModel productCart = (await _productService.GetProduct(productViewModel.Id));
                ProductCartViewModel productCartViewModel = new ()
                {
                    Id = productCart.Id,
                    Name = productCart.Name,
                    Description = productCart.Description,
                    Price = productCart.Price,
                    Stock = productCart.Stock,
                    ImageUrl = productCart.ImageUrl,
                    CategoryName = productCart.CategoryName
                };
                cart = new ()
                {
                    CartHeader = new CartHeaderViewModel
                    {
                        UserId = userId
                    },
                    CartItems = new List<CartItemViewModel>()
                    {
                        new CartItemViewModel
                        {
                            ProductId = productViewModel.Id,
                            Product = productCartViewModel,
                            Quantity = productCart.Quantity,
                            CartHeaderId = cart.CartHeader.Id
                        }
                    }
                };
                //var newCart = await _cartService.CreateUpdateCart(cart, await GetToken());
            }
            else
            {
                //var cartItem = cart.CartItems.Where(c => c.ProductId == productViewModel.Id).FirstOrDefault();
                //if (cartItem == null)
                //{
                    ProductViewModel productCart = (await _productService.GetProduct(productViewModel.Id));
                    ProductCartViewModel productCartViewModel = new ()
                    {
                        Id = productCart.Id,
                        Name = productCart.Name ?? string.Empty,
                        Description = productCart.Description ?? string.Empty,
                        Price = productCart.Price,
                        Stock = productCart.Stock,
                        ImageUrl = productCart.ImageUrl ?? string.Empty,
                        CategoryName = productCart.CategoryName ?? string.Empty
                    };
                    // no carrinho mas n tem o produto
                    var cartItem = new CartItemViewModel
                    {
                        ProductId = productViewModel.Id,
                        Quantity = productViewModel.Quantity,
                        Product = productCartViewModel,
                        CartHeaderId = cart.CartHeader.Id
                    };
                    //IEnumerable<CartItemViewModel> cartItemsNew = new List<CartItemViewModel> { cartItem };
                    //cartItems.Add(cartItem);
                    //cart.CartItems = cartItemsNew;

                //}
                /*else
                {
                    // ja tem o produto no carrinho
                    cartItem.Quantity += 1;
                    IEnumerable<CartItemViewModel> cartItemsNew = new List<CartItemViewModel> { cartItem };
                    //cartItems.Add(cartItem);
                    cart.CartItems = cartItemsNew;
                }*/
                IEnumerable<CartItemViewModel> cartItemsNew = new List<CartItemViewModel> { cartItem };
                //cartItems.Add(cartItem);
                cart.CartItems = cartItemsNew;
            }
            var updatedCart = await _cartService.AddToCart(cart, await GetToken());
            if (updatedCart is null)
            {
                return View(productViewModel);
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

    }

}

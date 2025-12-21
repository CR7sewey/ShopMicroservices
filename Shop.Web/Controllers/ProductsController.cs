using Microsoft.AspNetCore.Mvc;
using Shop.Web.Services;

namespace Shop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductsService _productService;

        public ProductsController(ILogger<ProductsController> logger, ProductsService productsService)
        {
            _logger = logger;
            _productService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            return View(products);
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Web.Models;
using Shop.Web.Roles;
using Shop.Web.Services;
using System.Collections;

namespace Shop.Web.Controllers
{
    [Authorize(Role.Admin)]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private string token = string.Empty;

        public ProductsController(ILogger<ProductsController> logger, IProductService productsService, ICategoryService categoryService)
        {
            _logger = logger;
            _productService = productsService;
            _categoryService = categoryService;
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

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult> CreateProduct()
        {
            var categories = await _categoryService.GetAllCategories(await GetToken());
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> CreateProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var p = await _productService.CreateProduct(productViewModel, await GetToken());
                if (p is not null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

           
            var categories = await _categoryService.GetAllCategories(await GetToken());
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            return View(productViewModel);
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult> EditProduct(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction(nameof(Index));
            }
            
            var product = await _productService.GetProduct(id);
            if (product is null)
            {
                return View("Error"); 
            }
            var categories = await _categoryService.GetAllCategories(await GetToken());
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");

            return View(product);
        }

        [HttpPost]
       // [Authorize]
        public async Task<ActionResult> EditProduct(Guid id, ProductViewModel productViewModel)
        {
            var prod = await _productService.UpdateProduct(id, productViewModel, await GetToken());
            if (prod == null)
            {
                ViewBag.CategoriaId = new SelectList(await _categoryService.GetAllCategories(token), "CategoriaId", "Nome"); // preenchido na view com um dropdown
                ViewBag.Erro = "Erro ao fazer update...";
                return View(productViewModel);
            }
            return RedirectToAction(nameof(Index)); ;
        }

        [HttpGet]
      //  [Authorize(Role.Admin)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return RedirectToAction(nameof(Index));
            }

            var product = await _productService.GetProduct(id);
            if (product is null)
            {
                return View("Error");
            }

            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
       // [Authorize(Role.Admin)]
        public async Task<ActionResult> DeleteProductConfirm(Guid id)
        {
            var prod = await _productService.DeleteProduct(id, await GetToken());
            if (prod == false)
            {
                ViewBag.Erro = "Erro ao fazer delete...";
                return View(id);
            }
            return RedirectToAction(nameof(Index)); ;
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

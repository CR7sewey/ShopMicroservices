using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
          var u = User;
            Console.WriteLine("User.Identity.IsAuthenticated: " + User.Identity.IsAuthenticated);
            return View();
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
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Logout()
        {
            return SignOut("Cookies", "oidc");
        }

    }

}

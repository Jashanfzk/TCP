using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TorontoCricketLeague.Models;

namespace TorontoCricketLeague.Controllers
{
    /// <summary>
    /// This controller handles home page and general navigation
    /// It provides endpoints for the main page, privacy page, and error handling
    /// </summary>
    /// <example>
    /// GET / - Returns the home page
    /// GET /Home/Privacy - Returns the privacy page
    /// GET /Home/Error - Returns error information
    /// </example>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebStokYApp.Models;
using WebStokYApp.Models.Entities;


namespace WebStokYApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult CustomerLogin()
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

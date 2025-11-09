using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using inxhsofti.Models;
using inxhsofti.Data;
using Microsoft.AspNetCore.Authentication;

namespace inxhsofti.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            
            var loggedInUser = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(loggedInUser))
            {
                
                return RedirectToAction("Login", "Account");
            }

            
            return RedirectToAction("Index", "Tasks");
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Account");
        }
    }
}
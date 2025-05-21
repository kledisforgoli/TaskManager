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
            // Check if the user is logged in using session
            var loggedInUser = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(loggedInUser))
            {
                // Redirect to login page if not logged in
                return RedirectToAction("Login", "Account");
            }

            // Since the user is logged in, we'll redirect to the Tasks index page
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
            // Clear the session and log out the user
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Account");
        }
    }
}
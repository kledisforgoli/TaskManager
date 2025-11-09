using Microsoft.AspNetCore.Mvc;
using inxhsofti.Models;
using System.Text.Encodings.Web;
using inxhsofti.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace inxhsofti.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email;
            var password = model.Password;

            email = HtmlEncoder.Default.Encode(email);
            password = HtmlEncoder.Default.Encode(password);

            if (email.Contains("'") || email.Contains("--"))
            {
                ViewBag.Error = "Email jo i vlefshëm.";
                return View(model);
            }

            User? existingUser = _context.Users.FirstOrDefault(u => u.Username == email);
            if (existingUser != null)
            {
                ViewBag.Error = "Ky email është regjistruar më parë.";
                return View(model);
            }

            var user = new User { Username = email, Password = password };
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Regjistrimi u krye me sukses! Tani mund të hyni në llogarinë tuaj.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            
            if (HttpContext.Session.GetString("user") != null)
            {
                return RedirectToAction("Index", "Tasks");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var encodedEmail = HtmlEncoder.Default.Encode(email);
            var encodedPassword = HtmlEncoder.Default.Encode(password);

            var user = _context.Users.FirstOrDefault(u => u.Username == encodedEmail && u.Password == encodedPassword);
            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                
                await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
                {
                    IsPersistent = false 
                });

                HttpContext.Session.SetString("user", user.Username);

                TempData["SuccessMessage"] = "Ju jeni loguar me sukses!";
                return RedirectToAction("Index", "Tasks");
            }

            TempData["ErrorMessage"] = "Kredencialet janë të pasakta.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
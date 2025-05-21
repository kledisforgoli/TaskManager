using inxhsofti.Data;
using inxhsofti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace inxhsofti.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Create()
        {
            // Check if user is logged in
            var loggedInUser = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(loggedInUser))
            {
                TempData["ErrorMessage"] = "Ju duhet të logoheni për të krijuar detyra.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskItem task)
        {
            try
            {
                _logger.LogInformation("Fillimi i metodes Create");

                var loggedInUser = HttpContext.Session.GetString("user");
                _logger.LogInformation($"Përdoruesi i loguar: {loggedInUser}");

                if (string.IsNullOrEmpty(loggedInUser))
                {
                    TempData["ErrorMessage"] = "Përdoruesi nuk është i loguar.";
                    return RedirectToAction("Login", "Account");
                }

                var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
                _logger.LogInformation($"Përdoruesi u gjet: {user != null}");

                if (user == null)
                {
                    TempData["ErrorMessage"] = "Përdoruesi nuk u gjet.";
                    return RedirectToAction("Login", "Account");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState jo valid");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogWarning($"Gabim: {error.ErrorMessage}");
                    }
                    return View(task);
                }

                // Vendos UserId dhe ruaj detyrën
                task.UserId = user.Id;
                _logger.LogInformation($"Detyra para ruajtjes - Titulli: {task.Title}, UserId: {task.UserId}");

                _context.Tasks.Add(task);
                var result = _context.SaveChanges();
                _logger.LogInformation($"Rezultati i SaveChanges: {result}");

                TempData["SuccessMessage"] = "Detyra u shtua me sukses.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gabim gjatë ruajtjes së detyrës");
                TempData["ErrorMessage"] = $"Ka ndodhur një gabim: {ex.Message}";
                return View(task);
            }
        }

        public IActionResult Index()
        {
            var loggedInUser = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(loggedInUser))
            {
                TempData["ErrorMessage"] = "Ju duhet të logoheni për të parë detyrat.";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Përdoruesi nuk u gjet.";
                    return RedirectToAction("Login", "Account");
                }

                // Merr detyrat duke përfshirë të dhënat e përdoruesit
                var tasks = _context.Tasks
                    .Where(t => t.UserId == user.Id)
                    .ToList();

                _logger.LogInformation($"U gjetën {tasks.Count} detyra për përdoruesin {user.Id}");

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gabim gjatë marrjes së detyrave");
                TempData["ErrorMessage"] = $"Ka ndodhur një gabim: {ex.Message}";
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            // Check if the task belongs to the logged in user
            var loggedInUser = HttpContext.Session.GetString("user");
            var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
            if (user == null || task.UserId != user.Id)
            {
                return Forbid();
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loggedInUser = HttpContext.Session.GetString("user");
                    var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
                    if (user != null)
                    {
                        // Verify the task belongs to this user
                        var existingTask = _context.Tasks.Find(task.Id);
                        if (existingTask != null && existingTask.UserId == user.Id)
                        {
                            existingTask.Title = task.Title;
                            existingTask.Description = task.Description;
                            existingTask.DueDate = task.DueDate;
                            existingTask.IsCompleted = task.IsCompleted;

                            _context.Update(existingTask);
                            _context.SaveChanges();

                            TempData["SuccessMessage"] = "Detyra u përditësua me sukses.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    TempData["ErrorMessage"] = "Nuk keni autorizim për të ndryshuar këtë detyrë.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Ka ndodhur një gabim gjatë përditësimit të detyrës: {ex.Message}";
                    _logger.LogError(ex, "Error updating task");
                }
            }

            return View(task);
        }

        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            // Check if the task belongs to the logged in user
            var loggedInUser = HttpContext.Session.GetString("user");
            var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
            if (user == null || task.UserId != user.Id)
            {
                return Forbid();
            }

            return View(task);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            // Check if the task belongs to the logged in user
            var loggedInUser = HttpContext.Session.GetString("user");
            var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
            if (user == null || task.UserId != user.Id)
            {
                return Forbid();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Detyra u fshi me sukses.";
            return RedirectToAction(nameof(Index));
        }
    }
}
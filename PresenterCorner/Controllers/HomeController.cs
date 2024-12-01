using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresenterCorner.Data;
using PresenterCorner.Models;
using System.Diagnostics;

namespace PresenterCorner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Nickname))
            {
                ModelState.AddModelError(nameof(user.Nickname),"The nickname must not be empty");
                return View("Index");
            }
            var userFromDb = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Nickname == user.Nickname);
            if (userFromDb == null)
            {
                userFromDb = new User
                {
                    Nickname = user.Nickname,
                    Role = "Viewer", // Default role
                    PresentationId = 0, // Not assigned to any presentation yet
                    ConnectionId = string.IsNullOrEmpty(user.ConnectionId) ? "" : user.ConnectionId
                };
                _context.Users.Add(userFromDb);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index","Presentation");
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
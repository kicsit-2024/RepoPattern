using Microsoft.AspNetCore.Mvc;
using RepoPattern.Data;
using RepoPattern.Models;
using System.Diagnostics;

namespace RepoPattern.Controllers
{
    [CustomAuthorize("Admin", "Moderator")]
    public class HomeController(AppDbContext _context) : Controller
    {
        [CustomAuthorize("Admin", "Moderator")]
        public IActionResult Index()
        {
            var userIdStr = HttpContext.Session.GetString("LoggedInUserId");
            if (!string.IsNullOrEmpty(userIdStr))
            {
                var user = _context.Users.Where(m => m.Id.ToString() == userIdStr).FirstOrDefault();

            }

            return View();
        }
        
        [CustomAuthorize("Admin")]
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

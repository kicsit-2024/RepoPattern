using Microsoft.AspNetCore.Mvc;
using RepoPattern.Data;
using RepoPattern.Models;

namespace RepoPattern.Controllers
{
    public class AccountsController(AppDbContext _context) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.Users.Where(m => m.Email == model.LoginId).FirstOrDefault();
            //if (user == null)
            //{
            //    ModelState.AddModelError("LoginId", "User not found with given email");
            //    return View(model);
            //}
            //else if(!user.ValidatePassword(model.Password))
            //{
            //    ModelState.AddModelError("Password", "Password not valid.");
            //    return View(model);
            //}

            if (user == null || !user.ValidatePassword(model.Password))
            {
                ModelState.AddModelError("", "Login Id or Password not valid.");
                return View(model);
            }

            HttpContext.Session.SetString("LoggedInUserId", user.Id.ToString());
            return View();
        }
    }
}

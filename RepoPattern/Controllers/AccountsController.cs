using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RepoPattern.Data;
using RepoPattern.Handlers;
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
            var user = _context.Users.Include(m => m.Roles).Where(m => m.Email == model.LoginId).FirstOrDefault();
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

            //HttpContext.Session.SetString("LoggedInUserId", user.Id.ToString());
            AuthViewModel authViewModel = new AuthViewModel()
            {
                UserId = user.Id,
                Roles = user.Roles.Select(m => m.Name).ToList(),
                ValidUpto = model.RememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(7),
            };

            var serilizedAuth = JsonConvert.SerializeObject(authViewModel);
            var encryptedAuth = EncryptionHelper.EncryptString(serilizedAuth);
            HttpContext.Response.Cookies.Append("AuthToken", encryptedAuth, new CookieOptions
            {
                IsEssential = true,
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(100)
                //Secure = true
            });

            return RedirectToAction("Index", "Home");
        }
    }
}

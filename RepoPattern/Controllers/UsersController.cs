using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepoPattern.Data;
using RepoPattern.Models;

namespace RepoPattern.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Uers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Uers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // GET: Uers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Password")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                string password = appUser.Password;
                appUser.Password = "";
                _context.Add(appUser);
                //_context.Entry(appUser).GetDatabaseValues();

                await _context.SaveChangesAsync();

                if (_context.Users.Count() == 1)
                {
                    var adminRole = _context.Roles.Where(m => m.Name == "Admin").FirstOrDefault();
                    if (adminRole == null)
                    {
                        adminRole = new AppRole { Name = "Admin" };
                        _context.Roles.Add(adminRole);

                        var moderator = new AppRole { Name = "Moderator" };
                        _context.Roles.Add(moderator);
                        _context.SaveChanges();
                        //_context.Entry(adminRole).GetDatabaseValues();
                    }
                    //appUser.Roles = _context.Roles.ToList();
                    //appUser.Roles ??= [];
                    //appUser.Roles.Add(adminRole);
                    appUser.Roles = [adminRole];
                    //_context.Entry(appUser.Roles).State = EntityState.Added;
                }
                appUser.EncryptPassword();
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Uers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: Uers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password")] AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Uers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: Uers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appUser = await _context.Users.FindAsync(id);
            if (appUser != null)
            {
                _context.Users.Remove(appUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

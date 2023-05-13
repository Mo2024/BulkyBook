using Microsoft.AspNetCore.Mvc;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using BulkyBookWeb.Data;

namespace BulkyBookWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET Register
        public IActionResult Register()
        {
            return View();
        }

        //POST Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register obj )
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.Email = obj.Email; 
                user.Password = obj.Password;
                user.Username = obj.Username;

                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, obj.Password);

                _db.Add(user);
                _db.SaveChanges();

                HttpContext.Session.SetString("UserId", user.Id.ToString());



                return RedirectToAction("Index", "Home");


            }
            else
            {
                TempData["error"] = "Invalid data type!";
                return View(obj);
            }
        }
    }
}

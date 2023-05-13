using Microsoft.AspNetCore.Mvc;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using BulkyBookWeb.Data;


namespace BulkyBookWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();

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

                user.Password = _passwordHasher.HashPassword(user, obj.Password);

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

        //GET Login
        public IActionResult Login()
        {
            return View();
        }

        //POST Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login obj)
        {
            if (ModelState.IsValid)
            {
                var User = _db.Users.FirstOrDefault(u => u.Username == obj.Username);
                //var User = _db.Users.Find(obj.Username);
                if(User == null)
                {
                    return NotFound();
                }
                else
                {
                    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(User, User.Password, obj.Password);
                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("UserId", User.Id.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["error"] = "Invalid Password!";
                        return View(obj);
                    }

                }
            }
            else
            {
                TempData["error"] = "Invalid data type!";
                return View(obj);
            }
        }
    }
}

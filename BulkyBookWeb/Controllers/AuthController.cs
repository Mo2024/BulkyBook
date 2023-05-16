using Microsoft.AspNetCore.Mvc;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using BulkyBookWeb.Data;
using BulkyBookWeb.Services;

namespace BulkyBookWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IEmailSenderService _emailSender;
        private readonly IGenerateTokenService _generateToken;

        public AuthController(ApplicationDbContext db, IEmailSenderService emailSender, IGenerateTokenService generateToken)
        {
            _db = db;
            _emailSender = emailSender;
            _generateToken = generateToken;
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
        public ActionResult Register(Register obj )
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = obj.Email,
                    Username = obj.Username,
                    EmailVerificationToken = _generateToken.GenerateToken(),
                    IsEmailVerified = false,
                    Role = Role.Admin
                };
                user.Password = _passwordHasher.HashPassword(user, obj.Password);

                _db.Add(user);
                _db.SaveChanges();

                string href = "https://localhost:7223/Auth/Verify?token=" + user.EmailVerificationToken.ToString();
                _emailSender.SendEmail(user.Email, "Email Verification", href);
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserRole", user.Role.ToString());
                TempData["success"] = "Account successfully created, check your email to verify your account";
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
                        HttpContext.Session.SetString("UserRole", User.Role.ToString());
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

        //GET Verify Account
        public IActionResult Verify(string? token)
        {
            string userId = HttpContext.Session.GetString("UserId");
            var user = _db.Users.Find(int.Parse(userId));
            if (token == null)
            {
                TempData["error"] = "Invalid Token";
                return RedirectToAction("Index", "Home");
            }
            else if(userId == null)
            {
                TempData["error"] = "Must login first";
                return RedirectToAction("Index", "Home");
            }

            //if (user.IsEmailVerified)
            //{
                Guid TokenRoute;
                if (Guid.TryParse(token, out TokenRoute))
                {

                    if (TokenRoute.Equals(user.EmailVerificationToken))
                    {
                        user.IsEmailVerified = true;
                        user.EmailVerificationToken = Guid.Empty;
                        _db.SaveChanges();
                        TempData["success"] = "Email verification success!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["error"] = "Invalid token!";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["error"] = "Invalid token!";
                    return RedirectToAction("Index", "Home");
                }
            //}
            //else
            //{
            //    TempData["error"] = "Email already verified";
            //    return RedirectToAction("Index", "Home");
            //}

        }
    }
}

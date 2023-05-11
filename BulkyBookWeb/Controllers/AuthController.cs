using Microsoft.AspNetCore.Mvc;
using BulkyBookWeb.Models;

namespace BulkyBookWeb.Controllers
{
    public class AuthController : Controller
    {
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
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid data type!";
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}

using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using BulkyBookWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BulkyBookWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("Create/Product")]
        public IActionResult CreateProduct()
        {
            var CreateProductViewModel = new CreateProductViewModel
            {
                Product = new Product(),
                categories = _db.Categories.ToList()

            };


            return View(CreateProductViewModel);

        }
    }
}

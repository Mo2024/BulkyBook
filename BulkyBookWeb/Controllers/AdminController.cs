using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using BulkyBookWeb.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;

namespace BulkyBookWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UploadImageService _uploadImageService;

        public AdminController(ApplicationDbContext db, UploadImageService uploadImageService)
        {
            _db = db;
            _uploadImageService = uploadImageService;
        }

        public IActionResult Index()
        {
            return View();
        }
        //GET
        [HttpGet("/Admin/Create/Category")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        //POST
        [HttpPost("/Admin/Create/Category")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        [HttpGet("/Admin/Create/Product")]
        public IActionResult CreateProduct()
        {
            var categories = _db.Categories.ToList();
            var CreateProductViewModel = new CreateProductViewModel
            {
                Product = new Product(),
                categories = categories

            };
            return View(CreateProductViewModel);

        }

        [HttpPost("/Admin/Create/Product")]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel CreateProductViewModel)
        {
            if (ModelState.IsValid)
            {
                Product Product = CreateProductViewModel.Product;
                foreach (var image in Product.ImageData)
                {
                    var uploadResult = await _uploadImageService.UploadImageToCloudinary(image);
                    var newImage = new Image
                    {
                        Url = uploadResult.SecureUrl.ToString(),
                        Product = Product
                    };
                    Product.Images.Add(newImage);
                    Console.WriteLine(uploadResult);

                }
                _db.Products.Add(Product);
                _db.SaveChanges();
                TempData["success"] = "Product created successfully";                
                //foreach (var file in Product.ImageData)
                //{
                //    if (file != null && file.Length > 0)
                //    {
                //        using var memoryStream = new MemoryStream();
                //        file.CopyTo(memoryStream);
                //        Product.ImageDataBytes.Add(memoryStream.ToArray());
                //    }
                //}
                //TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            

            return View(CreateProductViewModel);

        }
    }
}

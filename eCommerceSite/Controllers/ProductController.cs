using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;
        public ProductController(ProductContext context) 
        {
            _context = context;
        }
        /// <summary>
        /// Displays a view that lists all products
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //Get all products from database
            List<Product> products = _context.Products.ToList();

            //Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product p)
        {
            if (ModelState.IsValid)
            {
                //Add to db
                _context.Products.Add(p);
                _context.SaveChanges();

                TempData["Message"] = $"Item #{p.ProductId} : {p.Title} was added successfully!";
                TempData["Details"] = $"ID : {p.ProductId} , Title : {p.Title} , Category : {p.Category} , Price : {p.Price}";

                //redirect back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}

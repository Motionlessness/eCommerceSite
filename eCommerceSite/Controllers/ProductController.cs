using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// Displays a view that lists a page of products
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? id)
        {
            int pageNum = id ?? 1;
            const int PageSize = 3;
            ViewData["CurrentPage"] = pageNum;

            int numProducts = await (from p in _context.Products
                               select p).CountAsync();
            int totalPages = (int)Math.Ceiling((double)numProducts / PageSize);

            ViewData["MaxPage"] = totalPages;

            //Get 3 products from database per page
            List<Product> products = await (from p in _context.Products
                                            orderby p.Title ascending
                                            select p)
                                            .Skip(PageSize*(pageNum-1))
                                            .Take(PageSize)
                                            .ToListAsync();

            //Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product p)
        {
            if (ModelState.IsValid)
            {
                //Add to db
                _context.Products.Add(p);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Item #{p.ProductId} : {p.Title} was added successfully!";
                TempData["Details"] = $"ID : {p.ProductId} , Title : {p.Title} , Category : {p.Category} , Price : {p.Price}";

                //redirect back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //get product with corresponding id

            /*Product p = await (from prod in _context.Products
                               where prod.ProductId == id
                               select prod).SingleAsync();*/

            Product p = await _context
                .Products
                .Where(prod => prod.ProductId == id)
                .SingleAsync();

            //pass product to view
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(p).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                TempData["Message"] = $"Item #{p.ProductId} : {p.Title} was edited successfully!";
                TempData["Details"] = $"ID : {p.ProductId} , Title : {p.Title} , Category : {p.Category} , Price : {p.Price}";

                return RedirectToAction("Index");
            }

            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product p = await _context
                .Products
                .Where(prod => prod.ProductId == id)
                .SingleAsync();
            return View(p);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product p = await (from prod in _context.Products
                where(prod.ProductId == id)
                select prod).SingleAsync();


            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Item #{p.ProductId} : {p.Title} was deleted successfully!";
            TempData["Details"] = $"ID : {p.ProductId} , Title : {p.Title} , Category : {p.Category} , Price : {p.Price}";

            return RedirectToAction("Index");
        }
    }
}

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

            int numProducts = await ProductDb.GetTotalProductsAsync(_context);

            int totalPages = (int)Math.Ceiling((double)numProducts / PageSize);

            ViewData["MaxPage"] = totalPages;

            List<Product> products = await ProductDb.GetProductsAsync(_context, PageSize, pageNum);

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
                await ProductDb.AddProductAsync(_context, p);
                

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

            Product p = await ProductDb.GetSingleProductAsync(_context, id);

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
            Product p = await ProductDb.GetSingleProductAsync(_context, id);
            return View(p);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product p = await ProductDb.GetSingleProductAsync(_context, id);


            _context.Entry(p).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Item #{p.ProductId} : {p.Title} was deleted successfully!";
            TempData["Details"] = $"ID : {p.ProductId} , Title : {p.Title} , Category : {p.Category} , Price : {p.Price}";

            return RedirectToAction("Index");
        }
    }
}

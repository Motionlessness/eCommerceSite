using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public CartController(ProductContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }
        public async Task<IActionResult> Add(int id)
        {
            Product p = await ProductDb.GetSingleProductAsync(_context, id);

            CookieHelper.AddProductToCart(_httpContext, p);

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Summary()
        {
            return View(CookieHelper.GetCartProducts(_httpContext));
        }
    }
}

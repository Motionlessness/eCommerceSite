using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Data
{
    public static class ProductDb
    {
        /// <summary>
        /// Returns the total count of products
        /// </summary>
        /// <param name="_context">Database Context to use</param>
        public async static Task<int> GetTotalProductsAsync(ProductContext _context)
        {
            return await (from p in _context.Products
                          select p).CountAsync();
        }

        /// <summary>
        /// Get one page worth of products
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="pageSize">The number of products per page</param>
        /// <param name="pageNum">Page of products to return</param>
        /// <returns></returns>
        public async static Task<List<Product>> GetProductsAsync(ProductContext _context, int pageSize, int pageNum)
        {
            return await (from p in _context.Products
                          orderby p.Title ascending
                          select p)
                          .Skip(pageSize * (pageNum - 1))
                          .Take(pageSize)
                          .ToListAsync();
        }
        public async static Task<Product> AddProductAsync(ProductContext _context, Product p)
        {
            _context.Products.Add(p);
            await _context.SaveChangesAsync();
            return p;
        }

        public async static Task<Product> GetSingleProductAsync(ProductContext _context, int id)
        {
           Product p = await _context.Products
                .Where(prod => prod.ProductId == id)
                .SingleAsync();
            return p;
        }
    }
}

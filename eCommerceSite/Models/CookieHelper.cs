using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public static class CookieHelper
    {
        const string CartCookie = "CartCookie";
        /// <summary>
        /// Returns the current list of cart products. If cart is empty and empty list will be returned.
        /// </summary>
        /// <param name="http"></param>
        /// <returns>An empty list if cart is empty</returns>
        public static List<Product> GetCartProducts(IHttpContextAccessor http)
        {
            string userCart = http.HttpContext.Request.Cookies[CartCookie];

            List<Product> cartProducts = new List<Product>();

            if (userCart != null)
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(userCart);
            }
            return cartProducts;
        }

        public static void AddProductToCart(IHttpContextAccessor http, Product prod)
        {
            List<Product> cartProducts = GetCartProducts(http);
            cartProducts.Add(prod);

            string data = JsonConvert.SerializeObject(cartProducts);
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1),
                Secure = true,
                IsEssential = true
            };
            http.HttpContext.Response.Cookies.Append(CartCookie, data, options);
        }

        public static int GetAllCartProducts(IHttpContextAccessor http)
        {
            throw new NotImplementedException();
        }
    }
}

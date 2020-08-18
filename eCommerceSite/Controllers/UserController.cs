using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eCommerceSite.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductContext _context;
        public UserController(ProductContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel regi)
        {
            if (ModelState.IsValid)
            {
                UserAccount user = new UserAccount()
                {
                    DateOfBirth = regi.DateOfBirth ,
                    Email = regi.Email ,
                    Password = regi.Password ,
                    Username = regi.UserName
                };

                _context.Accounts.Add(user);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Welcome {user.Username}, registration was a success!";
                return RedirectToAction("Index", "Home");
            }
            return View(regi);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel logger)
        {
            if (!ModelState.IsValid) { return View(logger); }

            //UserAccount account = await (from u in _context.Accounts
            //                             where (u.Username == logger.UsernameOrEmail ||
            //                             u.Email == logger.UsernameOrEmail) &&
            //                             u.Password == logger.Password
            //                             select u).SingleOrDefaultAsync();

            UserAccount account = 
                await (_context.Accounts
                    .Where(ua => (ua.Username == logger.UsernameOrEmail ||
                                ua.Email == logger.UsernameOrEmail) &&
                                ua.Password == logger.Password)
                .SingleOrDefaultAsync());

            if(account == null)
            {
                ModelState.AddModelError(string.Empty, "Credentials were not found");

                return View(logger);
            }

            HttpContext.Session.SetInt32("UserId", account.UserId);

            return RedirectToAction("Index", "Home");
        }
    }
}

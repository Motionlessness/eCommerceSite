using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
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
    }
}

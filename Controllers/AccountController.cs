using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;

namespace ProductStore.Controllers
{
    public class AccountController : Controller
    {
        private DataContext db;
        public AccountController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                   
                user = new User {Name=model.Name,
                    Email = model.Email,
                    PhoneNumber=model.PhoneNumber,
                    Password = model.Password };
                Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "customer");
                if (userRole != null)
                    user.Role = userRole;

                db.Users.Add(user);
                Cart cart = new Cart() { User = user};
                db.Carts.Add(cart);
                await db.SaveChangesAsync();
                await Authenticate(user); 
                return RedirectToAction("GoToIndex", "NavBar");
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            User user = await db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                await Authenticate(user);
                return RedirectToAction("GoToIndex", "NavBar");
            }
              
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            ViewBag.IsCurrentUserAuthenticated = User.Identity.IsAuthenticated;
            return RedirectToAction("GoToIndex", "NavBar");
        }

        private async Task Authenticate(User user)
        {
           
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
           
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    public class NavBarController : Controller
    {
        
        public IActionResult GoToIndex()
        {
            string cookieValue_name = Request.Cookies["NameFilter"];
            string cookieValue_sortBy = Request.Cookies["SortBy"];
            string cookieValue_circle = Request.Cookies["Circle"];
            string cookieValue_triangle = Request.Cookies["Triangle"];
            string cookieValue_square = Request.Cookies["Square"];
            string cookieValue_blue = Request.Cookies["Blue"];
            string cookieValue_red = Request.Cookies["Red"];
            string cookieValue_yellow = Request.Cookies["Yellow"];
            string cookieValue_page = Request.Cookies["Page"];
            return RedirectToAction("Index", "Home", new
            {
                name = cookieValue_name,
                sortBy = cookieValue_sortBy,
                circle = cookieValue_circle,
                triangle = cookieValue_triangle,
                square = cookieValue_square,
                blue = cookieValue_blue,
                red = cookieValue_red,
                yellow = cookieValue_yellow,
                page = cookieValue_page
            });
        }
        public IActionResult Contacts()
        {

            return View();
        }
        public IActionResult Faq()
        {

            return View();
        }
        public IActionResult Aboutus()
        {

            return View();
        }
    }
}

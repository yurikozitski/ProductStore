using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductStore.Models;
using ProductStore.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace ProductStore.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db;
        private IProductsProvider gp;
        public HomeController(DataContext context, IProductsProvider getProducts)
        {
            gp = getProducts;
            db = context;
            if (db.Products.Count() == 0)
            {
                Product redsquare = new Product { Name = "Red Square", Shape="square",Color="red", Price = 19.99, Image = @"\Images\Redsquare.png",Quantity=10 };
                Product bluesquare = new Product { Name = "Blue Square", Shape = "square", Color = "blue", Price = 14.99, Image = @"\Images\1200px-000080_Navy_Blue_Square.png", Quantity = 10 };
                Product yellowsquare = new Product { Name = "Yellow Square", Shape = "square", Color = "yellow", Price = 20.99, Image = @"\Images\1024px-Square_Yellow.png", Quantity = 10 };
                Product bluecircle = new Product { Name = "Blue Circle", Shape = "circle", Color = "blue", Price = 9.99, Image = @"\Images\Ski_trail_rating_symbol_blue_circle.png", Quantity = 10 };
                Product redcircle = new Product { Name = "Red Circle", Shape = "circle", Color = "red", Price = 30.99, Image = @"\Images\Ski_trail_rating_symbol_red_circle.png", Quantity = 10 };
                Product yellowcircle = new Product { Name = "Yellow Circle", Shape = "circle", Color = "yellow", Price = 27.99, Image = @"\Images\yellowcircle.png", Quantity = 10 };
                Product yellowtriangle = new Product { Name = "Yellow Triangle", Shape = "triangle", Color = "yellow", Price = 29.99, Image = @"\Images\yellowtriangle.png", Quantity = 10 };
                Product redtriangle = new Product { Name = "Red Triangle", Shape = "triangle", Color = "red", Price = 12.99, Image = @"\Images\redtriangle.png", Quantity = 10 };
                Product bluetriangle = new Product { Name = "Blue Triangle", Shape = "triangle", Color = "blue", Price = 18.99, Image = @"\Images\bluetriangle.png", Quantity = 10 };
                db.Products.AddRange(redsquare,bluecircle,yellowtriangle,bluetriangle,yellowsquare,redtriangle,bluesquare,redcircle,yellowcircle);
                db.SaveChanges();
            }
        }
        int pageSize = 8;
        
        public async Task<IActionResult> Index(string sortBy,string circle,string triangle,string square,
            string blue,string red, string yellow,string name, int page = 1)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                HttpContext.Response.Cookies.Append("UserName", $"{user.Name}");
            }
            ViewBag.SortBy = sortBy;
            ViewBag.Circle = circle;
            ViewBag.Triangle = triangle;
            ViewBag.Square = square;
            ViewBag.Blue = blue;
            ViewBag.Red = red;
            ViewBag.Yellow = yellow;
            ViewBag.Name = name;
            db.Products.Load();
            var products = db.Products.Where(x=>x.Name!=null);

            
            if (!String.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Contains(name));
            }
            if (circle == null && triangle == null && square == null && blue == null && red == null && yellow == null)
            {
                goto M1;
            }
            else
            {
                if (circle == null && triangle == null && square == null) goto M2;
                
                if (circle == null && circle != "on")
                {
                    products = products.Where(p => p.Shape != "circle");
                }
                if (triangle == null && triangle != "on")
                {
                    products = products.Where(p => p.Shape != "triangle");
                }
                if (square == null && square != "on")
                {
                    products = products.Where(p => p.Shape != "square");
                }
                
                if (blue == null && red == null && yellow == null) goto M1;
                M2:
                if (blue == null && blue != "on")
                {
                    products = products.Where(p => p.Color != "blue");
                }
                if (red == null && red != "on")
                {
                    products = products.Where(p => p.Color != "red");
                }
                if (yellow == null && yellow != "on")
                {
                    products = products.Where(p => p.Color != "yellow");
                }
            }
            
            M1:
            switch (sortBy)
            {
                case "byNameAz":
                    products = products.OrderBy(s => s.Name);
                    break;
                case "byNameZa":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "byPrice_cp_to_exp":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "byPrice_ex_to_cp":
                    products = products.OrderByDescending(s => s.Price);
                    break;
            }
            var count = await products.CountAsync();
            var items = await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Products = items
            };
            HttpContext.Response.Cookies.Append("NameFilter", $"{name}");
            HttpContext.Response.Cookies.Append("SortBy", $"{sortBy}");
            HttpContext.Response.Cookies.Append("Circle", $"{circle}");
            HttpContext.Response.Cookies.Append("Triangle", $"{triangle}");
            HttpContext.Response.Cookies.Append("Square", $"{square}");
            HttpContext.Response.Cookies.Append("Blue", $"{blue}");
            HttpContext.Response.Cookies.Append("Red", $"{red}");
            HttpContext.Response.Cookies.Append("Yellow", $"{yellow}");
            HttpContext.Response.Cookies.Append("Page", $"{page}");
            return View(viewModel);

        }

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> AddtoCart(int productId, int productQuantity)
        {
            string productIdstring = productId.ToString();
            string productQuantitystring = productQuantity.ToString(); 
            Cart cart1 = await db.Carts.FirstOrDefaultAsync(c => c.User.Email == User.Identity.Name);
            if (cart1.ProductsInCart != null)
            {
                string ProductsInCart = cart1.ProductsInCart;
                char[] separators = new char[] { ',', '=' };
                string[] subs = ProductsInCart.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                bool isTheSame = false;
                for(int i=0; i < subs.Length; i += 2)
                {
                    if(subs[i]== productIdstring)
                    {
                        int subs_i_plus_one= Int32.Parse(subs[i+1])+productQuantity;
                        subs[i + 1] = subs_i_plus_one.ToString();
                        isTheSame = true;
                    }
                }
                for (int i = 0; i < subs.Length; i++)
                {
                    if (i == 0 || i % 2 == 0)
                    {
                        subs[i] += "=";
                    }
                    else
                    {
                        subs[i] += ",";
                    }
                }
                if (isTheSame) 
                {
                    cart1.ProductsInCart = String.Concat(subs);
                    await db.SaveChangesAsync();
                    return RedirectToAction("GoToIndex", "NavBar");
                } 
            }
            cart1.ProductsInCart += productIdstring + "=" + productQuantity + ",";
            await db.SaveChangesAsync();
            return RedirectToAction("GoToIndex", "NavBar");
            
            
        }
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> Cart(int? deletedProductId=null)
        {
            Cart cart = await db.Carts.Include(c => c.User).FirstOrDefaultAsync(c => c.User.Email == User.Identity.Name);
            string ProductsInCart = cart.ProductsInCart;
            if (ProductsInCart != null)
            {
                if (deletedProductId != null)
                {
                    char[] separators = new char[] { ',', '=' };
                    string[] subs = ProductsInCart.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < subs.Length; i += 2)
                    {
                        if (subs[i] == deletedProductId.ToString())
                        {
                            subs = subs.Where((source, index) => index != i).ToArray();
                            subs = subs.Where((source, index) => index != i).ToArray();

                        }
                    }
                    for (int i = 0; i < subs.Length; i++)
                    {
                        if (i == 0 || i % 2 == 0)
                        {
                            subs[i] += "=";
                        }
                        else
                        {
                            subs[i] += ",";
                        }
                    }
                    string newProducts= String.Concat(subs);
                    ProductsInCart = newProducts;
                    if (ProductsInCart == "")
                    {
                        cart.ProductsInCart = null;
                        await db.SaveChangesAsync();
                        return View();
                    }
                    cart.ProductsInCart = newProducts;
                    await db.SaveChangesAsync(); 
                }
                List<Product> cartProducts = await gp.GetProductListAsync(ProductsInCart);
                
                return View(cartProducts);
            }
            else return View();
        }
        
    }
    
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductStore.Models;
using ProductStore.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
namespace ProductStore.Controllers
{
    public class OrderController : Controller
    {
        private DataContext db;
        private IProductsProvider gp;
        public OrderController(DataContext context, IProductsProvider getProducts)
        {
            gp = getProducts;
            db = context;
        }

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> ConfirmOrder()
        {
            Cart cart = await db.Carts.Include(c => c.User).FirstOrDefaultAsync(c => c.User.Email == User.Identity.Name);
            Order order = new Order() { User = cart.User, ProductsOrdered = cart.ProductsInCart, DateOfOrder = DateTime.Now };
            cart.ProductsInCart = null;
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Cart", "Home");
        }

        [Authorize(Roles = "customer,manager")]
        public async Task<IActionResult> OrderView()
        {
            
            var orders = new List<Order>();
            if (User.Identity.IsAuthenticated == true && User.IsInRole("customer"))
            {
                orders = await db.Orders.Include(o => o.User).Where(o => o.User.Email == User.Identity.Name).ToListAsync();
            }
            else if (User.Identity.IsAuthenticated == true && User.IsInRole("manager")) 
            {
                orders = await db.Orders.Include(o => o.User).ToListAsync();
            }
            if (orders.Count==0)
            {
                return View();
            }
            List<OrderViewModel> ordersViewModel = new List<OrderViewModel>();
            foreach (Order order in orders)
            {
                OrderViewModel orderViewModel = new OrderViewModel() { Order = order };
                orderViewModel.ProductsOrdered= await gp.GetProductListAsync(order.ProductsOrdered);
                ordersViewModel.Add(orderViewModel);
            }
            return View(ordersViewModel);
        }
    }
}

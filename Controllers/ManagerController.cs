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
using System.Security.Claims;
using System.Globalization;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "manager")]
    public class ManagerController : Controller
    {
        private DataContext db;
        public ManagerController(DataContext context)
        {
            db = context;
        }

        public async Task<IActionResult> ChangeProduct(int productId,string prodName,string prodPrice,int prodQuantity)
        {
            Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            product.Name = prodName;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ",";
            product.Price= Convert.ToDouble(prodPrice, provider);
            product.Quantity = prodQuantity;
            await db.SaveChangesAsync();
            return RedirectToAction("GoToIndex", "NavBar");
        }
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("GoToIndex", "NavBar");
        }

        public async Task<IActionResult> ChangeOrder(int orderId)
        {
            Order order = await db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            order.IsDelivered = true;
            await db.SaveChangesAsync();
            return RedirectToAction("OrderView", "Order");
        }

        public async Task<IActionResult> AddProduct(
            string name,
            string shape,
            string color,
            string path,
            string price,
            int quantity)
        {
            
            if (name != null && path != null && price != null)
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ",";
                double priceDouble= Convert.ToDouble(price, provider);
                Product product = new Product() {Name=name,Shape=shape,Color=color,Image=path,Price=priceDouble,Quantity=quantity };
                db.Products.Add(product);
                await db.SaveChangesAsync();
            }
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductStore.Services
{
    public class GetProducts:IProductsProvider
    {
        private DataContext db;
        public GetProducts(DataContext context)
        {
            db = context;
        }
        public async Task<List<Product>> GetProductListAsync(string productString)
        {
            string[] prSpltd = productString.Split(',');
            prSpltd = prSpltd.SkipLast(1).ToArray();
            List<Product> cartProducts = new List<Product>();
            foreach (string s in prSpltd)
            {
                string[] spl_s = s.Split('=');
                int productId;
                int productQuantity;
                Int32.TryParse(spl_s[0], out productId);
                Int32.TryParse(spl_s[1], out productQuantity);
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
                product.Quantity = productQuantity;
                cartProducts.Add(product);
            }
            return (cartProducts);
        }
    }
}

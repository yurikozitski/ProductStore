using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductStore.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductStore.Services
{
    public interface IProductsProvider
    {
        public Task<List<Product>> GetProductListAsync(string productString);
    }
}

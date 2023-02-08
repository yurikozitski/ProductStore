using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<Product> ProductsOrdered { get; set; }

    }
}

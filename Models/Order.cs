using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string ProductsOrdered { get; set; }
        public DateTime DateOfOrder { get; set; }
        public bool IsDelivered { get; set; }

    }
}

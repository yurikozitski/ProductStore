using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductStore.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();   
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string ManagerRoleName = "manager";
            string CustomerRoleName = "customer";

            string managerEmail = "manager@gmail.com";
            string managerPassword = "manager";
            string managerName = "Manager";

            // добавляем роли
            Role managerRole = new Role { Id = 1, Name = ManagerRoleName };
            Role customerRole = new Role { Id = 2, Name = CustomerRoleName };
            User managerUser = new User { Id = 1, Name=managerName, Email = managerEmail, Password = managerPassword, RoleId = managerRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { managerRole, customerRole });
            modelBuilder.Entity<User>().HasData(new User[] { managerUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductStore.Models;

namespace ProductStore.Services
{
    public class DataInitializer:IDataInitializer
    {
        private readonly DataContext db;
        public DataInitializer(DataContext context)=>db=context;

        public async Task InitializeData()
        {
            Product redsquare = new Product
            {
                Name = "Red Square",
                Shape = "square",
                Color = "red",
                Price = 19.99,
                Image = @"\Images\Redsquare.png",
                Quantity = 10
            };
            Product bluesquare = new Product
            {
                Name = "Blue Square",
                Shape = "square",
                Color = "blue",
                Price = 14.99,
                Image = @"\Images\1200px-000080_Navy_Blue_Square.png",
                Quantity = 10
            };
            Product yellowsquare = new Product
            {
                Name = "Yellow Square",
                Shape = "square",
                Color = "yellow",
                Price = 20.99,
                Image = @"\Images\1024px-Square_Yellow.png",
                Quantity = 10
            };
            Product bluecircle = new Product
            {
                Name = "Blue Circle",
                Shape = "circle",
                Color = "blue",
                Price = 9.99,
                Image = @"\Images\Ski_trail_rating_symbol_blue_circle.png",
                Quantity = 10
            };
            Product redcircle = new Product
            {
                Name = "Red Circle",
                Shape = "circle",
                Color = "red",
                Price = 30.99,
                Image = @"\Images\Ski_trail_rating_symbol_red_circle.png",
                Quantity = 10
            };
            Product yellowcircle = new Product
            {
                Name = "Yellow Circle",
                Shape = "circle",
                Color = "yellow",
                Price = 27.99,
                Image = @"\Images\yellowcircle.png",
                Quantity = 10
            };
            Product yellowtriangle = new Product
            {
                Name = "Yellow Triangle",
                Shape = "triangle",
                Color = "yellow",
                Price = 29.99,
                Image = @"\Images\yellowtriangle.png",
                Quantity = 10
            };
            Product redtriangle = new Product
            {
                Name = "Red Triangle",
                Shape = "triangle",
                Color = "red",
                Price = 12.99,
                Image = @"\Images\redtriangle.png",
                Quantity = 10
            };
            Product bluetriangle = new Product
            {
                Name = "Blue Triangle",
                Shape = "triangle",
                Color = "blue",
                Price = 18.99,
                Image = @"\Images\bluetriangle.png",
                Quantity = 10
            };

            await db.Products.AddRangeAsync(
                redsquare,
                bluecircle,
                yellowtriangle,
                bluetriangle,
                yellowsquare,
                redtriangle,
                bluesquare,
                redcircle,
                yellowcircle);

            await db.SaveChangesAsync();
        }

    }
}

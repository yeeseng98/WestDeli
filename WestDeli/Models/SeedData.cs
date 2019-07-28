using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WestDeli.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new WestDeliContext(serviceProvider.GetRequiredService<DbContextOptions<WestDeliContext>>()))
            {                 
                if (context.Dish.Any()) {
                    return;   // DB has been seeded                 
                } 

                context.Dish.AddRange(
                    new Dish {
                        DishName = "Carrot Orange Soup",
                        Category = "Soup",
                        Description = "Asteraceae",
                        Price = 12,
                        PrepTime = 12
                    },

                    new Dish
                    {
                        DishName = "Grilled Beef Brisket",
                        Category = "Main",
                        Description = "Opus Irium Ipsum",
                        Price = 10,
                        PrepTime = 15
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}

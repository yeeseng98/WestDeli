using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WestDeli.Models
{
    public class WestDeliContext : DbContext
    {
        public WestDeliContext (DbContextOptions<WestDeliContext> options)
            : base(options)
        {
        }

        public DbSet<WestDeli.Models.Dish> Dish { get; set; }
    }
}

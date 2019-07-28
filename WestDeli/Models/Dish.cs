using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestDeli.Models
{
    public class Dish
    {
        public int ID { get; set; }
        public string DishName { get; set; }
        public int Price { get; set; }
        public int PrepTime { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}

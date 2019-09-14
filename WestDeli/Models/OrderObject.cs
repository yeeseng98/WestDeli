using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WestDeli.Models
{
    public class OrderObject
    {

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Identifier")]
        public string Identifier { get; set; }

        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Dish name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name Should be minimum 3 characters and a maximum of 100 characters")]
        [DataType(DataType.Text)]
        public string DishName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 999, ErrorMessage = "This field must be a number")]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Preparation time is required")]
        [Range(1, 999, ErrorMessage = "This field must be a number")]
        [Display(Name = "Preparation Time")]
        public int PrepTime { get; set; }

        [Required(ErrorMessage = "Dish category is required")]
        [Display(Name = "Dish Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Dish portion is required")]
        public int Portion { get; set; }
    }
}

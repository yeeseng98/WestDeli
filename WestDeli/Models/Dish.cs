using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace WestDeli.Models
{
    public class Dish
    {
        [Required]
        [Display(Name= "ID")]
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

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string ImgLink { get; set; }
    }
}

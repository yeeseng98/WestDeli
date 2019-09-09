using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace WestDeli.Models
{
    public class UserEntity: TableEntity
    {
        public UserEntity(string username, string password)
        {
            this.PartitionKey = username;
            this.RowKey = password;
        }

        public UserEntity() { }

        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^(?!.*[-_]{2,})(?=^[^-_].*[^-_]$)[\w\s-]{3,9}$", ErrorMessage = "Invalid Username, minimum 6 characters!")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?!.*[-_]{2,})(?=^[^-_].*[^-_]$)[\w\s-]{3,9}$", ErrorMessage = "Invalid Password, minimum 6 characters!")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Invalid Email!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string RegisterDate { get; set; }

        [Required(ErrorMessage = "Credit Card Number is required")]
        [RegularExpression(@"^4[0-9]{12}(?:[0-9]{3})?$", ErrorMessage = "Invalid Credit Card Number!")]
        [Display(Name = "Credit Card Number (VISA)")]
        public string CreditNum { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 99, ErrorMessage = "This field must be a valid age number!")]
        [Display(Name = "Age")]
        public string Age { get; set; }

        [Required(ErrorMessage = "IC Number is required")]
        [RegularExpression(@"(([[1-9]{2})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01]))-([0-9]{2})-([0-9]{4})", ErrorMessage = "Invalid Malaysian IC Number!")]
        [Display(Name = "IC Number")]
        public string IdentityNumber { get; set; }

        public string Role { get; set; }

        public int AccessLevel { get; set; }

        public int HasPurchase { get; set; }

        public int HasPending { get; set; }

        public string LastLogin { get; set; }

    }
}

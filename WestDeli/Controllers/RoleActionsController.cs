using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WestDeli.Models;

namespace WestDeli.Controllers
{
    public class RoleActionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ViewBag.isValid = true;

            return View();
        }

        public IActionResult Create()
        {
            List<String> vm = new List<String>();

            vm.Add("Male");
            vm.Add("Female");
            vm.Add("Other");

            ViewBag.genderList = vm;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username, Password, LastName, FirstName, Email, CreditNum, Gender, Age, IdentityNumber")] UserEntity user)
        {

            if (ModelState.IsValid)
            {
                UserEntity newUser = new UserEntity();

                newUser.Username = user.Username;
                newUser.Password = user.Password;
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Email = user.Email;
                newUser.CreditNum = user.CreditNum;
                newUser.Gender = user.Gender;
                newUser.Age = user.Age;
                newUser.IdentityNumber = user.IdentityNumber;

                TablesController table = new TablesController();

                table.AddUser(newUser);

                return RedirectToAction("Index", "Dishes");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Password")] LoginCredential user)
        {

            if (ModelState.IsValid)
            {
                TablesController table = new TablesController();

                if (table.GetUser(user.Username, user.Password)) {
                    return RedirectToAction("Index", "Dishes");
                }
                else
                {
                    ViewBag.isValid = false;
                }
            }
            return View();
        }
    }
}
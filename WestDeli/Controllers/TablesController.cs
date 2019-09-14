using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using WestDeli.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using WestDeli.Helpers;

namespace WestDeli.Controllers
{
    public class TablesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //method to get the access key
        public CloudStorageAccount addconnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            CloudStorageAccount storageaccount =
                CloudStorageAccount.Parse(configuration["ConnectionStrings:delitablestorage_AzureStorageConnectionString"]);
            return storageaccount;
        }

        public ActionResult AddUser(UserEntity user)
        {
            UserEntity newUser = new UserEntity(user.Username, user.Password);
            newUser.Username = user.Username;
            newUser.Password = user.Password;
            newUser.Email = user.Email;
            newUser.CreditNum = user.CreditNum;
            newUser.Gender = user.Gender;
            newUser.RegisterDate = DateTime.Now.ToString();
            newUser.FirstName = user.FirstName;
            newUser.LastName = user.LastName;
            newUser.Age = user.Age;
            newUser.IdentityNumber = user.IdentityNumber;
            newUser.Role = "Customer";
            newUser.AccessLevel = 1;
            newUser.HasPending = 0;
            newUser.HasPurchase = 0;
            newUser.LastLogin = null;

            CloudStorageAccount storage = addconnection();
            //refer which table/ create which table
            CloudTableClient tableclient = storage.CreateCloudTableClient();
            CloudTable table = tableclient.GetTableReference("UserTable");

            try
            {
                TableOperation insertoperation = TableOperation.Insert(newUser);
                TableResult result = table.ExecuteAsync(insertoperation).Result;
                ViewBag.TableName = table.Name;
                ViewBag.result = result.HttpStatusCode; //status of your process = 204
            }
            catch (Exception ex)
            {
                ViewBag.TableName = table.Name;
                ViewBag.result = 101;
            }
            return View();

        }

        public ActionResult UpdateLastLogin(string partitionKey, string rowKey)
        {
            CloudStorageAccount storage = addconnection();
            //refer which table/ create which table
            CloudTableClient tableclient = storage.CreateCloudTableClient();
            CloudTable table = tableclient.GetTableReference("UserTable");

            TableOperation retrieve = TableOperation.Retrieve<UserEntity>(partitionKey, rowKey);

            TableResult result = table.ExecuteAsync(retrieve).Result;

            UserEntity user = (UserEntity)result.Result;

            user.LastLogin = DateTime.Now.ToString();

            if (result != null)
            {
                TableOperation update = TableOperation.Replace(user);

                table.ExecuteAsync(update);
            }
            return View();
        }

        public Boolean GetUser(string username, string password)
        {
            CloudStorageAccount storage = addconnection();
            //refer which table/ create which table
            CloudTableClient tableclient = storage.CreateCloudTableClient();
            CloudTable table = tableclient.GetTableReference("UserTable");

            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<UserEntity>(username, password);
                TableResult retrievedResult = table.ExecuteAsync(retrieveOperation).Result;

                UserEntity entity = (UserEntity)retrievedResult.Result;

                if (entity != null)
                {
                    HttpHelper.HttpContext.Session.SetString("currentUser", entity.Username);
                    HttpHelper.HttpContext.Session.SetString("role", entity.Role);
                    HttpHelper.HttpContext.Session.SetString("identifier", entity.Password);

                    ViewBag.TableName = table.Name;
                    ViewBag.result = retrievedResult.HttpStatusCode; //status of your process = 204
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ViewBag.TableName = table.Name;
                ViewBag.result = 101;
            }
            return false;
        }
    }

}
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

        ////create table using the code
        //public ActionResult CreateTable()
        //{
        //    CloudStorageAccount storage = addconnection();
        //    //refer which table/ create which table
        //    CloudTableClient tableclient = storage.CreateCloudTableClient();
        //    CloudTable table = tableclient.GetTableReference("TestTable");
        //    ViewBag.result = table.CreateIfNotExistsAsync().Result;
        //    ViewBag.TableName = table.Name;

        //    return View();
        //}

        //method to insert the single entities to the table storage
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

        ////method to insert multiple data in one time
        //public ActionResult AddMultiEntity()
        //{
        //    CloudStorageAccount storage = addconnection();
        //    //refer which table/ create which table
        //    CloudTableClient tableclient = storage.CreateCloudTableClient();
        //    CloudTable table = tableclient.GetTableReference("TestTable");

        //    UserEntity customer1 = new UserEntity("Smith", "Jeff");
        //    customer1.email = "Jeff@gmail.com";

        //    UserEntity customer2 = new UserEntity("Smith", "Ben");
        //    customer2.email = "ben@gmail.com";

        //    try
        //    {
        //        IList<TableResult> results;

        //        //run the multiple process using batch operation
        //        TableBatchOperation batch = new TableBatchOperation();
        //        batch.Insert(customer1);
        //        batch.Insert(customer2);

        //        results = table.ExecuteBatchAsync(batch).Result;
        //        return View(results);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.result = 101;
        //        ViewBag.Message = "Error: Unable to insert all data";
        //        return View();
        //    }
        //}

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
                    ViewBag.TableName = table.Name;
                    ViewBag.result = retrievedResult.HttpStatusCode; //status of your process = 204
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ViewBag.TableName = table.Name;
                ViewBag.result = 101;
            }
            return false;
        }
    }

}
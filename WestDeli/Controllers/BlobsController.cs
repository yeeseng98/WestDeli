using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Http;

namespace WestDeli.Controllers
{
    public class BlobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //this method used to add the container information
        private CloudBlobContainer GetCloudBlobContainer()
        {
            //1. access the appsetting.json to get the access information
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration Configuration = builder.Build();

            //2. access the appsetting.json to get the access key
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(Configuration["ConnectionStrings:westdelistorage_AzureStorageConnectionString"]);

            //3. start to create an client object to connect the account
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //4. provide container name that the client need to access
            CloudBlobContainer container = blobClient.GetContainerReference("image-blob-container");

            return container;
        }

        //public ActionResult CreateBlobContainer()
        //{
        //    //1.1 call back the above getcloudblobcontainer() method
        //    CloudBlobContainer container = GetCloudBlobContainer();

        //    //1.2 start create a new container if the container not yet exist
        //    //store result under ViewBag.Success variable
        //    ViewBag.Success = container.CreateIfNotExistsAsync().Result; //true/false

        //    //1.3 get the container name so that to display in the webpage
        //    ViewBag.BlobContainerName = container.Name;

        //    //1.4 return to the page and see the final result
        //    return View();
        //}

        //action 2: upload the file to the blob storage
        public string UploadBlob(IFormFile formFile, string path)
        {
            //2.1. call back the above getcloudblobcontainer() method
            CloudBlobContainer container = GetCloudBlobContainer();

            //2.2 mention what is the name should be store for the file
            CloudBlockBlob blob = container.GetBlockBlobReference(formFile.FileName);

            //2.3 state the location of the sourcefile
            //read and send the file to the blob storage
            using (Stream stream = formFile.OpenReadStream())
            {
                stream.Position = 0;
                //cope the content to the blob name blob.jpg
                blob.UploadFromStreamAsync(stream).Wait();
            }
            return "Already send the file to blob storage!";
        }

        public ActionResult ListBlobs()
        {
            CloudBlobContainer container = GetCloudBlobContainer();

            //make a list object to store list result from blob storage.
            List<string> blobs = new List<string>();

            //read the result from blob storage
            BlobResultSegment result = container.ListBlobsSegmentedAsync(null).Result;

            foreach (IListBlobItem item in result.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    blobs.Add(blob.Name + "#" + blob.Uri.ToString());
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(blob.Name + "#" + blob.Uri.ToString());
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    blobs.Add(dir.Uri.ToString());
                }
            }

            return View(blobs);
        }

        public string DownloadBlob(string item)
        {
            try
            {
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(item);

                using (var fileStream = System.IO.File.OpenWrite(@"C:\Users\Acer\Desktop\" + item))
                {
                    blob.DownloadToStreamAsync(fileStream).Wait();
                }
                return "Success!";
            }
            catch (Exception ex)
            {
                return "Failure!" + ex;
            }
        }

        public string DeleteBlob(string item)
        {
            try
            {
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(item);

                blob.DeleteAsync().Wait();
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failure!" + ex;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration Configuration = builder.Build();

            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(Configuration["ConnectionStrings:westdelistorage_AzureStorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

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

        public string UploadBlob(IFormFile formFile, string path)
        {
            CloudBlobContainer container = GetCloudBlobContainer();

            CloudBlockBlob blob = container.GetBlockBlobReference(formFile.FileName);

            using (Stream stream = formFile.OpenReadStream())
            {
                stream.Position = 0;
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

        //public string DownloadBlob(string item)
        //{
        //    try
        //    {
        //        CloudBlobContainer container = GetCloudBlobContainer();
        //        CloudBlockBlob blob = container.GetBlockBlobReference(item);

        //        using (var fileStream = System.IO.File.OpenWrite(@"C:\Users\Acer\Desktop\" + item))
        //        {
        //            blob.DownloadToStreamAsync(fileStream).Wait();
        //        }
        //        return "Success!";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Failure!" + ex;
        //    }
        //}

        public void DeleteBlob(string item)
        {
            try
            {
                CloudBlobContainer container = GetCloudBlobContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(item);

                blob.DeleteAsync().Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
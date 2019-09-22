using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WestDeli.Helpers;
using WestDeli.Models;

namespace WestDeli.Views.Transactions
{
    public class TransactionsController : Controller
    {

        public async Task<ActionResult> Index()
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            var items = await TransactRepository<Transaction>.GetItemsAsync(d => d.Status == "PENDING");

            return View(items);
        }

        public async Task<ActionResult> History()
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            var items = await TransactRepository<Transaction>.GetItemsAsync(d => d.Status == "COMPLETE");
            return View(items);
        }

        // GET: Transactions/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (id == null) { return BadRequest(); }

            Transaction item = await TransactRepository<Transaction>.GetItemAsync(id);

            if (item == null) { return NotFound(); }

            return View(item);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Transactions/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null) { return BadRequest(); }

            Transaction item = await TransactRepository<Transaction>.GetItemAsync(id);

            if (item == null) { return NotFound(); }

            return View(item);
        }


        // GET: Transactions/Delete/5
        public async Task<ActionResult> Verify(string id)
        {
            Transaction item = await TransactRepository<Transaction>.GetItemAsync(id);

            item.Status = "COMPLETE";

            await TransactRepository<Transaction>.UpdateItemAsync(id, item);
            return RedirectToAction(nameof(Index));
        }

        // GET: Transactions/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            await TransactRepository<Transaction>.DeleteItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
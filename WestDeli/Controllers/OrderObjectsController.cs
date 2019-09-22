using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polly;
using WestDeli.Helpers;
using WestDeli.Models;

namespace WestDeli.Views
{
    public class OrderObjectsController : Controller
    {
        private readonly WestDeliContext _context;

        public OrderObjectsController(WestDeliContext context)
        {
            _context = context;
        }

        // GET: OrderObjects
        public async Task<IActionResult> Index()
        {
            ViewBag.hasPending = false;

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
                ViewBag.identifier = HttpHelper.HttpContext.Session.GetString("identifier");

                String user = HttpHelper.HttpContext.Session.GetString("currentUser");
                String identifier = HttpHelper.HttpContext.Session.GetString("identifier");

                var items = await TransactRepository<Transaction>.GetItemsAsync(d => d.Username == user && d.Identifier == identifier && d.Status == "PENDING");

                if (items.Count() > 0)
                {
                    ViewBag.hasPending = true;
                    foreach (var i in items)
                    {
                        ViewBag.currentOrder = i.TransactDate;
                        ViewBag.user = i.Username;
                        ViewBag.price = i.TotalPrice;
                        ViewBag.time = i.TotalTime;
                    }
                }

                return View(await _context.OrderObject.Where(s => s.Username == user).Where(s => s.Identifier == identifier).ToListAsync());
            }

            return View();
        }

        // GET: OrderObjects
        public async Task<IActionResult> Finalize(int tprice, int tprepTime)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            ViewBag.tprice = tprice;

            ViewBag.tprepTime = tprepTime;

            return View();
        }

        // GET: OrderObjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var orderObject = await _context.OrderObject
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderObject == null)
            {
                return NotFound();
            }

            return View(orderObject);
        }

        // GET: OrderObjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Identifier,ID,DishName,Price,PrepTime,Category,Portion")] OrderObject orderObject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderObject);
        }

        [HttpPost]
        public async Task<IActionResult> addOrder(OrderObject orderObject)
        {
            var breakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10));
            _context.Add(orderObject);

            await breakerPolicy.ExecuteAsync(async () =>
            { 
                await _context.SaveChangesAsync();
            });

            return View();
        }

        // GET: OrderObjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (id == null)
            {
                return NotFound();
            }

            var orderObject = await _context.OrderObject.FindAsync(id);
            if (orderObject == null)
            {
                return NotFound();
            }
            return View(orderObject);
        }

        // POST: OrderObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Username,Identifier,ID,DishName,Price,PrepTime,Category,Portion")] OrderObject orderObject)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (id != orderObject.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderObjectExists(orderObject.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orderObject);
        }

        // GET: OrderObjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (id == null)
            {
                return NotFound();
            }

            var orderObject = await _context.OrderObject
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderObject == null)
            {
                return NotFound();
            }

            return View(orderObject);
        }

        // POST: OrderObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderObject = await _context.OrderObject.FindAsync(id);
            _context.OrderObject.Remove(orderObject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderObjectExists(int id)
        {
            return _context.OrderObject.Any(e => e.ID == id);
        }

        [ActionName("submitOrder")]
        public async Task<IActionResult> submitOrder(string tprice, int tprepTime, string address)
        {
            string username = HttpHelper.HttpContext.Session.GetString("currentUser");
            string identifier = HttpHelper.HttpContext.Session.GetString("identifier");

            Transaction newTrans = new Transaction();

            newTrans.ID = username + identifier + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss");
            newTrans.Username = username;
            newTrans.Identifier = identifier;
            newTrans.TotalPrice = tprice;
            newTrans.TotalTime = tprepTime;
            newTrans.TransactDate = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss");
            newTrans.Address = address;
            newTrans.Status = "PENDING";
            newTrans.items = _context.OrderObject.Where(s => s.Username == username).Where(s => s.Identifier == identifier).ToArray();

            await TransactRepository<Transaction>.CreateItemAsync(newTrans);

            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
                ViewBag.identifier = HttpHelper.HttpContext.Session.GetString("identifier");

                String user = HttpHelper.HttpContext.Session.GetString("currentUser");
                String identifier2 = HttpHelper.HttpContext.Session.GetString("identifier");

                OrderObject[] DeleteList = _context.OrderObject.Where(s => s.Username == user).Where(s => s.Identifier == identifier).ToArray();

                foreach( OrderObject i in DeleteList)
                {
                    var Obj = await _context.OrderObject.FindAsync(i.ID);
                    _context.OrderObject.Remove(Obj);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index", "Dishes");
        }
        
    }
}

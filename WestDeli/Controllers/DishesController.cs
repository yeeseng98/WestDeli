using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WestDeli.Controllers;
using WestDeli.Models;
using System.Diagnostics;
using WestDeli.Helpers;
using WestDeli.Filters;

namespace WestDeli.Views.Dishes
{
    public class DishesController : Controller
    {

        private string role = "";

        private readonly WestDeliContext _context;

        public DishesController(WestDeliContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(string category)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (!String.IsNullOrEmpty(category))
            {

                return View(await _context.Dish.Where(b => b.Category == category).ToListAsync());
            }

            return View(await _context.Dish.ToListAsync());
        }

        public async Task<ActionResult> Filter(string ccategory)
        {
            return View("Index", new { category = ccategory });
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                return NotFound();
            }

            ViewBag.portionCount = 1;

            ViewBag.hasPending = false;


            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                string user = HttpHelper.HttpContext.Session.GetString("currentUser");
                string identifier = HttpHelper.HttpContext.Session.GetString("identifier");
                var pendingOrder = await TransactRepository<Transaction>.GetItemsAsync(d => d.Username == user && d.Identifier == identifier && d.Status == "PENDING");

                if (pendingOrder.Count() > 0)
                {
                    ViewBag.hasPending = true;
                }
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DishName,Price,PrepTime,Category,Description")] Dish dish)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
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

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DishName,Price,PrepTime,Category,Description, ImgLink")] Dish dish)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            if (id != dish.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.ID))
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
            return View(dish);
        }

        // GET: Dishes/Delete/5
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

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

            
            var dish = await _context.Dish.FindAsync(id);

            string imgRef = dish.ImgLink.Replace("https://westdelistorage.blob.core.windows.net/image-blob-container/", "");
            BlobsController bc = new BlobsController();
            bc.DeleteBlob(imgRef);

            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.ID == id);
        }

        [HttpPost, ActionName("Upload")]
        public async Task<IActionResult> Post([Bind("ID,DishName,Price,PrepTime,Category,Description")] Dish dish, List<IFormFile> files)
        {
            long sizes = files.Sum(f => f.Length);

            var filepath = Path.GetTempFileName();

            foreach (var FormFile in files)
            {
                if (!FormFile.ContentType.ToLower().StartsWith("image"))
                {
                    return BadRequest("The " + Path.GetFileName(filepath) +
                        " is not a jpg file! Please re-upload a correct file!");
                }
                else if (FormFile.Length <= 0)
                {
                    return BadRequest("The " + Path.GetFileName(filepath) +
                        " is EMPTY! Please re-upload non empty file!");
                }
                else if (FormFile.Length > 1048576) //>1MB
                {
                    return BadRequest("The " + Path.GetFileName(filepath) +
                        " is more than 1MB! Please re-upload a file less than 1MB!");
                }
                else
                {
                    BlobsController bc = new BlobsController();
                    bc.UploadBlob(FormFile, Path.GetTempFileName());

                    if (ModelState.IsValid)
                    {
                        dish.ImgLink = "https://westdelistorage.blob.core.windows.net/image-blob-container/" + FormFile.FileName;
                        _context.Add(dish);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(dish);
                }
            }

            return View(dish);
        }

        [HttpPost, ActionName("addToCart")]
        [Throttle(Name = "ThrottleTest", Seconds = 3)]
        public async Task<IActionResult> addToCart(string Id, string DishName, int Price, int PrepTime, string Category, int portion)
        {
            ViewBag.hasPending = false;

            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                String username = HttpHelper.HttpContext.Session.GetString("currentUser");
                String identifier = HttpHelper.HttpContext.Session.GetString("identifier");
                OrderObject odrObj = new OrderObject();
                odrObj.DishName = DishName;
                odrObj.Category = Category;
                odrObj.Price = Price;
                odrObj.Portion = portion;
                odrObj.PrepTime = PrepTime;
                odrObj.Username = username;
                odrObj.Identifier = identifier;

                OrderObjectsController OControls = new OrderObjectsController(_context);
                await OControls.addOrder(odrObj);

                return RedirectToAction(nameof(Index));

            }

            return View();
        }
    }
}

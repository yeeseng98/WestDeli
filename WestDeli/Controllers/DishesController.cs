﻿using System;
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

namespace WestDeli.Views.Dishes
{
    public class DishesController : Controller
    {
        private readonly WestDeliContext _context;

        public DishesController(WestDeliContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dish.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,DishName,Price,PrepTime,Category,Description")] Dish dish)
        {
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.ID == id);
        }

        //add a new post method for uploading the file to the server
        [HttpPost, ActionName("Upload")]
        public async Task<IActionResult> Post([Bind("ID,DishName,Price,PrepTime,Category,Description")] Dish dish, List<IFormFile> files)
        {
            //get the total file sizes from the file browser
            long sizes = files.Sum(f => f.Length);

            //get the temporary file path
            var filepath = Path.GetTempFileName();

            int i = 1; string contents = "";
            //to read file by file
            foreach (var FormFile in files)
            {
                //do the file validation (optional)
                //step 1: check for file content type
                //if i only allowed for text file
                if (!FormFile.ContentType.ToLower().StartsWith("image"))
                {
                    return BadRequest("The " + Path.GetFileName(filepath) +
                        " is not a jpg file! Please re-upload a correct file!");
                }
                //step 2: check whether the file is empty or not
                else if (FormFile.Length <= 0)
                {
                    return BadRequest("The " + Path.GetFileName(filepath) +
                        " is EMPTY! Please re-upload non empty file!");
                }
                //step 3: check whether the file is over the size limit
                else if (FormFile.Length > 1048576) //>1MB
                {
                    return BadRequest("The " + Path.GetFileName(filepath) +
                        " is more than 1MB! Please re-upload a file less than 1MB!");
                }
                //step 4: start to transfer the file / read the file contents
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
    }
}

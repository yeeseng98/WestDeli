﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
                ViewBag.identifier = HttpHelper.HttpContext.Session.GetString("identifier");

                String user = HttpHelper.HttpContext.Session.GetString("currentUser");
                String identifier = HttpHelper.HttpContext.Session.GetString("identifier");

                return View(await _context.OrderObject.Where(s => s.Username == user).Where(s => s.Identifier == identifier).ToListAsync());

            }

            return View();
        }

        // GET: OrderObjects
        public async Task<IActionResult> Finalize()
        {
            if (HttpHelper.HttpContext.Session.GetString("currentUser") != null)
            {
                ViewBag.user = HttpHelper.HttpContext.Session.GetString("currentUser");
            }

            if (HttpHelper.HttpContext.Session.GetString("role") != null)
            {
                ViewBag.role = HttpHelper.HttpContext.Session.GetString("role");
            }

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
            _context.Add(orderObject);
            await _context.SaveChangesAsync();
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
    }
}
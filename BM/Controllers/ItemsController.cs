﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BM.Models;

namespace BM.Controllers
{
    public class ItemsController : Controller
    {
        private readonly BIMSContext _context;

        public ItemsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.Items.Include(i => i.ItemCategory).Include(i => i.User);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "Name");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //public async Task<IActionResult> Create([Bind("Id,Name,UserId,ItemCategoryId,IsAvailable,IsActive,IsDeleted")] Item item)
        //{
        //    int userId = (int)HttpContext.Session.GetInt32("UserId");
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(item);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "Name", item.ItemCategoryId);
        //    //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", item.UserId);
        //    return View(item);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId,ItemCategoryId,IsAvailable,IsActive,IsDeleted")] Item item)
        {
            
            int? sessionUserId = HttpContext.Session.GetInt32("UserId");

           
            if (!sessionUserId.HasValue)
            {
               
                return RedirectToAction("Login", "Account"); 
            }

           
            item.UserId = sessionUserId.Value; 
            
            var itemCategoryExists = await _context.ItemCategories.AnyAsync(ic => ic.Id == item.ItemCategoryId);
            if (!itemCategoryExists)
            {
                
                ModelState.AddModelError("ItemCategoryId", "The selected item category does not exist.");
                ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "Name", item.ItemCategoryId);
                return View(item);
            }

            if (ModelState.IsValid)
            {
               
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "Name", item.ItemCategoryId);
            // ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", item.UserId); // Optionally handle UserId dropdown
            return View(item);
        }
       
        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "Name", item.ItemCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", item.UserId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId,ItemCategoryId,IsAvailable,IsActive,IsDeleted")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "Name", item.ItemCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", item.UserId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}

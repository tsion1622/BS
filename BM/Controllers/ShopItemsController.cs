using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BM.Models;

namespace BM.Controllers
{
    public class ShopItemsController : Controller
    {
        private readonly BIMSContext _context;

        public ShopItemsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: ShopItems
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.ShopItems.Include(s => s.Item).Include(s => s.Shop);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: ShopItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems
                .Include(s => s.Item)
                .Include(s => s.Shop)
                .Include(x => x.ItemEntryVarations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // GET: ShopItems/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name");
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Description");
            return View();
        }

        // POST: ShopItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,ShopId,Balance,IsActive,IsDeleted")] ShopItem shopItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", shopItem.ItemId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Description", shopItem.ShopId);
            return View(shopItem);
        }

        // GET: ShopItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems.FindAsync(id);
            if (shopItem == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", shopItem.ItemId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Description", shopItem.ShopId);
            return View(shopItem);
        }

        // POST: ShopItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,ShopId,Balance,IsActive,IsDeleted")] ShopItem shopItem)
        {
            if (id != shopItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopItemExists(shopItem.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", shopItem.ItemId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Description", shopItem.ShopId);
            return View(shopItem);
        }

        // GET: ShopItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItems
                .Include(s => s.Item)
                .Include(s => s.Shop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // POST: ShopItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopItem = await _context.ShopItems.FindAsync(id);
            if (shopItem != null)
            {
                _context.ShopItems.Remove(shopItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopItemExists(int id)
        {
            return _context.ShopItems.Any(e => e.Id == id);
        }
    }
}

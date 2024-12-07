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
    public class ItemEntriesController : Controller
    {
        private readonly BIMSContext _context;

        public ItemEntriesController(BIMSContext context)
        {
            _context = context;
        }

        // GET: ItemEntries
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.ItemEntries.Include(i => i.ShopItem);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: ItemEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemEntry = await _context.ItemEntries
                .Include(i => i.ShopItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemEntry == null)
            {
                return NotFound();
            }

            return View(itemEntry);
        }

        // GET: ItemEntries/Create
        public IActionResult Create()
        {
            ViewData["ShopItemId"] = new SelectList(_context.ShopItems, "Id", "Balance");
            return View();
        }

        // POST: ItemEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShopItemId,EntryDate,Quantity,WithdrawQuantity,Price,IsActive,IsDeleted")] ItemEntry itemEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShopItemId"] = new SelectList(_context.ShopItems, "Id", "Balance", itemEntry.ShopItemId);
            return View(itemEntry);
        }

        // GET: ItemEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemEntry = await _context.ItemEntries.FindAsync(id);
            if (itemEntry == null)
            {
                return NotFound();
            }
            ViewData["ShopItemId"] = new SelectList(_context.ShopItems, "Id", "Balance", itemEntry.ShopItemId);
            return View(itemEntry);
        }

        // POST: ItemEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShopItemId,EntryDate,Quantity,WithdrawQuantity,Price,IsActive,IsDeleted")] ItemEntry itemEntry)
        {
            if (id != itemEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemEntryExists(itemEntry.Id))
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
            ViewData["ShopItemId"] = new SelectList(_context.ShopItems, "Id", "Balance", itemEntry.ShopItemId);
            return View(itemEntry);
        }

        // GET: ItemEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemEntry = await _context.ItemEntries
                .Include(i => i.ShopItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemEntry == null)
            {
                return NotFound();
            }

            return View(itemEntry);
        }

        // POST: ItemEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemEntry = await _context.ItemEntries.FindAsync(id);
            if (itemEntry != null)
            {
                _context.ItemEntries.Remove(itemEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemEntryExists(int id)
        {
            return _context.ItemEntries.Any(e => e.Id == id);
        }
    }
}

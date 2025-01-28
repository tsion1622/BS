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
    public class ItemEntryVarationsController : Controller
    {
        private readonly BIMSContext _context;

        public ItemEntryVarationsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: ItemEntryVarations
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.ItemEntryVarations.Include(i => i.ItemEntry).Include(i => i.Varation).Include(i => i.VarationType);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: ItemEntryVarations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemEntryVaration = await _context.ItemEntryVarations
                .Include(i => i.ItemEntry)
                .Include(i => i.Varation)
                .Include(i => i.VarationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemEntryVaration == null)
            {
                return NotFound();
            }

            return View(itemEntryVaration);
        }

        // GET: ItemEntryVarations/Create
        public IActionResult Create()
        {
            ViewData["ItemEntryId"] = new SelectList(_context.ItemEntries, "Id", "Id");
            ViewData["VarationId"] = new SelectList(_context.Varations, "Id", "Name");
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name");
            return View();
        }

        // POST: ItemEntryVarations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VarationTypeId,VarationId,ItemEntryId,Quantity,Price,IsActive,IsDeleted")] ItemEntryVaration itemEntryVaration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemEntryVaration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemEntryId"] = new SelectList(_context.ItemEntries, "Id", "Id", itemEntryVaration.ItemEntryId);
            ViewData["VarationId"] = new SelectList(_context.Varations, "Id", "Name", itemEntryVaration.VarationId);
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name", itemEntryVaration.VarationTypeId);
            return View(itemEntryVaration);
        }

        // GET: ItemEntryVarations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemEntryVaration = await _context.ItemEntryVarations.FindAsync(id);
            if (itemEntryVaration == null)
            {
                return NotFound();
            }
            ViewData["ItemEntryId"] = new SelectList(_context.ItemEntries, "Id", "Id", itemEntryVaration.ItemEntryId);
            ViewData["VarationId"] = new SelectList(_context.Varations, "Id", "Name", itemEntryVaration.VarationId);
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name", itemEntryVaration.VarationTypeId);
            return View(itemEntryVaration);
        }

        // POST: ItemEntryVarations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VarationTypeId,VarationId,ItemEntryId,Quantity,Price,IsActive,IsDeleted")] ItemEntryVaration itemEntryVaration)
        {
            if (id != itemEntryVaration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemEntryVaration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemEntryVarationExists(itemEntryVaration.Id))
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
            ViewData["ItemEntryId"] = new SelectList(_context.ItemEntries, "Id", "Id", itemEntryVaration.ItemEntryId);
            ViewData["VarationId"] = new SelectList(_context.Varations, "Id", "Name", itemEntryVaration.VarationId);
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name", itemEntryVaration.VarationTypeId);
            return View(itemEntryVaration);
        }

        // GET: ItemEntryVarations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemEntryVaration = await _context.ItemEntryVarations
                .Include(i => i.ItemEntry)
                .Include(i => i.Varation)
                .Include(i => i.VarationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemEntryVaration == null)
            {
                return NotFound();
            }

            return View(itemEntryVaration);
        }

        // POST: ItemEntryVarations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemEntryVaration = await _context.ItemEntryVarations.FindAsync(id);
            if (itemEntryVaration != null)
            {
                _context.ItemEntryVarations.Remove(itemEntryVaration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemEntryVarationExists(int id)
        {
            return _context.ItemEntryVarations.Any(e => e.Id == id);
        }
    }
}

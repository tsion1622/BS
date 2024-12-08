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
    public class VarationTypesController : Controller
    {
        private readonly BIMSContext _context;

        public VarationTypesController(BIMSContext context)
        {
            _context = context;
        }

        // GET: VarationTypes
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.VarationTypes.Include(v => v.Item);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: VarationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varationType = await _context.VarationTypes
                .Include(v => v.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varationType == null)
            {
                return NotFound();
            }

            return View(varationType);
        }

        // GET: VarationTypes/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name");
            return View();
        }

        // POST: VarationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ItemId,IsActive,IsDeleted")] VarationType varationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(varationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", varationType.ItemId);
            return View(varationType);
        }

        // GET: VarationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varationType = await _context.VarationTypes.FindAsync(id);
            if (varationType == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", varationType.ItemId);
            return View(varationType);
        }

        // POST: VarationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ItemId,IsActive,IsDeleted")] VarationType varationType)
        {
            if (id != varationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(varationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VarationTypeExists(varationType.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", varationType.ItemId);
            return View(varationType);
        }

        // GET: VarationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varationType = await _context.VarationTypes
                .Include(v => v.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varationType == null)
            {
                return NotFound();
            }

            return View(varationType);
        }

        // POST: VarationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var varationType = await _context.VarationTypes.FindAsync(id);
            if (varationType != null)
            {
                _context.VarationTypes.Remove(varationType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VarationTypeExists(int id)
        {
            return _context.VarationTypes.Any(e => e.Id == id);
        }
    }
}

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
    public class VarationsController : Controller
    {
        private readonly BIMSContext _context;

        public VarationsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: Varations
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.Varations.Include(v => v.VarationType);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: Varations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varation = await _context.Varations
                .Include(v => v.VarationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varation == null)
            {
                return NotFound();
            }

            return View(varation);
        }

        // GET: Varations/Create
        public IActionResult Create()
        {
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name");
            return View();
        }

        // POST: Varations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,VarationTypeId,IsActive,IsDeleted")] Varation varation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(varation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name", varation.VarationTypeId);
            return View(varation);
        }

        // GET: Varations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varation = await _context.Varations.FindAsync(id);
            if (varation == null)
            {
                return NotFound();
            }
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name", varation.VarationTypeId);
            return View(varation);
        }

        // POST: Varations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,VarationTypeId,IsActive,IsDeleted")] Varation varation)
        {
            if (id != varation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(varation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VarationExists(varation.Id))
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
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name", varation.VarationTypeId);
            return View(varation);
        }

        // GET: Varations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varation = await _context.Varations
                .Include(v => v.VarationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varation == null)
            {
                return NotFound();
            }

            return View(varation);
        }

        // POST: Varations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var varation = await _context.Varations.FindAsync(id);
            if (varation != null)
            {
                _context.Varations.Remove(varation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VarationExists(int id)
        {
            return _context.Varations.Any(e => e.Id == id);
        }
    }
}

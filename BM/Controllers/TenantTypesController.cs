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
    public class TenantTypesController : Controller
    {
        private readonly BIMSContext _context;

        public TenantTypesController(BIMSContext context)
        {
            _context = context;
        }

        // GET: TenantTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TenantTypes.ToListAsync());
        }

        // GET: TenantTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenantType = await _context.TenantTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tenantType == null)
            {
                return NotFound();
            }

            return View(tenantType);
        }

        // GET: TenantTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TenantTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive,IsDeleted")] TenantType tenantType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tenantType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tenantType);
        }

        // GET: TenantTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenantType = await _context.TenantTypes.FindAsync(id);
            if (tenantType == null)
            {
                return NotFound();
            }
            return View(tenantType);
        }

        // POST: TenantTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive,IsDeleted")] TenantType tenantType)
        {
            if (id != tenantType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tenantType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantTypeExists(tenantType.Id))
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
            return View(tenantType);
        }

        // GET: TenantTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenantType = await _context.TenantTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tenantType == null)
            {
                return NotFound();
            }

            return View(tenantType);
        }

        // POST: TenantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tenantType = await _context.TenantTypes.FindAsync(id);
            if (tenantType != null)
            {
                _context.TenantTypes.Remove(tenantType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TenantTypeExists(int id)
        {
            return _context.TenantTypes.Any(e => e.Id == id);
        }
    }
}

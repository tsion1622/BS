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
    public class RoomPropertyTypeOptionsController : Controller
    {
        private readonly BIMSContext _context;

        public RoomPropertyTypeOptionsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: RoomPropertyTypeOptions
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.RoomPropertyTypeOptions.Include(r => r.RoomPropertyType);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: RoomPropertyTypeOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPropertyTypeOption = await _context.RoomPropertyTypeOptions
                .Include(r => r.RoomPropertyType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomPropertyTypeOption == null)
            {
                return NotFound();
            }

            return View(roomPropertyTypeOption);
        }

        // GET: RoomPropertyTypeOptions/Create
        public IActionResult Create()
        {
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name");
            return View();
        }

        // POST: RoomPropertyTypeOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomPropertyTypeId,Name,IsActive,IsDeleted")] RoomPropertyTypeOption roomPropertyTypeOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomPropertyTypeOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name", roomPropertyTypeOption.RoomPropertyTypeId);
            return View(roomPropertyTypeOption);
        }

        // GET: RoomPropertyTypeOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPropertyTypeOption = await _context.RoomPropertyTypeOptions.FindAsync(id);
            if (roomPropertyTypeOption == null)
            {
                return NotFound();
            }
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name", roomPropertyTypeOption.RoomPropertyTypeId);
            return View(roomPropertyTypeOption);
        }

        // POST: RoomPropertyTypeOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomPropertyTypeId,Name,IsActive,IsDeleted")] RoomPropertyTypeOption roomPropertyTypeOption)
        {
            if (id != roomPropertyTypeOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomPropertyTypeOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomPropertyTypeOptionExists(roomPropertyTypeOption.Id))
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
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name", roomPropertyTypeOption.RoomPropertyTypeId);
            return View(roomPropertyTypeOption);
        }

        // GET: RoomPropertyTypeOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPropertyTypeOption = await _context.RoomPropertyTypeOptions
                .Include(r => r.RoomPropertyType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomPropertyTypeOption == null)
            {
                return NotFound();
            }

            return View(roomPropertyTypeOption);
        }

        // POST: RoomPropertyTypeOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomPropertyTypeOption = await _context.RoomPropertyTypeOptions.FindAsync(id);
            if (roomPropertyTypeOption != null)
            {
                _context.RoomPropertyTypeOptions.Remove(roomPropertyTypeOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomPropertyTypeOptionExists(int id)
        {
            return _context.RoomPropertyTypeOptions.Any(e => e.Id == id);
        }
    }
}

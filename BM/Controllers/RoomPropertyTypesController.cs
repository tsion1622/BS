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
    public class RoomPropertyTypesController : Controller
    {
        private readonly BIMSContext _context;

        public RoomPropertyTypesController(BIMSContext context)
        {
            _context = context;
        }

        // GET: RoomPropertyTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomPropertyTypes.ToListAsync());
        }

        // GET: RoomPropertyTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPropertyType = await _context.RoomPropertyTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomPropertyType == null)
            {
                return NotFound();
            }

            return View(roomPropertyType);
        }

        // GET: RoomPropertyTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomPropertyTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive,IsDeleted")] RoomPropertyType roomPropertyType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomPropertyType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomPropertyType);
        }

        // GET: RoomPropertyTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPropertyType = await _context.RoomPropertyTypes.FindAsync(id);
            if (roomPropertyType == null)
            {
                return NotFound();
            }
            return View(roomPropertyType);
        }

        // POST: RoomPropertyTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive,IsDeleted")] RoomPropertyType roomPropertyType)
        {
            if (id != roomPropertyType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomPropertyType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomPropertyTypeExists(roomPropertyType.Id))
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
            return View(roomPropertyType);
        }

        // GET: RoomPropertyTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomPropertyType = await _context.RoomPropertyTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomPropertyType == null)
            {
                return NotFound();
            }

            return View(roomPropertyType);
        }

        // POST: RoomPropertyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomPropertyType = await _context.RoomPropertyTypes.FindAsync(id);
            if (roomPropertyType != null)
            {
                _context.RoomPropertyTypes.Remove(roomPropertyType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomPropertyTypeExists(int id)
        {
            return _context.RoomPropertyTypes.Any(e => e.Id == id);
        }
    }
}

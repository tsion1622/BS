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
    public class RoomPropertiesController : Controller
    {
        private readonly BIMSContext _context;

        public RoomPropertiesController(BIMSContext context)
        {
            _context = context;
        }

        // GET: RoomProperties
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.RoomProperties.Include(r => r.Room).Include(r => r.RoomPropertyType);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: RoomProperties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomProperty = await _context.RoomProperties
                .Include(r => r.Room)
                .Include(r => r.RoomPropertyType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomProperty == null)
            {
                return NotFound();
            }

            return View(roomProperty);
        }

        // GET: RoomProperties/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description");
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name");
            return View();
        }

        // POST: RoomProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,RoomPropertyTypeId,Value,IsActive,IsDeleted")] RoomProperty roomProperty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomProperty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", roomProperty.RoomId);
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name", roomProperty.RoomPropertyTypeId);
            return View(roomProperty);
        }

        // GET: RoomProperties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomProperty = await _context.RoomProperties.FindAsync(id);
            if (roomProperty == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", roomProperty.RoomId);
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name", roomProperty.RoomPropertyTypeId);
            return View(roomProperty);
        }

        // POST: RoomProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,RoomPropertyTypeId,Value,IsActive,IsDeleted")] RoomProperty roomProperty)
        {
            if (id != roomProperty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomProperty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomPropertyExists(roomProperty.Id))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Description", roomProperty.RoomId);
            ViewData["RoomPropertyTypeId"] = new SelectList(_context.RoomPropertyTypes, "Id", "Name", roomProperty.RoomPropertyTypeId);
            return View(roomProperty);
        }

        // GET: RoomProperties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomProperty = await _context.RoomProperties
                .Include(r => r.Room)
                .Include(r => r.RoomPropertyType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomProperty == null)
            {
                return NotFound();
            }

            return View(roomProperty);
        }

        // POST: RoomProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomProperty = await _context.RoomProperties.FindAsync(id);
            if (roomProperty != null)
            {
                _context.RoomProperties.Remove(roomProperty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomPropertyExists(int id)
        {
            return _context.RoomProperties.Any(e => e.Id == id);
        }
    }
}

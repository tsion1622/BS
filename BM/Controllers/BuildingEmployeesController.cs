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
    public class BuildingEmployeesController : Controller
    {
        private readonly BIMSContext _context;

        public BuildingEmployeesController(BIMSContext context)
        {
            _context = context;
        }

        // GET: BuildingEmployees
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.BuildingEmployees.Include(b => b.Building).Include(b => b.ServiceCategory).Include(b => b.User);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: BuildingEmployees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingEmployee = await _context.BuildingEmployees
                .Include(b => b.Building)
                .Include(b => b.ServiceCategory)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingEmployee == null)
            {
                return NotFound();
            }

            return View(buildingEmployee);
        }

        // GET: BuildingEmployees/Create
        public IActionResult Create()
        {
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id");
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BuildingEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,PhoneNumber,BuildingId,UserId,ServiceCategoryId,IsActive,IsDeleted")] BuildingEmployee buildingEmployee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buildingEmployee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", buildingEmployee.BuildingId);
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id", buildingEmployee.ServiceCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", buildingEmployee.UserId);
            return View(buildingEmployee);
        }

        // GET: BuildingEmployees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingEmployee = await _context.BuildingEmployees.FindAsync(id);
            if (buildingEmployee == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", buildingEmployee.BuildingId);
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id", buildingEmployee.ServiceCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", buildingEmployee.UserId);
            return View(buildingEmployee);
        }

        // POST: BuildingEmployees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,PhoneNumber,BuildingId,UserId,ServiceCategoryId,IsActive,IsDeleted")] BuildingEmployee buildingEmployee)
        {
            if (id != buildingEmployee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buildingEmployee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingEmployeeExists(buildingEmployee.Id))
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
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Id", buildingEmployee.BuildingId);
            ViewData["ServiceCategoryId"] = new SelectList(_context.ServiceCategories, "Id", "Id", buildingEmployee.ServiceCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", buildingEmployee.UserId);
            return View(buildingEmployee);
        }

        // GET: BuildingEmployees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingEmployee = await _context.BuildingEmployees
                .Include(b => b.Building)
                .Include(b => b.ServiceCategory)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingEmployee == null)
            {
                return NotFound();
            }

            return View(buildingEmployee);
        }

        // POST: BuildingEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buildingEmployee = await _context.BuildingEmployees.FindAsync(id);
            if (buildingEmployee != null)
            {
                _context.BuildingEmployees.Remove(buildingEmployee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingEmployeeExists(int id)
        {
            return _context.BuildingEmployees.Any(e => e.Id == id);
        }
    }
}

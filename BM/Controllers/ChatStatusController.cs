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
    public class ChatStatusController : Controller
    {
        private readonly BIMSContext _context;

        public ChatStatusController(BIMSContext context)
        {
            _context = context;
        }

        // GET: ChatStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChatStatuses.ToListAsync());
        }

        // GET: ChatStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatStatus = await _context.ChatStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatStatus == null)
            {
                return NotFound();
            }

            return View(chatStatus);
        }

        // GET: ChatStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive,IsDeleted")] ChatStatus chatStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatStatus);
        }

        // GET: ChatStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatStatus = await _context.ChatStatuses.FindAsync(id);
            if (chatStatus == null)
            {
                return NotFound();
            }
            return View(chatStatus);
        }

        // POST: ChatStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive,IsDeleted")] ChatStatus chatStatus)
        {
            if (id != chatStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatStatusExists(chatStatus.Id))
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
            return View(chatStatus);
        }

        // GET: ChatStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatStatus = await _context.ChatStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatStatus == null)
            {
                return NotFound();
            }

            return View(chatStatus);
        }

        // POST: ChatStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatStatus = await _context.ChatStatuses.FindAsync(id);
            if (chatStatus != null)
            {
                _context.ChatStatuses.Remove(chatStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatStatusExists(int id)
        {
            return _context.ChatStatuses.Any(e => e.Id == id);
        }
    }
}

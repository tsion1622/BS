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
    public class ChatVersionsController : Controller
    {
        private readonly BIMSContext _context;

        public ChatVersionsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: ChatVersions
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.ChatVersions.Include(c => c.Chat);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: ChatVersions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatVersion = await _context.ChatVersions
                .Include(c => c.Chat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatVersion == null)
            {
                return NotFound();
            }

            return View(chatVersion);
        }

        // GET: ChatVersions/Create
        public IActionResult Create()
        {
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Id");
            return View();
        }

        // POST: ChatVersions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChatId,Date,OldMessage,IsActive,IsDeleted")] ChatVersion chatVersion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatVersion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Id", chatVersion.ChatId);
            return View(chatVersion);
        }

        // GET: ChatVersions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatVersion = await _context.ChatVersions.FindAsync(id);
            if (chatVersion == null)
            {
                return NotFound();
            }
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Id", chatVersion.ChatId);
            return View(chatVersion);
        }

        // POST: ChatVersions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChatId,Date,OldMessage,IsActive,IsDeleted")] ChatVersion chatVersion)
        {
            if (id != chatVersion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatVersion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatVersionExists(chatVersion.Id))
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
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Id", chatVersion.ChatId);
            return View(chatVersion);
        }

        // GET: ChatVersions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatVersion = await _context.ChatVersions
                .Include(c => c.Chat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatVersion == null)
            {
                return NotFound();
            }

            return View(chatVersion);
        }

        // POST: ChatVersions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatVersion = await _context.ChatVersions.FindAsync(id);
            if (chatVersion != null)
            {
                _context.ChatVersions.Remove(chatVersion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatVersionExists(int id)
        {
            return _context.ChatVersions.Any(e => e.Id == id);
        }
    }
}

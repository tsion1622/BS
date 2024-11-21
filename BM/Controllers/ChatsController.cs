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
    public class ChatsController : Controller
    {
        private readonly BIMSContext _context;

        public ChatsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: Chats
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.Chats.Include(c => c.ChatStatus).Include(c => c.Parent).Include(c => c.Receiver).Include(c => c.Sender);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .Include(c => c.ChatStatus)
                .Include(c => c.Parent)
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // GET: Chats/Create
        public IActionResult Create()
        {
            ViewData["ChatStatusId"] = new SelectList(_context.ChatStatuses, "Id", "Id");
            ViewData["ParentId"] = new SelectList(_context.Chats, "Id", "Id");
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SenderId,ReceiverId,ParentId,Message,ChatStatusId,Date,IsActive,IsDeleted")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChatStatusId"] = new SelectList(_context.ChatStatuses, "Id", "Id", chat.ChatStatusId);
            ViewData["ParentId"] = new SelectList(_context.Chats, "Id", "Id", chat.ParentId);
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", chat.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Id", chat.SenderId);
            return View(chat);
        }

        // GET: Chats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            ViewData["ChatStatusId"] = new SelectList(_context.ChatStatuses, "Id", "Id", chat.ChatStatusId);
            ViewData["ParentId"] = new SelectList(_context.Chats, "Id", "Id", chat.ParentId);
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", chat.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Id", chat.SenderId);
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SenderId,ReceiverId,ParentId,Message,ChatStatusId,Date,IsActive,IsDeleted")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
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
            ViewData["ChatStatusId"] = new SelectList(_context.ChatStatuses, "Id", "Id", chat.ChatStatusId);
            ViewData["ParentId"] = new SelectList(_context.Chats, "Id", "Id", chat.ParentId);
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", chat.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Id", chat.SenderId);
            return View(chat);
        }

        // GET: Chats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .Include(c => c.ChatStatus)
                .Include(c => c.Parent)
                .Include(c => c.Receiver)
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                _context.Chats.Remove(chat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}

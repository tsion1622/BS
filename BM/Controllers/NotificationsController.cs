using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using BM.Uitilities;
using BM.Models;
using Portal.Utilities;
using Microsoft.AspNetCore.SignalR;
using BM.SignalR.hub;

namespace BM.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly BIMSContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private CancellationToken message;

        public NotificationsController(BIMSContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }



        // GET: Notifications/SendToAll
        public IActionResult SendToAll()
        {
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Name");
            return View(); 
        }
        //public IActionResult Notificationlist()
        //{
           
        //     return View();
        //}


       
       
        

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToAll([Bind("Message,NotificationTypeId")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                
                var users = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();

                foreach (var user in users)
                {
                    SaveNotification(notification.Message, notification.NotificationTypeId, user.Id);
                }

                ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Name");
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification.Message);

                //return RedirectToAction(nameof(Index));
                return Json(new { success = true, message = "Notification sent to all users!" });
            }
            return Json(new { success = false, message = "Failed to send notification." });
        

    

           
        }

        private void SaveNotification(string message,int notificationTypeId,  int userId )
        {
         var  notification = new Notification
            {
                UserId = userId,
                NotificationTypeId = notificationTypeId,
                NotificationStatusId = (int)notificationStatus.Pending,
                NotificationDate = DateOnly.FromDateTime(DateTime.Now),
                Message = message,
                IsActive = true,
                IsDeleted = false
            };
             _context.Notifications.Add(notification);
            _context.SaveChanges();
        }
             
        public void MakeAsRead(int id)
        {
            var notification =  _context.Notifications.FirstOrDefault(u => u.Id == id);
           
            notification.NotificationStatusId = (int)notificationStatus.Seen;
            _context.Notifications.Update(notification);
            _context.SaveChanges();
           
        }
        // Other actions ...
    




    // GET: Notifications
    public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.Notifications.Include(n => n.NotificationStatus).Include(n => n.NotificationType).Include(n => n.User);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .Include(n => n.NotificationStatus)
                .Include(n => n.NotificationType)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            ViewData["NotificationStatusId"] = new SelectList(_context.NotificationStatuses, "Id", "Name");
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,NotificationTypeId,NotificationStatusId,NotificationDate,IsActive,IsDeleted")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationStatusId"] = new SelectList(_context.NotificationStatuses, "Id", "Name", notification.NotificationStatusId);
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Name", notification.NotificationTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", notification.UserId);
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            ViewData["NotificationStatusId"] = new SelectList(_context.NotificationStatuses, "Id", "Name", notification.NotificationStatusId);
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Name", notification.NotificationTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", notification.UserId);
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,NotificationTypeId,NotificationStatusId,NotificationDate,IsActive,IsDeleted")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
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
            ViewData["NotificationStatusId"] = new SelectList(_context.NotificationStatuses, "Id", "Name", notification.NotificationStatusId);
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationTypes, "Id", "Name", notification.NotificationTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", notification.UserId);
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .Include(n => n.NotificationStatus)
                .Include(n => n.NotificationType)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}

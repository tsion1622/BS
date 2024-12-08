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
    public class ShopRequestsController : Controller
    {
        private readonly BIMSContext _context;

        public ShopRequestsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: ShopRequests
      


        public async Task<IActionResult> Index()

        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var shopRequests = await _context.ShopRequests.Include(s => s.RequestStatus).Include(s => s.User)
                .Where(br => br.UserId == userId)
                .ToListAsync(); ;
            return View(shopRequests);
        }

        // GET: ShopRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopRequest = await _context.ShopRequests
                .Include(s => s.RequestStatus)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopRequest == null)
            {
                return NotFound();
            }

            return View(shopRequest);
        }

        // GET: ShopRequests/Create
        public IActionResult Create()
        {
            ViewData["RequestStatusId"] = new SelectList(_context.ShopRequestStatuses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: ShopRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Description,NumberOfShops,RequestStatusId,RequestDate,IsActive,IsDeleted")] ShopRequest shopRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestStatusId"] = new SelectList(_context.ShopRequestStatuses, "Id", "Name", shopRequest.RequestStatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", shopRequest.UserId);
            return View(shopRequest);
        }

        // GET: ShopRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopRequest = await _context.ShopRequests.FindAsync(id);
            if (shopRequest == null)
            {
                return NotFound();
            }
            ViewData["RequestStatusId"] = new SelectList(_context.ShopRequestStatuses, "Id", "Name", shopRequest.RequestStatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", shopRequest.UserId);
            return View(shopRequest);
        }

        // POST: ShopRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Description,NumberOfShops,RequestStatusId,RequestDate,IsActive,IsDeleted")] ShopRequest shopRequest)
        {
            if (id != shopRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopRequestExists(shopRequest.Id))
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
            ViewData["RequestStatusId"] = new SelectList(_context.ShopRequestStatuses, "Id", "Name", shopRequest.RequestStatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", shopRequest.UserId);
            return View(shopRequest);
        }

        // GET: ShopRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopRequest = await _context.ShopRequests
                .Include(s => s.RequestStatus)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopRequest == null)
            {
                return NotFound();
            }

            return View(shopRequest);
        }

        // POST: ShopRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopRequest = await _context.ShopRequests.FindAsync(id);
            if (shopRequest != null)
            {
                _context.ShopRequests.Remove(shopRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopRequestExists(int id)
        {
            return _context.ShopRequests.Any(e => e.Id == id);
        }

        public async Task<IActionResult> RequestList()
        {

            int? userId = HttpContext.Session.GetInt32("UserId");


            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to access this page.";
                return RedirectToAction("Login", "Account");
            }


            var user = await _context.Users
                 .Include(x => x.ShopRequests).ThenInclude(x => x.RequestStatus)
                   //.Include(x => x.BuildingRequests).ThenInclude(x => x.BuildingType)

                   .Include(x => x.UserRoles)
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null && user.UserRoles.Any(x => x.RoleId == 1))
            {
                var shopRequests = await _context.ShopRequests
                                                //.Where(br => br.IsActive == true)
                                                .Include(b => b.RequestStatus)
                                                //.Include(b => b.Location)
                                                .Include(b => b.RequestStatus)
                                                .Include(b => b.User)
                                                .ToListAsync();

                return View(shopRequests);
            }

            else
            {
                TempData["ErrorMessage"] = "You don't have sufficient privileges to access this page.";
                return RedirectToAction("Login", "Account");
            }






        }

        public IActionResult RequestDetail(int? id)
        {

            int? userId = HttpContext.Session.GetInt32("UserId");


            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to access this page.";
                return RedirectToAction("Login", "Account");
            }


            var user = _context.Users
                .Include(x => x.UserRoles)
                .FirstOrDefault(x => x.Id == userId);

            if (user != null && user.UserRoles.Any(x => x.RoleId == 1))
            {
                var shopRequests = _context.ShopRequests
               //.Include(b => b.BuildingType)
               //.Include(b => b.Location)
               .Include(b => b.RequestStatus)
               .Include(b => b.User)
               .FirstOrDefault(m => m.Id == id);
                if (shopRequests == null)
                {
                    return NotFound();
                }

                return View(shopRequests);
            }

            else
            {
                TempData["ErrorMessage"] = "You don't have sufficient privileges to access this page.";
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpPost]
        public async Task<IActionResult> ChangeRequestStatus(int id, int status)
        {

            var shopRequests = await _context.ShopRequests.FindAsync(id);

            if (shopRequests == null)
            {
                return NotFound();
            }

            shopRequests.RequestStatusId = status;


            _context.Update(shopRequests);
            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] = "Request status updated successfully.";
            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }

    }
}

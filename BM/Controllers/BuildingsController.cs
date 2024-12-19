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
    public class BuildingsController : Controller
    {
        private readonly BIMSContext _context;

        public BuildingsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
            var bIMSContext = _context.Buildings.Include(b => b.BuildingType).Include(b => b.City).Include(b => b.Location).Include(b => b.Owner).Include(b => b.OwnershipType).Include(b => b.User).Include(b => b.UseType);
            return View(await bIMSContext.ToListAsync());
        }

        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .Include(b => b.BuildingType)
                .Include(b => b.City)
                .Include(b => b.Location)
                .Include(b => b.Owner)
                .Include(b => b.OwnershipType)
                .Include(b => b.User)
                .Include(b => b.UseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            ViewData["BuildingTypeId"] = new SelectList(_context.BuildingTypes, "Id", "Id");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id");
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Id");
            ViewData["OwnershipTypeId"] = new SelectList(_context.OwnershipTypes, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UseTypeId"] = new SelectList(_context.UseTypes, "Id", "Id");
            return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UseTypeId,UserId,CityId,LocationId,Name,Description,ConstractionYear,NumberOfFloor,BuildingTypeId,OwnershipTypeId,OwnerId,IsActive,IsDeleted")] Building building)
        {
            if (ModelState.IsValid)
            {
                _context.Add(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingTypeId"] = new SelectList(_context.BuildingTypes, "Id", "Id", building.BuildingTypeId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", building.CityId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", building.LocationId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Id", building.OwnerId);
            ViewData["OwnershipTypeId"] = new SelectList(_context.OwnershipTypes, "Id", "Id", building.OwnershipTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", building.UserId);
            ViewData["UseTypeId"] = new SelectList(_context.UseTypes, "Id", "Id", building.UseTypeId);
            return View(building);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            ViewData["BuildingTypeId"] = new SelectList(_context.BuildingTypes, "Id", "Id", building.BuildingTypeId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", building.CityId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", building.LocationId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Id", building.OwnerId);
            ViewData["OwnershipTypeId"] = new SelectList(_context.OwnershipTypes, "Id", "Id", building.OwnershipTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", building.UserId);
            ViewData["UseTypeId"] = new SelectList(_context.UseTypes, "Id", "Id", building.UseTypeId);
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UseTypeId,UserId,CityId,LocationId,Name,Description,ConstractionYear,NumberOfFloor,BuildingTypeId,OwnershipTypeId,OwnerId,IsActive,IsDeleted")] Building building)
        {
            if (id != building.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.Id))
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
            ViewData["BuildingTypeId"] = new SelectList(_context.BuildingTypes, "Id", "Id", building.BuildingTypeId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", building.CityId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", building.LocationId);
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Id", building.OwnerId);
            ViewData["OwnershipTypeId"] = new SelectList(_context.OwnershipTypes, "Id", "Id", building.OwnershipTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", building.UserId);
            ViewData["UseTypeId"] = new SelectList(_context.UseTypes, "Id", "Id", building.UseTypeId);
            return View(building);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .Include(b => b.BuildingType)
                .Include(b => b.City)
                .Include(b => b.Location)
                .Include(b => b.Owner)
                .Include(b => b.OwnershipType)
                .Include(b => b.User)
                .Include(b => b.UseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
        
        public IActionResult Tenants(int? id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var approvedRoomRentalRequests = _context.RoomRentalRequests
                .Where(rr => rr.UserId == userId && rr.RequestStatusId == 2)
                .Select(rr => rr.RoomId)
                .ToList();

            ViewBag.ApprovedRoomRentalRequests = approvedRoomRentalRequests;


            var buildingId = _context.Rooms
                .Where(r => approvedRoomRentalRequests.Contains(r.Id))
                .Select(r => r.Floor.BuildingId)
                .Distinct()
                .ToList();


            var tenants = _context.Tenants
                .Include(x => x.Building)
                .Include(x => x.TenantType)
                .Where(x => buildingId.Contains(x.BuildingId));

            ViewData["BuildingId"] = new SelectList(_context.Buildings
                .Where(b => buildingId.Contains(b.Id)), "Id", "Name");
            ViewData["TenantTypeId"] = new SelectList(_context.TenantTypes, "Id", "Name");

            return View(tenants);
        }

        public IActionResult Tenant(int? id)
        {
            var tenant = _context.Tenants
                .Include(x => x.Building)
                .Include(x => x.RoomRentals).ThenInclude(x=>x.BusinessArea)
                 .Include(x => x.RoomRentals).ThenInclude(x => x.Room)
                 // .Include(x => x.RoomRentals).ThenInclude(x => x.Tenant)
                //.Include(x => x.Floors).ThenInclude(x => x.Rooms)
                //.Include(x => x.City)
                .Include(x => x.TenantType)
                .FirstOrDefault(m => m.Id == id);
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }


            var approvedRoomRentalRequests = _context.RoomRentalRequests
                .Where(rr => rr.UserId == userId && rr.RequestStatusId == 2)
                .Select(rr => rr.RoomId);


            var rooms = _context.Rooms
                .Include(x => x.Floor)
                    .ThenInclude(x => x.Building)
                        .ThenInclude(x => x.City)
                .Where(x => x.IsActive && approvedRoomRentalRequests.Contains(x.Id))
                .Select(s => new
                {
                    s.Id,
                    Name = $"{s.Name}/{s.Floor.Name}/{s.Floor.Building.Name}/{s.Floor.Building.City.Name}"
                });
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name");
            ViewData["BusinessAreaId"] = new SelectList(_context.BusinessAreas, "Id", "Name");
          
            return View(tenant);
        }

        
        //[HttpPost]

        //public IActionResult AddTenant(int BuildingId, int TenantTypeId, string Name, int Tin, string Description, string Contact)
        //{
        //    if (BuildingId <= 0)
        //    {
        //        return BadRequest("Invalid building ID.");
        //    }
        //    int? userId = HttpContext.Session.GetInt32("UserId");

        //    var existingtenant = _context.TenantUsers.Where(x => x.UserId == userId);
        //    if (existingtenant == null)
        //    {
        //        var tenant = new Tenant
        //        {
        //            BuildingId = BuildingId,
        //            TenantTypeId = TenantTypeId,
        //            Name = Name,
        //            Tin = Tin,
        //            Description = Description,
        //            Contact = Contact,
        //            IsActive = true
        //        };

        //        _context.Tenants.Add(tenant);
        //        _context.SaveChanges();


        //        CreateTenantUser(tenant.Id, userId.Value);

        //    }
        //    else
        //    {
        //        TempData["Error"] = "you already exists";
        //    }
        //    return Ok();
        //}
        //private void CreateTenantUser(int tenantId, int userId)
        //{
        //    //int? userId = HttpContext.Session.GetInt32("UserId");

        //    //if (!userId.HasValue)
        //    //{
        //    //    return RedirectToAction("Login", "Account");
        //    //}

        //    var tenantUser = new TenantUser
        //    {
        //        TenantId = tenantId,
        //        UserId = userId,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = userId,
        //        IsActive = true
        //    };

        //    _context.TenantUsers.Add(tenantUser);
        //    _context.SaveChanges();


        //}

        [HttpPost]
           
        public async Task<IActionResult> AddRoomRentals(int TenantId, int RoomId, float TotalPrice, int BusinessAreaId)
        {
            if (TenantId <= 0)
            {
                return BadRequest("Invalid Tenant ID.");
            }




            var roomRental = new RoomRental
            {
                TenantId = TenantId,
                RoomId = RoomId,
                TotalPrice = TotalPrice,
                BusinessAreaId = BusinessAreaId,
                StartDate = DateTime.Now,
                IsActive = true
               
            };

            _context.RoomRentals.Add(roomRental);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }

}

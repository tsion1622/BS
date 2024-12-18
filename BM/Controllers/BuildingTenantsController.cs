using BM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BM.Controllers
{
    public class BuildingTenantsController : Controller
    {
        private readonly BIMSContext _context;

        public BuildingTenantsController(BIMSContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
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


        //public IActionResult Index(int? id)
        //{

        //    int? userId = HttpContext.Session.GetInt32("UserId");

        //    if (!userId.HasValue)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var approvedRoomRentalRequests = _context.RoomRentalRequests
        //       .Where(rr => rr.UserId == userId && rr.RequestStatusId == 2)
        //       .Select(rr => rr.RoomId);
        //    ViewBag.ApprovedRoomRentalRequests = approvedRoomRentalRequests;
        //    var tenant = _context.Tenants
        //        .Include(x => x.Building)
        //        //.Include(x => x.Floors).ThenInclude(x => x.Rooms)
        //        //.Include(x => x.City)
        //        .Include(x => x.TenantType)
        //        .Where(x => x.BuildingId == id);
        //    ViewData["BuildingId"] = new SelectList(_context.Buildings, "Id", "Name");
        //    ViewData["TenantTypeId"] = new SelectList(_context.TenantTypes, "Id", "Name");
        //    return View(tenant);
        //}

        //.FirstOrDefault(x => x.Id == id);

        //if (id == null)
        //{
        //    return NotFound();
        //}


        //var building = _context.Buildings
        //    .Include(x => x.BuildingType)
        //    .Include(x => x.Floors).ThenInclude(x => x.Rooms)
        //    .Include(x => x.City)
        //    .Include(x => x.Tenants).ThenInclude(x => x.TenantType)
        //    .FirstOrDefault(x => x.Id == id); 

        //if (building == null)
        //{
        //    return NotFound();
        //}


        //int? userId = HttpContext.Session.GetInt32("UserId");
        //if (!userId.HasValue)
        //{
        //    return RedirectToAction("Login", "Account");
        //}


        //var approvedRoomRentalRequests = _context.RoomRentalRequests
        //    .Where(rr => rr.UserId == userId && rr.RequestStatusId == 2)
        //    .Select(rr => rr.RoomId);


        //var rooms = _context.Rooms
        //    .Include(x => x.Floor).ThenInclude(x => x.Building).ThenInclude(x => x.City)
        //    .Where(x => x.IsActive && approvedRoomRentalRequests.Contains(x.Id))
        //    .Select(s => new
        //    {
        //        s.Id,
        //        Name = $"{s.Name}/{s.Floor.Name}/{s.Floor.Building.Name}/{s.Floor.Building.City.Name}"
        //    }).ToList(); 




        public IActionResult Details(int? id)
        {
            var tenant = _context.Tenants
                .Include(x => x.Building)
                .Include(x => x.RoomRentals).ThenInclude(x => x.BusinessArea)
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

        //public async Task<IActionResult> AddTenant(int BuildingId, int TenantTypeId, string Name, int Tin, string Description, string Contact)
        //{
        //    if (BuildingId <= 0)
        //    {
        //        return BadRequest("Invalid building ID.");
        //    }




        //    var tenant = new Tenant
        //    {
        //        BuildingId = BuildingId,
        //        TenantTypeId = TenantTypeId,
        //        Name = Name,
        //        Tin = Tin,
        //        Description = Description,
        //        Contact = Contact,
        //        IsActive = true
        //    };

        //    _context.Tenants.Add(tenant);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}
        [HttpPost]
        public IActionResult AddTenant(int BuildingId, int TenantTypeId, string Name, int Tin, string Description, string Contact)
        {
            if (BuildingId <= 0)
            {
                return BadRequest("Invalid building ID.");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

            var existingtenant = _context.TenantUsers.Where(x => x.UserId == userId);
            if (existingtenant == null)
            {
                var tenant = new Tenant
                {
                    BuildingId = BuildingId,
                    TenantTypeId = TenantTypeId,
                    Name = Name,
                    Tin = Tin,
                    Description = Description,
                    Contact = Contact,
                    IsActive = true
                };

                _context.Tenants.Add(tenant);
                _context.SaveChanges();


                CreateTenantUser(tenant.Id, userId.Value);

            }
            else
            {
                TempData["Error"] = "you already exists";
            }
            return Ok();
        }

           


        private void CreateTenantUser(int tenantId , int userId)
        {
            //int? userId = HttpContext.Session.GetInt32("UserId");

            //if (!userId.HasValue)
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            var tenantUser = new TenantUser
            {
                TenantId = tenantId,
                UserId = userId,
                CreatedDate = DateTime.Now,
                CreatedBy = userId, 
                IsActive = true
            };

            _context.TenantUsers.Add(tenantUser);
             _context.SaveChanges(); 

           
        }
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

using BM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BM.Controllers
{
    public class BuildingListController : Controller
    {
        private readonly BIMSContext _context;

        public BuildingListController(BIMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var buildings = await _context.Buildings
                .Include(b => b.BuildingType)
                .Include(b => b.City)
                .Include(b => b.Location)
                .Include(b => b.Owner)
                .Include(b => b.OwnershipType)
                .Include(b => b.User)
                .Include(b => b.UseType)
                .ToListAsync();

            return View(buildings);
        }

        public async Task<IActionResult> Details(int? id, int? floorId)
        {
            if (id == null)
            {
                return NotFound();
            }

           
            var building = await _context.Buildings
                                          .Include(b => b.Floors)
                                              .ThenInclude(f => f.Rooms)
                                          .FirstOrDefaultAsync(b => b.Id == id.Value);

            if (building == null)
            {
                return NotFound();
            }

           
            if (floorId.HasValue)
            {
                var selectedFloor = building.Floors.FirstOrDefault(f => f.Id == floorId.Value);
                if (selectedFloor != null)
                {
                   
                    building.Floors = new List<Floor> { selectedFloor }; 
                }
            }

            return View(building);
        }


        [HttpGet]
        public async Task<IActionResult> GetRooms(int floorId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            var rooms = await _context.Rooms
                .Where(r => r.FloorId == floorId && !r.IsDeleted)
                .Include(x => x.RoomStatus)
                .Select(r => new
                {
                    roomId = r.Id,
                    r.Name,
                    RoomStatus = r.RoomStatus.Name,
                    r.Width,
                    r.Length,
                    RentalRequestId = r.RoomRentalRequests
                        .Where(x => x.IsActive && x.UserId == userId)
                        .Select(x => x.Id)
                        .FirstOrDefault() 
                })
                .ToListAsync();

            return Json(rooms);
        }
        public IActionResult saverentalRequest(int RoomId, string Description)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

          
            var existingRequest = _context.RoomRentalRequests
                .FirstOrDefault(r => r.RoomId == RoomId && r.UserId == userId && !r.IsDeleted);

            if (existingRequest == null)
            {
               
                var roomRentalRequest = new RoomRentalRequest
                {
                    RoomId = RoomId,
                    Description = Description,
                    UserId = userId.Value,
                    RequestedDate = DateTime.Now,
                    RequestStatusId = 1, 
                    IsActive = true,
                };

                _context.RoomRentalRequests.Add(roomRentalRequest);
                _context.SaveChanges();
                TempData["success"] = "Request saved!";
            }
            else
            {
                Console.WriteLine("Existing request found:");
                Console.WriteLine($"Request ID: {existingRequest.Id}, Room ID: {existingRequest.RoomId}, User ID: {existingRequest.UserId}");

                TempData["Info"] = "Request for this room already exists.";
            }

            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }
        public async Task<IActionResult> Deleterentalrequest(int id)
        {
            var roomRentalRequest = await _context.RoomRentalRequests.FirstOrDefaultAsync(x=>x.Id == id);
            if (roomRentalRequest == null ) return NotFound();
            roomRentalRequest.IsActive = false;
            roomRentalRequest.IsDeleted = true;
            _context.RoomRentalRequests.Update(roomRentalRequest);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}





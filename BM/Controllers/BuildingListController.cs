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

            // Retrieve the building along with floors and rooms
            var building = await _context.Buildings
                                          .Include(b => b.Floors)
                                              .ThenInclude(f => f.Rooms)
                                          .FirstOrDefaultAsync(b => b.Id == id.Value);

            if (building == null)
            {
                return NotFound();
            }

            // If a floorId is provided, find the specific floor and its rooms
            if (floorId.HasValue)
            {
                var selectedFloor = building.Floors.FirstOrDefault(f => f.Id == floorId.Value);
                if (selectedFloor != null)
                {
                    // Assign the selected floor's rooms to a view model 
                    // or pass them directly to the view
                    building.Floors = new List<Floor> { selectedFloor }; // Only include the selected floor
                }
            }

            return View(building);
        }
        
        
            [HttpGet]
            public async Task<IActionResult> GetRooms(int floorId)
            {
                var rooms = await _context.Rooms
                    .Where(r => r.FloorId == floorId)
                    .Select(r => new { id = r.Id, name = r.Name  }) // Ensure property names are lowercase
                    .ToListAsync();

                return Json(rooms);
            }
        }
    }





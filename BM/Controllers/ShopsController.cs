using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BM.Models;
using NuGet.Protocol;

namespace BM.Controllers
{
    public class ShopsController : Controller
    {
        private readonly BIMSContext _context;

        public ShopsController(BIMSContext context)
        {
            _context = context;
        }

        // GET: Shops
        //public async Task<IActionResult> Index()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var bIMSContext = _context.Shops.Where(x => x.UserId == userId)
        //        .Include(s => s.BusinessArea).Include(s => s.User);

        //    ViewData["BusinessAreaId"] = new SelectList(_context.BusinessAreas, "Id", "Name");
        //    ViewBag.ShopRequests = _context.ShopRequests;
        //    bool canCreateShop = _context.ShopRequests.Any(r => r.UserId == userId && r.RequestStatusId == 1);
        //    ViewBag.CanCreateShop = canCreateShop;

        //    return View(await bIMSContext.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

           
            var bIMSContext = _context.Shops.Where(x => x.UserId == userId)
                .Include(s => s.BusinessArea)
                .Include(s => s.User);

            ViewData["BusinessAreaId"] = new SelectList(_context.BusinessAreas, "Id", "Name");

           
            var shopRequests = await _context.ShopRequests
                .Where(r => r.UserId == userId) 
                .Include(r => r.RequestStatus)
                .Include(r => r.User)
                .ToListAsync();

            ViewBag.ShopRequests = shopRequests; 

            bool canCreateShop = shopRequests.Any(r => r.RequestStatusId == 1);
            ViewBag.CanCreateShop = canCreateShop;

            return View(await bIMSContext.ToListAsync()); 
        }

        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.BusinessArea)
                .Include(s => s.User)
                .Include(s => s.ShopItems).ThenInclude(x=>x.Item).ThenInclude(x=>x.ItemCategory)
                .Include(s => s.ShopItems).ThenInclude(x => x.Item).ThenInclude(x => x.ItemImages)
                .Include(s=> s.ShopLocations).ThenInclude(x => x.Room).ThenInclude(x => x.Floor).ThenInclude(x=>x.Building).ThenInclude(x => x.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

           
            var items = _context.Items.Where(x => x.UserId == userId);
            ViewData["ItemId"] = new SelectList(items, "Id", "Name");

           
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
            return View(shop);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            ViewData["BusinessAreaId"] = new SelectList(_context.BusinessAreas, "Id", "Name");
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Name,UserId,BusinessAreaId,ItemId,Description,IsActive,IsDeleted")] Shop shop)
        {
            int? sessionUserId = HttpContext.Session.GetInt32("UserId");

            if (!sessionUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            shop.UserId = sessionUserId.Value;

            var approvedShopRequest = await _context.ShopRequests
                .FirstOrDefaultAsync(sr => sr.UserId == shop.UserId && sr.RequestStatusId == 1);

            if (approvedShopRequest != null)
            {
                int shopCount = await _context.Shops.CountAsync(s => s.UserId == shop.UserId);

                if (shopCount >= approvedShopRequest.NumberOfShops)
                {
                    TempData["Error"] = "You cannot create more shops than what was approved in your Shop Request.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["Error"] = "You have no approved Shop Requests.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BusinessAreaId"] = new SelectList(_context.BusinessAreas, "Id", "Name", shop.BusinessAreaId);
            return View(shop);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            ViewData["BusinessAreaId"] = new SelectList(_context.BusinessAreas, "Id", "Name", shop.BusinessAreaId);
          
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", shop.UserId);
            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId,BusinessAreaId,ItemId,Description,IsActive,IsDeleted")] Shop shop)
        {
            if (id != shop.Id)
            {
                return NotFound();
            }
            var userId = HttpContext.Session.GetInt32("UserId");

           
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); 
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.Id))
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
            ViewData["BusinessAreaId"] = new SelectList(_context.BusinessAreas, "Id", "Name", shop.BusinessAreaId);
        
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", shop.UserId);
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.BusinessArea)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
            return _context.Shops.Any(e => e.Id == id);

        }




        [HttpPost]
        public async Task<IActionResult> AddItem(int ShopId, int ItemId, string Balance)
        {
            if (ShopId <= 0)
            {
                return BadRequest("Invalid Shop ID.");
            }

            if (ItemId <= 0)
            {
                return BadRequest("Invalid Item ID.");
            }

            if (string.IsNullOrWhiteSpace(Balance) || !decimal.TryParse(Balance, out decimal balanceValue))
            {
                return BadRequest("Balance must be a valid non-negative number.");
            }

            
            if (balanceValue < 0)
            {
                return BadRequest("Balance cannot be negative.");
            }


            var shopItem = new ShopItem 
            {
                ShopId = ShopId,
                ItemId = ItemId,
                Balance = Balance,
                IsActive = true
            };

            _context.ShopItems.Add(shopItem);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> AddShopLocation(int ShopId, int RoomId)
        {
            if (ShopId <= 0)
            {
                return BadRequest("Invalid Shop ID.");
            }

            if (RoomId <= 0)
            {
                return BadRequest("Invalid Room ID.");
            }

            var shopLocation = new ShopLocation
            {
                ShopId = ShopId,
                RoomId = RoomId,
                CeratedDate = DateTime.Now,
                IsActive = true
            };

            _context.ShopLocations.Add(shopLocation);
            await _context.SaveChangesAsync();

            return Ok();
        }




        [HttpPost]
        public async Task<IActionResult> AddEntry(int ShopItemId, DateTime EntryDate, int Quantity, int? WithdrawQuantity, float Price)
        {
           
            
            if (EntryDate == default)
            {
                return BadRequest("Entry Date is required.");
            }

            
            if (Quantity < 0)
            {
                return BadRequest("Quantity cannot be negative.");
            }

            
            if (WithdrawQuantity.HasValue && WithdrawQuantity < 0)
            {
                return BadRequest("Withdraw Quantity cannot be negative.");
            }

           
            if (Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            var itemEntry = new ItemEntry
            {
                ShopItemId = ShopItemId,
                EntryDate = EntryDate,
                Quantity = Quantity,
                WithdrawQuantity = WithdrawQuantity,
               
                Price = Price,
                IsActive = true 
            };

            _context.ItemEntries.Add(itemEntry); 
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShopItems(int Id)
        {
            var item = await _context.ShopItems.FindAsync(Id);
            //var shopItem = await _context.ShopItems.Where();
            if (item == null) return NotFound();
            item.IsActive = false;
            item.IsDeleted = true;
            _context.ShopItems.Update(item);
            await _context.SaveChangesAsync();

            return Ok();
        }


   
        [HttpPost]
        public async Task<IActionResult> EditItemEntry(int Id, int Quantity, float Price)
        {
            var itemEntry = await _context.ItemEntries.FindAsync(Id);
            if (itemEntry == null) return NotFound();

            // Update the item entry's properties
            itemEntry.Quantity = Quantity;
            itemEntry.Price = Price;

            // Save the changes to the database
            _context.ItemEntries.Update(itemEntry);
            await _context.SaveChangesAsync();


            return Ok();
        }


        public IActionResult ItemDetail(int id)
        {
            var shopItem = _context.ShopItems
                .Include(x=> x.Item).ThenInclude(x=>x.ItemImages)
                .Include(x => x.ItemEntries).ThenInclude(x=>x.ItemEntryVarations).ThenInclude(x=>x.VarationType).ThenInclude(x=>x.Varations)
                 .Include(x => x.ItemEntryVarations)
                .FirstOrDefault(x=>x.Id==id);

            //if (shopItem == null)
            //{
            //    return NotFound();
            //}
            //int? userId = HttpContext.Session.GetInt32("UserId");
            //var varationType = _context.VarationTypes.Where(x => x.ItemId == userId);
            //var varation = _context.Varations.Where(x => x.VarationTypeId == userId);
            ViewData["VarationTypeId"] = new SelectList(_context.VarationTypes, "Id", "Name");
            ViewData["VarationId"] = new SelectList(_context.Varations, "Id", "Name");
            return View(shopItem);  
        }
        
    public async Task<IActionResult> DeleteItemEntrys(int Id)
        {
            var item = await _context.ItemEntries.FindAsync(Id);
            if (item == null) return NotFound();
            item.IsActive = false;
            item.IsDeleted = true;
            _context.ItemEntries.Update(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        
 public async Task<IActionResult> DeleteItemEntryVaration(int Id)
        {
            var itementryvaration = await _context.ItemEntryVarations.FindAsync(Id);
            if (itementryvaration == null) return NotFound();
            itementryvaration.IsActive = false;
            itementryvaration.IsDeleted = true;
            _context.ItemEntryVarations.Update(itementryvaration);
            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddItemEntryVariation(int VarationTypeId, int VarationId, int ItemEntryId, float Quantity , float Price, int ShopItemId) 
        {
            
            if (Quantity < 0)
            {
                return BadRequest("Quantity cannot be negative.");
            }

            if (Price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

           
            var itemEntryVaration = new ItemEntryVaration
            {   ShopItemId = ShopItemId,
                ItemEntryId = ItemEntryId,
                VarationTypeId = VarationTypeId,
                VarationId = VarationId,
                Quantity = Quantity,
                Price = Price,
            };

            try
            {
                
                _context.ItemEntryVarations.Add(itemEntryVaration);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
               
                return BadRequest($"An error occurred while saving: {ex.InnerException?.Message}");
            }
        }

        [HttpPost]                                                 
        public async Task<IActionResult> EditItemEntryVariation(int Id, int VarationTypeId, int VarationId, float Quantity, float Price)
        {
            var itemVariation = await _context.ItemEntryVarations.FindAsync(Id);
            if (itemVariation == null) return NotFound();

            // Update the item variation's properties
            itemVariation.VarationTypeId = VarationTypeId;
            itemVariation.VarationId = VarationId;
            itemVariation.Quantity = Quantity;
            itemVariation.Price = Price;

            // Save the changes to the database
            _context.ItemEntryVarations.Update(itemVariation);
            await _context.SaveChangesAsync();

            return Ok();
        }
      
        [HttpPost]
        public async Task<IActionResult> UploadItemImage(IFormFile image,int itemId, int? itemImageId = null)
        {
            string url = "";

           
            if (image != null)
            {
                url = await UploadImage(image, "images");

                if (url == "Invalid file type" || url == "File size exceeds limit")
                {
                    TempData["Error"] = url;
                    return Redirect(Request.GetTypedHeaders().Referer.ToString());
                }

                else
                {
                   
                    var newItemImage = new ItemImage { Url = url,ItemId= itemId, IsActive = true }; 
                    await _context.ItemImages.AddAsync(newItemImage);
                }

                await _context.SaveChangesAsync();
               
            }
            else
            {
                TempData["Error"] = "No image provided.";
            }

            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }

        private async Task<string> UploadImage(IFormFile image, string dir)
        {
            string url = "";

            if (image.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return "Invalid file type";
                }

                if (image.Length > 5 * 1024 * 1024) 
                {
                    return "File size exceeds limit";
                }

                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                url = $"/{dir}/" + uniqueFileName;
            }
            return url;
        }


        [HttpPost]
    public async Task<IActionResult> Edititemimage(IFormFile image, int itemImageId)
        {
                 var itemImage = await _context.ItemImages
                .FirstOrDefaultAsync(img => img.Id == itemImageId && img.IsActive);

            
            if (itemImage == null)
            {
                return NotFound(new { message = "item image not found." });
            }
            string url = "";
            if (url != "Invalid file type" && image.Length < 5 * 1024 * 1024)
            {

                var Url = Editupload(image, "images");
                itemImage.Url = Url;
                _context.ItemImages.Update(itemImage);
                await _context.SaveChangesAsync();

                TempData["Success"] = "image updated successfully.";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }
            else
            {
               TempData["Error"] = "image error.";
            }

            return View(Url);
        }
 
    private   string Editupload (IFormFile image, string dir)
        {
            string url = "";

            if (image != null && image.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                   url= "Invalid file type";
                    return url;
                }

                if (image.Length > 5 * 1024 * 1024) // Limit file size to 5 MB
                {
                    url = "File size exceeds limi"; 
                    return url;
                }


                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }


                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(uploadFolder, uniqueFileName);


                //var oldImagePath = Path.Combine(uploadFolder, Path.GetFileName(itemImage.Url));
                //if (System.IO.File.Exists(oldImagePath))
                //{
                //    System.IO.File.Delete(oldImagePath);
                //}

                // Save the new image
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                     image.CopyTo(stream);
                }



                url= $"/{dir}/" + uniqueFileName;
            }
            return url;
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateShopRequest(int Id,  int RequestStatusId, string Description, int NumberOfShop)
        {
            
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var shopRequest = new ShopRequest
            {
                Id = Id,
                UserId = (int)userId,
                RequestStatusId = 2,
                Description = Description,
                NumberOfShops= NumberOfShop,
                RequestDate = DateTime.Now,
                IsActive = true
            };

            _context.ShopRequests.Add(shopRequest);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Shop Request created successfully!" });
        }

        [HttpPost]
        public JsonResult LoadItems(int shopId, int page, int pageSize, string itemName)
        {
            var itemsQuery = _context.ShopItems
                .Include(x => x.Item).ThenInclude(x => x.ItemImages)
                .Where(x => x.ShopId == shopId && x.IsActive);

            int totalItems = itemsQuery.Count();

            var items = itemsQuery
                .OrderBy(item => item.Item.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(s => new
                {
                    s.Id,
                    ItemName = s.Item.Name,
                    s.Item.ItemImages.FirstOrDefault()?.Url
                });
            if (!string.IsNullOrEmpty(itemName))
            {
                items = items.Where(x => x.ItemName.ToLower().StartsWith(itemName.ToLower()));
            }

            var result = new
            {
                items,
                totalItems
            };

            
            return Json(result);
        }

    }

}





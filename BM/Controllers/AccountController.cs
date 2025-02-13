using BM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BM.Controllers
{
    public class AccountController : Controller

    {
        private readonly BIMSContext _context;

        public AccountController(BIMSContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
         
        //public IActionResult Login( string username ,string password)
        //{
            //if (username != "tsi")
            //{
            //    TempData["error"] = "invalid username";
            //}
            //else { 
            //HttpContext.Session.SetString("name", username);
            //HttpContext.Session.SetString("FirstName", username);
            //}
            public ActionResult Login(string userName, string password)
            {
           
            try
                {
                    var user = _context.Users
                    .Include(u => u.UserRoles)   
                    .FirstOrDefault(u => u.UserName == userName && u.Password == password && u.IsActive && !u.IsDeleted);
                  

                if (user != null)
                    {
                    var userRole = user.UserRoles.Where(x => x.IsActive == true);
                    if (userRole.Any(x => x.RoleId == 1)) {
                        HttpContext.Session.SetString("RoleName", "Admin");
                    }
                    HttpContext.Session.SetString("userName", user.FirstName + " " + user.MiddleName);
                    HttpContext.Session.SetInt32("UserId", user.Id);
                   
                    TempData["Success"] = "Login successful!";
                    return RedirectToAction("Index", "Profile");
                    }
               
                    else 
                    {
                        TempData["Error"] = "Invalid username or password.";
                        return View();
                    }
 
            }

                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred during login.";
                return RedirectToAction("Login", "Account");
            }


        }

        public ActionResult Login2(string userName, string password)
        {
            try
            {
                var user = _context.Users
                    .Include(u => u.UserRoles)
                    .FirstOrDefault(u => u.UserName == userName && u.Password == password && u.IsActive && !u.IsDeleted);

                if (user != null)
                {
                    var userRoles = user.UserRoles.Where(x => x.IsActive == true).ToList();

                   
                    if (userRoles.Any(x => x.RoleId == 1))
                    {
                        HttpContext.Session.SetString("RoleName", "Admin");
                        HttpContext.Session.SetString("userName", user.FirstName + " " + user.MiddleName);
                        HttpContext.Session.SetInt32("UserId", user.Id);

                        TempData["Success"] = "Login successful!";
                        return RedirectToAction("Index", "Profile");
                    }
                   
                    else if (userRoles.Any(x => x.RoleId == 2))
                    {
                        HttpContext.Session.SetString("RoleName", "Guest");
                        HttpContext.Session.SetString("userName", user.FirstName + " " + user.MiddleName);
                        HttpContext.Session.SetInt32("UserId", user.Id);

                        TempData["Success"] = "Login successful!";
                        return RedirectToAction("Index", "Home"); 
                    }

                    
                }
                else
                {
                    TempData["Error"] = "Invalid username or password.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred during login.";
                return RedirectToAction("Login", "Account");
            }

            return View(); 
        }

        public IActionResult SignUp()
        {
           
                return View();
            
        }

        [HttpPost]
        
        public IActionResult SignUp(string firstName, string middleName,  string phoneNumber, string email , string lastName,int genderId ,string userName, string password)
        {
            try
            {
                var newUser = new User
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    UserName = userName,
                    Password = password,
                   
                    Email = email,
                    PhoneNumber = phoneNumber,
                    GenderId = genderId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,

                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                TempData["Success"] = " you have successfully registered!!";
                //TempData["FirstName"] = firstName;
                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Not registered.";
                return View();
            }
        }


    
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");


    }
}
}








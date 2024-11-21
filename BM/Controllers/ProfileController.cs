using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BM.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace BM.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
       
        private readonly BIMSContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProfileController(BIMSContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
           
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: UseTypes
        public IActionResult Index()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            var user = _context.Users
                .Include(x => x.Notifications)
                .Include(x => x.UserImages)
                 .Include(x => x.Gender)
                .Include(x => x.Notifications).ThenInclude(x => x.NotificationStatus)
                 .Include(x => x.Notifications).ThenInclude(x => x.NotificationType)
                .FirstOrDefault(x => x.Id == userId);
            return View(user);
        }

        // GET: UseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var useType = await _context.UseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useType == null)
            {
                return NotFound();
            }

            return View(useType);
        }

        // GET: UseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsActive,IsDeleted")] UseType useType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(useType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(useType);
        }

        // GET: UseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var useType = await _context.UseTypes.FindAsync(id);
            if (useType == null)
            {
                return NotFound();
            }
            return View(useType);
        }

        // POST: UseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsActive,IsDeleted")] UseType useType)
        {
            if (id != useType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(useType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UseTypeExists(useType.Id))
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
            return View(useType);
        }

        // GET: UseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var useType = await _context.UseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useType == null)
            {
                return NotFound();
            }

            return View(useType);
        }

        // POST: UseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var useType = await _context.UseTypes.FindAsync(id);
            if (useType != null)
            {
                _context.UseTypes.Remove(useType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UseTypeExists(int id)
        {
            return _context.UseTypes.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult Upload(string imagedescription, IFormFile image)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "No file selected for upload." });
            }


            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            var filePath = Path.Combine(uploadFolder, image.FileName);
            var url = "/images/" + image.FileName;

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            var userImage = new UserImage()
            {
                UserId = userId,
                ImageDescription = imagedescription,
                Url = url,
                IsActive = true
            };


            _context.UserImages.Add(userImage);
            _context.SaveChanges();
            TempData["Success"] = "Image Saved successfully.";

            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }
        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    var mailMessage = new MailMessage()
        //    {
        //        From = new MailAddress("cakek433@gmail.com"),
        //        Subject = subject,
        //        Body = message,
        //        IsBodyHtml = true,
        //    };

        //    mailMessage.To.Add(email);

        //    await _smtpClient.SendMailAsync(mailMessage);
        //}


        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult CheckCode(string verificationcode)
        //{

        //    var storedVerificationCode = HttpContext.Session.GetString("VerificationCode");


        //    if (string.IsNullOrWhiteSpace(verificationcode) ||
        //        string.IsNullOrWhiteSpace(storedVerificationCode) ||
        //        verificationcode != storedVerificationCode)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid verification code. Please try again.");
        //        return View(); 
        //    }


        //    TempData["SuccessMessage"] = "The verification code is valid. You can now reset your password.";
        //    return View("VerifyEmail"); 
        //}
        public async Task<IActionResult> VerifyEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Invalid email. Please try again.");
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email. Please try again.");
                return View();
            }

            ViewBag.Message = "Success! You can now reset your password.";

            var verificationCode = new Random().Next(100000, 999999).ToString();

           
            await SendPasswordResetEmail(email, verificationCode);

           
            var passwordResetCode = new PasswordResetCode
            {
                Code = verificationCode,
                UserId = user.Id,
                Email = email, 
                IsActive = true,
                IsVerifayed = false,
                IsDeleted = false,
                SentDate = DateTime.Now,
            };

            await _context.PasswordResetCodes.AddAsync(passwordResetCode);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("VerificationCode", verificationCode);
            HttpContext.Session.SetString("UserEmail", email);
            TempData["rt"] = "vcss";

            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string email, string newPassword, string confirmPassword, string verificationCode)
        {
            int userId = CheckVerficationCode(email, verificationCode);
            if (userId != 0)
            {
                if (newPassword == confirmPassword)
                {
                    ChangePassword(userId, newPassword);
                    TempData["success"] = "Password reset successfully.";
                    return RedirectToAction("login", "Account");
                }
                else
                {
                    TempData["error"] = "Passwords do not match.";
                }
            }
            else
            {
                TempData["error"] = "Invalid verification code.";
            }

            return View();
        }

        public int CheckVerficationCode(string email, string verificationCode)
        {
            var passwordResetCode = _context.PasswordResetCodes
                .FirstOrDefault(x => x.Email == email && x.Code == verificationCode);

            if (passwordResetCode != null)
            {
                return passwordResetCode.UserId; // UserId is returned if verification code is valid
            }
            return 0; // Return 0 if not valid
        }

        private void ChangePassword(int userId, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.Password = newPassword;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public async Task SendPasswordResetEmail(string email, string verificationCode)
        {
            var resetPasswordLink = "http://localhost:5151/Profile/ResetPassword";
                //Url.Action("ResetPassword", "Profile", null, Request.Scheme);

            // Create the email body with a clickable link and verification code
            //var message = $"Your verification code is: <strong>{verificationCode}</strong>.<br />" +
            //              $"Click <a href='http://localhost:5151/Profile/ResetPassword'>here</a> to reset your password.";

             var message = $"<p>Your verification code is: <strong>{verificationCode}</strong>.</p>" +
                  $"<p>Click <a href='{resetPasswordLink}'>here</a> to reset your password.</p>";


            await SendEmailAsync(email, message);
        }

        private async Task SendEmailAsync(string email, string message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("beayerlay@gmail.com"), 
                Subject = "Password Reset Verification",
                Body = message, 
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("beayerlay@gmail.com", "pxurvzigjdyidfbw");
                smtpClient.EnableSsl = true;
                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (SmtpException smtpEx)
                {
                    throw new InvalidOperationException("SMTP error occurred: " + smtpEx.Message, smtpEx);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error sending email", ex);
                }
            }
        }



        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(string email, string newPassword, string verificationCode)
        //{

        //    var storedCode = HttpContext.Session.GetString("VerificationCode");
        //    var userEmail = HttpContext.Session.GetString("UserEmail");

        //    if (verificationCode == storedCode && email.Equals(userEmail, StringComparison.OrdinalIgnoreCase))
        //    {

        //        var user = await _userManager.FindByEmailAsync(email);
        //        if (user != null)
        //        {

        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        //            if (result.Succeeded)
        //            {

        //                return RedirectToAction("ResetPasswordConfirmation");
        //            }


        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError(string.Empty, error.Description);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid verification code or email.");
        //    }

        //    return View();

        //}

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


    }
}









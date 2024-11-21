using BM.Controllers; // ensure this is the correct namespace for IEmailService
using System.Net.Mail;
using System.Net;

namespace BM.Models
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpClientFactory _smtpClientFactory;

        public EmailService(ISmtpClientFactory smtpClientFactory)
        {
            _smtpClientFactory = smtpClientFactory;
        }

        public async Task SendEmailAsync(string email, string verificationCode)
        {
            using (var smtpClient = _smtpClientFactory.CreateClient())
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("cakek433@gmail.com"); 
                mailMessage.To.Add(email);
                mailMessage.Subject = "Password Reset Verification";
                mailMessage.Body = $"Your verification code is: {verificationCode}.";
                mailMessage.IsBodyHtml = true;

                
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
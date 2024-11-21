using BM.Controllers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BM.Models
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        private readonly EmailSettings _emailSettings;

        public SmtpClientFactory(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public SmtpClient CreateClient()
        {
            return new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true,
            };
        }
    }
}

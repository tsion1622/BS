using System.Net.Mail;

namespace BM.Controllers
{
    public interface ISmtpClientFactory
    {
        SmtpClient CreateClient();
    }


}
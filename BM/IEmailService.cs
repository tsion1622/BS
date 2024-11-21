namespace BM.Controllers
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string verificationCode);
    }
}
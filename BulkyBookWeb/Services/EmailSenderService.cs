using System.Net;
using System.Net.Mail;

namespace BulkyBookWeb.Services
{
    public interface IEmailSenderService
    {
        Task SendEmail(string recipient, string subject, string body);

    }
    public class EmailSenderService : IEmailSenderService
    {
        public async Task SendEmail(string recipient, string subject, string body)
        {
            var email = "itcs333projectgroup@gmail.com";
            var password = "asljbcsnzxfypfqa";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(email, password)
            };

            await client.SendMailAsync(
                new MailMessage( from: email,
                                to: recipient,
                                subject,
                                body
                    )
                );
        }
    }
}

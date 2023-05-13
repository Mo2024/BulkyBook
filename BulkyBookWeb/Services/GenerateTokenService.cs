using System.Net;
using System.Net.Mail;

namespace BulkyBookWeb.Services
{
    public interface IGenerateTokenService
    {
        Guid GenerateToken();

    }
    public class GenerateTokenService : IGenerateTokenService
    {
        public Guid GenerateToken()

        {
            Guid token = Guid.NewGuid();
            return token;
        }
    }
}

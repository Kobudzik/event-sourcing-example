using System.Threading.Tasks;

namespace EventSourcingExample.Infrastructure.Email.Core
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string text);
    }
}
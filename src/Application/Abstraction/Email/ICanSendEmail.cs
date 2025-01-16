using System.Threading.Tasks;

namespace EventSourcingExample.Application.Abstraction.Email
{
    public interface ICanSendEmail
    {
        Task SendEmail(string receiverEmail);
    }
}
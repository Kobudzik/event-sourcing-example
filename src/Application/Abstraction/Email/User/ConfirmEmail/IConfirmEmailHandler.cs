using System.Threading.Tasks;

namespace EventSourcingExample.Application.Abstraction.Email.User.ConfirmEmail
{
    public interface IConfirmEmailHandler: ICanSendEmail
    {
        void SetTemplateData(string username, string userId, string token);
    }
}
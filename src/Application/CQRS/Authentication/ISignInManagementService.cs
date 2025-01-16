using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Authentication
{
    public interface ISignInManagementService
    {
        Task<SignInResultStatus> SignInAsync(string userName, string password);
        Task SignOutAsync();
        Task SaveLogForFailedLoginAttemptAsync(string userName, CancellationToken cancellationToken = default);
    }
}
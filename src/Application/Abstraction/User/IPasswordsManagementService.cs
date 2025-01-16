using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.Abstraction.User
{
    public interface IPasswordsManagementService
    {
        Task<string> GenerateResetUserPasswordTokenAsync(Guid userPublicId);
        Task<string> GenerateConfirmAccountTokenAsync(Guid userPublicId);
        Task<string> GetResetPasswordTokenAsync(Guid userPublicId);
        Task<bool> ChangePasswordAsync(Guid userPublicId, string newPassword, string resetPasswordToken, CancellationToken cancellationToken = default);
        Task<bool> ChangeUserPasswordAsync(Guid userPublicId, string newPassword, CancellationToken cancellationToken = default);
        Task ConfirmUserEmailAndSetPasswordAsync(Guid userPublicId, string token, string password);
    }
}
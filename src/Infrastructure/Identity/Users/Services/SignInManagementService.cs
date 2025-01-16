using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.CQRS.Authentication;
using EventSourcingExample.Domain.Entities.Core;
using EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens;
using EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingExample.Infrastructure.Identity.Users.Services
{
    internal class SignInManagementService : ISignInManagementService
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly SignInManager<UserAccount> signInManager;
        private readonly ITokenStoreManager tokenStoreManager;
        private readonly UserManager<UserAccount> userManager;

        public SignInManagementService(
            SignInManager<UserAccount> signInManager,
            UserManager<UserAccount> userManager,
            ITokenStoreManager tokenStoreManager,
            IRefreshTokenRepository refreshTokenRepository,
            ICurrentUserService currentUserService
        )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenStoreManager = tokenStoreManager;
            this.refreshTokenRepository = refreshTokenRepository;
            this.currentUserService = currentUserService;
        }

        public async Task<SignInResultStatus> SignInAsync(string userName, string password)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user is null)
                return SignInResultStatus.Failure;

            if(!user.IsActive)
                return SignInResultStatus.AccountDisactivated;

            var signInResult = await signInManager.PasswordSignInAsync(user, password, true, false);

            if (signInResult.IsLockedOut) return SignInResultStatus.Locked;
            if (!signInResult.Succeeded) return SignInResultStatus.Failure;

            return SignInResultStatus.Success;
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
            await tokenStoreManager.DeactivateCurrentAsync();
            var userPublicId = currentUserService.UserGuid();
            await refreshTokenRepository.RemoveForUserAsync(userPublicId);
        }

        public async Task SaveLogForFailedLoginAttemptAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            if (user is null) return;
            //todo failure register
            await userManager.UpdateAsync(user);
        }
    }
}
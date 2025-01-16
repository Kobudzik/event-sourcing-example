using System;
using System.Threading.Tasks;
using EventSourcingExample.Domain.Entities.Core;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens.Repository
{
    internal interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByUserPublicIdOrDefaultAsync(Guid userPublicId);
        Task AddAsync(RefreshToken refreshToken);
        Task RemoveAsync(RefreshToken refreshToken);
        Task RemoveForUserAsync(Guid userPublicId);
    }
}
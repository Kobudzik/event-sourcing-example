using System;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Authentication
{
    public interface IRefreshTokenGenerationService
    {
        Task<string> GenerateRefreshTokenAsync(Guid userPublicId);
        Task<bool> HasValidRefreshTokenAsync(Guid userPublicId, string refreshToken);
    }
}
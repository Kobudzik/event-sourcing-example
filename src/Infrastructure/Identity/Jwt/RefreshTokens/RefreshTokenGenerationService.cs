using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EventSourcingExample.Application.CQRS.Authentication;
using EventSourcingExample.Domain.Entities.Core;
using EventSourcingExample.Infrastructure.Identity.Jwt.Configuration;
using EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens.Repository;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens
{
    internal sealed class RefreshTokenGenerationService : IRefreshTokenGenerationService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenConfiguration _tokenConfiguration;

        public RefreshTokenGenerationService(IRefreshTokenRepository refreshTokenRepository, ITokenConfiguration tokenConfiguration)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenConfiguration = tokenConfiguration;
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userPublicId)
        {
            //todo usuwanie tokena
            //await RemoveTokenFromDbForUserIfExists(userPublicId);

            var refreshTokenString = GenerateRandomRefreshToken();
            await SaveTokenInDb(userPublicId, refreshTokenString);

            return refreshTokenString;
        }

        public async Task<bool> HasValidRefreshTokenAsync(Guid userPublicId, string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByUserPublicIdOrDefaultAsync(userPublicId);
            if (userRefreshToken == null)
                return false;

            return userRefreshToken.Token == refreshToken && userRefreshToken.ExpiryDate > DateTime.Now;
        }

        private static string GenerateRandomRefreshToken()
        {
            var randomNumber = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshTokenString = Convert.ToBase64String(randomNumber);

            return refreshTokenString;
        }

        private async Task SaveTokenInDb(Guid userPublicId, string refreshTokenString)
        {
            var refreshToken = new RefreshToken
            {
                Token = refreshTokenString,
                UserPublicId = userPublicId,
                ExpiryDate = DateTime.Now.AddMinutes(_tokenConfiguration.RefreshExpirationInMinutes)
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
        }

        private async Task RemoveTokenFromDbForUserIfExists(Guid userPublicId)
        {
            var existingRefreshToken = await _refreshTokenRepository.GetByUserPublicIdOrDefaultAsync(userPublicId);

            if (existingRefreshToken != null)
                await _refreshTokenRepository.RemoveAsync(existingRefreshToken);
        }
    }
}
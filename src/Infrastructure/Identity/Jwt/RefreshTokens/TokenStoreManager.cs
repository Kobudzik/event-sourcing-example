﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingExample.Infrastructure.Identity.Jwt.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens
{
    public class TokenStoreManager : ITokenStoreManager
    {
        private readonly IDistributedCache cache;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITokenConfiguration tokenConfiguration;

        public TokenStoreManager(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, ITokenConfiguration tokenConfiguration)
        {
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
            this.tokenConfiguration = tokenConfiguration;
        }

        public async Task<bool> IsCurrentTokenActiveAsync() => await IsTokenActiveAsync(GetCurrent());

        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrent());

        private async Task<bool> IsTokenActiveAsync(string token) => await cache.GetStringAsync(GetKey(token)) == null;

        private async Task DeactivateAsync(string token)
            => await cache.SetStringAsync(GetKey(token), " ", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(tokenConfiguration.AccessExpirationInMinutes)
            });

        private string GetCurrent()
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["authorization"];

            return authorizationHeader == string.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token) => $"tokens:{token}:deactivated";
    }
}
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using System.Linq;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Domain.Enums.Core;

namespace EventSourcingExample.WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public Guid UserGuid()
        {
            var userPublicId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return !string.IsNullOrEmpty(userPublicId) ? Guid.Parse(userPublicId) : Guid.Empty;
        }

        public string[] GetCurrentUserRoles()
        {
            var roles = _httpContextAccessor.HttpContext?.User?
                .FindAll(ClaimTypes.Role)
                .Select(r => r.Value)
                .ToArray();

            return roles;
        }

        public bool IsAdmin
            => GetCurrentUserRoles().Contains(nameof(UserRoles.Administrator));

        public bool IsSeller
            => GetCurrentUserRoles().Contains(nameof(UserRoles.Seller));
    }
}

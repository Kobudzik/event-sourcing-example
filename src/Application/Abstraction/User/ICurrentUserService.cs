using System;

namespace EventSourcingExample.Application.Abstraction.User
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        Guid UserGuid();
        string[] GetCurrentUserRoles();
        bool IsAdmin { get; }
        bool IsSeller { get; }
    }
}
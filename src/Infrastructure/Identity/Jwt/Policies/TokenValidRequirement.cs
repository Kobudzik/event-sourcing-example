using Microsoft.AspNetCore.Authorization;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.Policies
{
    public class TokenValidRequirement : IAuthorizationRequirement
    {
    }
}
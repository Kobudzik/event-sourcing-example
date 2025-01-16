using Microsoft.AspNetCore.Authorization;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.Policies
{
    public class TokenValidAuthorizeAttribute : AuthorizeAttribute
    {
        public TokenValidAuthorizeAttribute(string roles)
        {
            Policy = PolicyNameKeys.TokenValid;
            Roles = roles;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;

namespace EventSourcingExample.Application.CQRS.Authentication
{
    public interface IJwtTokenManagementService
    {
        string GenerateJwtToken(UserDto userDetails, IList<string> roles);
        Guid GetUserIdFromToken(string accessToken);
        Task<string> ReissueToken(string oldTokenString);
    }
}
using System.Collections.Generic;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;

namespace EventSourcingExample.Application.CQRS.Authentication
{
    public interface ITokenGenerationService
    {
        string GenerateToken(UserDto userDetails, IEnumerable<string> roles);
    }
}
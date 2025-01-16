using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EventSourcingExample.Application.CQRS.Authentication;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using EventSourcingExample.Infrastructure.Identity.Jwt.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens
{
    //nieużwane?
    public class TokenGenerationService : ITokenGenerationService
    {
        private readonly ITokenConfiguration tokenConfiguration;

        public TokenGenerationService(ITokenConfiguration tokenConfiguration)
        {
            this.tokenConfiguration = tokenConfiguration;
        }

        public string GenerateToken(UserDto userDetails, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDetails.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userDetails.Id.ToString()),
                new Claim(ClaimTypes.Email, userDetails.Email),
                new Claim(ClaimTypes.GivenName, $"{userDetails.FirstName} {userDetails.LastName}"),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfiguration.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(tokenConfiguration.Issuer,
                tokenConfiguration.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(tokenConfiguration.AccessExpirationInMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
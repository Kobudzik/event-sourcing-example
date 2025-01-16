using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.Common.Exceptions;
using EventSourcingExample.Application.CQRS.Authentication;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using EventSourcingExample.Infrastructure.Identity.Jwt.Claims;
using EventSourcingExample.Infrastructure.Identity.Jwt.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens
{
    internal sealed class JwtTokenManagementService : IJwtTokenManagementService
    {
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
        private readonly ITokenConfiguration tokenConfiguration;
        private readonly IUserManagementService userManagementService;

        public JwtTokenManagementService(ITokenConfiguration tokenConfiguration, IUserManagementService userManagementService)
        {
            this.tokenConfiguration = tokenConfiguration;
            jwtSecurityTokenHandler ??= new JwtSecurityTokenHandler();
            this.userManagementService = userManagementService;
        }

        public Guid GetUserIdFromToken(string accessToken)
        {
            var principal = jwtSecurityTokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfiguration.Secret)),
                //ValidIssuer = tokenConfiguration.Issuer,
                //ValidAudience = tokenConfiguration.Audience,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false // we do not validate lifetime - token can be expired and we will generate new one based on refresh token
            }, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var userId = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(userId);
        }

        public string GenerateJwtToken(UserDto userDetails, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userDetails.Id.ToString()),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Email, userDetails.Email),
                new(CustomClaimTypes.FirstName, !string.IsNullOrEmpty(userDetails.FirstName) ? userDetails.FirstName : ""),
                new(CustomClaimTypes.LastName, !string.IsNullOrEmpty(userDetails.LastName) ? userDetails.LastName : ""),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            SigningCredentials signingCredentials = GetSigningCredentials();

            var token = new JwtSecurityToken(tokenConfiguration.Issuer,
                tokenConfiguration.Audience,
                claims,
                signingCredentials: signingCredentials,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(tokenConfiguration.AccessExpirationInMinutes)
            );

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public async Task<string> ReissueToken(string oldTokenString)
        {
            if (string.IsNullOrEmpty(oldTokenString))
                throw new DomainLogicException("Wrong token");

            var oldToken = new JwtSecurityTokenHandler().ReadJwtToken(oldTokenString[7..]); // trim 'Bearer ' from the start
            var user = await userManagementService.GetUserDetailsAsync(Guid.Parse(oldToken.Subject));

            if(!user.IsActive)
                throw new DomainLogicException("User account is not active!");

            var token = new JwtSecurityToken(tokenConfiguration.Issuer,
                tokenConfiguration.Audience,
                oldToken.Claims,
                signingCredentials: GetSigningCredentials(),
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(tokenConfiguration.AccessExpirationInMinutes)
            );

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfiguration.Secret));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}
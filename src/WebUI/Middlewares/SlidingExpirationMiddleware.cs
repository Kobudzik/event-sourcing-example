using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using EventSourcingExample.Application.CQRS.Authentication;

namespace EventSourcingExample.WebUI.Middlewares
{
    public class SlidingExpirationMiddleware(RequestDelegate next,
        ILogger<SlidingExpirationMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<SlidingExpirationMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, IJwtTokenManagementService jwtTokenManagementService)
        {
            try
            {
                string tokenString = context.Request.Headers.Authorization;

                JwtSecurityToken token = null;
                if (!string.IsNullOrEmpty(tokenString) && tokenString.StartsWith("Bearer"))
                    token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString[7..]); // trim 'Bearer ' from the start

                if (token != null && token.ValidTo > DateTime.UtcNow) //if is still valid
                {
                    TimeSpan timeElapsed = DateTime.UtcNow.Subtract(token.ValidFrom);
                    TimeSpan timeRemaining = token.ValidTo.Subtract(DateTime.UtcNow);

                    if (timeRemaining < timeElapsed) //if more than half of the timeout interval has elapsed
                        context.Response.Headers.Append("Set-Authorization", await jwtTokenManagementService.ReissueToken(tokenString));
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, message: e.Message);
            }

            await _next(context);
        }
    }
}
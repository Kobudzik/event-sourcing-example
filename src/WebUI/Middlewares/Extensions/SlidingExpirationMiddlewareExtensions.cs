using Microsoft.AspNetCore.Builder;

namespace EventSourcingExample.WebUI.Middlewares.Extensions
{
    public static class SlidingExpirationMiddlewareExtensions
    {
        public static IApplicationBuilder UseSlidingExpiration(this IApplicationBuilder builder)
            => builder.UseMiddleware<SlidingExpirationMiddleware>();
    }
}
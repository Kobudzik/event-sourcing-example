using EventSourcingExample.Application.Abstraction.Email.Authentication.ResetPassword;
using EventSourcingExample.Infrastructure.Email.Modules.Authentication.ResetPasswordEmail;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingExample.Infrastructure.Email.Modules.Authentication
{
    internal static class EmailAuthModule
    {
        internal static IServiceCollection AddAuthEmails(this IServiceCollection services)
        {
            services.AddScoped<IResetPasswordEmailHandler, ResetPasswordEmailHandler>();
            return services;
        }
    }
}
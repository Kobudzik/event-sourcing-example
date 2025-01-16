using EventSourcingExample.Application.Abstraction.Email.User.ConfirmEmail;
using EventSourcingExample.Infrastructure.Email.Modules.User.ConfirmEmail;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingExample.Infrastructure.Email.Modules.User
{
    internal static class EmailUserModule
    {
        internal static IServiceCollection AddUserEmails(this IServiceCollection services)
        {
            services.AddScoped<IConfirmEmailHandler, ConfirmEmailHandler>();
            return services;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using EventSourcingExample.Infrastructure.Email.Core;
using EventSourcingExample.Infrastructure.Email.Modules.User;
using EventSourcingExample.Infrastructure.Email.Modules.Authentication;

namespace EventSourcingExample.Infrastructure.Email
{
    public static class EmailsModule
    {
        public static IServiceCollection AddEmailsModule(this IServiceCollection services)
        {
            services.AddEmailCore();
            services.AddUserEmails();
            services.AddAuthEmails();

            return services;
        }
    }
}
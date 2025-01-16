using EventSourcingExample.Infrastructure.Email.Core.Configuration;
using EventSourcingExample.Infrastructure.Email.Core.TemplateReader;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingExample.Infrastructure.Email.Core
{
    internal static class EmailCoreModule
    {
        internal static IServiceCollection AddEmailCore(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IMailConfiguration, MailConfiguration>();
            services.AddScoped<IMailTemplateReader, MailTemplateReader>();

            return services;
        }
    }
}
using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Application.Abstraction.Configurations;
using EventSourcingExample.Infrastructure.Common;
using EventSourcingExample.Infrastructure.Configuration;
using EventSourcingExample.Infrastructure.Identity;
using EventSourcingExample.Infrastructure.Identity.Jwt;
using EventSourcingExample.Infrastructure.Identity.Users;
using EventSourcingExample.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingExample.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Infrastructure's DI, Databases
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("CommandsRegistry"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                            //sqlOptions.MigrationsAssembly("Infrastructure.Persistence.CoreMigrations");
                        }
                    ),
                    ServiceLifetime.Transient
              );
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IDateTime, DateTimeService>();

            services.AddIdentity();

            services.AddJwtTokenAuthorizationModule(configuration);

            services.AddUserManagementModule();

            services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();

            return services;
        }
    }
}
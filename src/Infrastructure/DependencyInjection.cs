using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Application.Abstraction.Configurations;
using EventSourcingExample.Application.Abstraction.Persistence;
using EventSourcingExample.Domain.Entities.Banking;
using EventSourcingExample.Infrastructure.Common;
using EventSourcingExample.Infrastructure.Configuration;
using EventSourcingExample.Infrastructure.Identity;
using EventSourcingExample.Infrastructure.Identity.Jwt;
using EventSourcingExample.Infrastructure.Identity.Users;
using EventSourcingExample.Infrastructure.Persistence;
using EventStore.Client;
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
                    options.UseInMemoryDatabase("EventSourcingExample"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
						sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
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

            services.AddTransient(typeof(ISqlUnitOfWork), typeof(SqlUnitOfWork));

            services.AddTransient(typeof(IUnitOfWork), typeof(CompositeUnitOfWork));

            if (configuration.GetValue<bool>("UseBankingEventStore"))
            {
                services.AddScoped<IRepository<BankAccount>, EventStoreRepository<BankAccount>>();
                services.AddTransient(typeof(IEventSourcingUnitOfWork), typeof(EventSourcingUnitOfWork<BankAccount>));
                services.AddTransient(typeof(IEventStoreChangeTracker<BankAccount>), typeof(EventStoreChangeTracker<BankAccount>));

                services.AddSingleton(_ =>
				{
					var settings = EventStoreClientSettings.Create("esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false");
					return new EventStoreClient(settings);
				});
			}
            else
            {
                services.AddScoped<IRepository<BankAccount>, SqlRepository<BankAccount>>();
            }

            return services;
        }
    }
}
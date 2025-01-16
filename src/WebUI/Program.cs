using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EventSourcingExample.Domain.Entities.Core;
using EventSourcingExample.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

namespace EventSourcingExample.WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false)
              .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                //logger.LogInformation("Using configuration: " + config.GetDebugView());

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (context.Database.IsSqlServer())
                        await context.Database.MigrateAsync();

                    var userManager = services.GetRequiredService<UserManager<UserAccount>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await ApplicationDbContextSeed.SeedDefaultUserAndRolesAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}

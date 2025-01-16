using System;
using EventSourcingExample.Domain.Entities.Core;
using EventSourcingExample.Infrastructure.Identity.Users.Errors;
using EventSourcingExample.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingExample.Infrastructure.Identity
{
    internal static class IdentityModule
    {
        internal static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            return services.AddIdentity<ApplicationDbContext>();
        }

        private static IServiceCollection AddIdentity<T>(this IServiceCollection services) where T : DbContext
        {
            var lockoutOptions = new LockoutOptions
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15),
                MaxFailedAccessAttempts = 3
            };

            services.AddIdentity<UserAccount, IdentityRole>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                    config.SignIn.RequireConfirmedPhoneNumber = false;
                    config.Password.RequiredLength = 6;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Lockout = lockoutOptions;
                    config.User.RequireUniqueEmail = true;
                })
                .AddUserManager<UserManager<UserAccount>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}